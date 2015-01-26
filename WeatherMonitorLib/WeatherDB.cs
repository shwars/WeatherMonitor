using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMonitorLib
{
    public static class WeatherDB
    {
        static CloudStorageAccount CloudStore;
        static CloudTableClient CloudTable;
        public static CloudTable Table;

        static WeatherDB()
        {
            CloudStore = CloudStorageAccount.Parse(Config.DBConnectionString);
            CloudTable = CloudStore.CreateCloudTableClient();
            Table = CloudTable.GetTableReference(Config.TableName);
            Table.CreateIfNotExists();
        }

        public static DateTime CurrentDateTime
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.CreateCustomTimeZone("Msk", TimeSpan.FromHours(3), "Msk", "Msk")); }
        }

        public static async void Record(ReadingType t, WeatherInfoSource src, double x)
        {
            var rd = new WeatherRecord(CurrentDateTime, x, src, t);
            await Table.ExecuteAsync(TableOperation.Insert(rd));
        }

        public static async Task<bool> RecordSuccess(ReadingType t, WeatherInfoSource src, double x)
        {
            var rd = new WeatherRecord(CurrentDateTime, x, src, t);
            await Table.ExecuteAsync(TableOperation.Insert(rd));
            return true;
        }

        public static IEnumerable<WeatherRecord> GetData()
        {
            var q = new TableQuery<WeatherRecord>();
            return WeatherDB.Table.ExecuteQuery(q);
        }

        // Returns current (most recent) readings for the given reading type
        // but only within last 10 minutes.
        public static WeatherRecord[] GetCurrentReading(ReadingType type)
        {
            var data = (from z in WeatherDB.Table.CreateQuery<WeatherRecord>()
                       where z.When > DateTime.Now.AddMinutes(-10)
                       // where z.ReadingType == type
                       select z).ToList();
            var res = from z in data
                      where z.ReadingType == type
                      group z by z.WeatherInfoSource into g
                      select g.OrderByDescending(x => x.When).First();
            return res.ToArray();
        }

    }
}
