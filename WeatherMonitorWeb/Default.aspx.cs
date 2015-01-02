using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeatherMonitorLib;

namespace WeatherMonitorWeb
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReadingType t = ReadingType.Temperature;
            switch(ddDispType.SelectedValue)
            {
                case "Temperature":
                    t = ReadingType.Temperature; break;
                case "Pressure":
                    t = ReadingType.Pressure; break;
                case "Humidity":
                    t = ReadingType.Humidity; break;
                case "Luminocity":
                    t = ReadingType.Luminocity; break;
            }
            var data = from z in WeatherDB.GetData()
                        where z.WeatherInfoSource == WeatherInfoSource.WeatherService
                        where z.ReadingType == t
                        orderby z.When descending
                        select new { When = z.When, Data = z.Reading };
            MainChart.Series[0].Points.DataBind(data, "When", "Data", "");
            var rdata = from z in WeatherDB.GetData()
                       where z.WeatherInfoSource == WeatherInfoSource.Device
                       where z.ReadingType == t
                       orderby z.When descending
                       select new { When = z.When, Data = z.Reading };
            MainChart.Series[1].Points.DataBind(rdata, "When", "Data", "");
        }
    }
}