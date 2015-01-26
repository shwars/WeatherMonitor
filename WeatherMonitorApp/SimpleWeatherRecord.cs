using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMonitorApp
{
    public class SimpleWeatherRecord
    {
        public DateTime When { get; set; }

        public int iWeatherInfoSource
        {
            get; set;
        }

        public int iReadingType { get; set; }

        public double Reading { get; set; }

    }
}
