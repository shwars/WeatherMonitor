using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMonitorLib
{

    public enum WeatherInfoSource { WeatherService, Device };
    public enum ReadingType { Temperature, Humidity, Pressure };

    public class WeatherRecord : TableEntity
    {

        public WeatherRecord(DateTime When, double Reading, WeatherInfoSource src, ReadingType type)
        {
            this.Reading = Reading;
            this.WeatherInfoSource = src;
            this.When = When;
            this.ReadingType = type;
            PartitionKey = string.Format("{0}.{1}", When.Year, When.Month);
            RowKey = string.Format("{0}-{1}:{2}.{3}.{4},{5}:{6}", src, type, When.Year, When.Month, When.Day, When.Hour, When.Minute);
        }

        public WeatherRecord() { }

        public DateTime When { get; set; }

        public int iWeatherInfoSource
        {
            get; set;
        }
        public WeatherInfoSource WeatherInfoSource
        {
            get { return (WeatherInfoSource)iWeatherInfoSource; }
            set { iWeatherInfoSource = (int)value; }
        }

        public int iReadingType { get; set; }

        public ReadingType ReadingType
        {
            get { return (ReadingType)iReadingType; }
            set { iReadingType = (int)value; }
        }
        public double Reading { get; set; } 

    }
}
