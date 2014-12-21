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

        static string conn = "DefaultEndpointsProtocol=http;AccountName=weathermon;AccountKey=y6dh/p2udkcysNt7LXAvqVxaqVHpkXq+Xa+C/ac7vAp1+OD6+jQapXcE8W511HDJosUnIfvyn7Hxp2EnuqlU1Q==";

        static CloudStorageAccount CloudStore;
        static CloudTableClient CloudTable;
        public static CloudTable Table;

        static WeatherDB()
        {
            CloudStore = CloudStorageAccount.Parse(conn);
            CloudTable = CloudStore.CreateCloudTableClient();
            Table = CloudTable.GetTableReference("WeatherRecord");
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


    }
}
