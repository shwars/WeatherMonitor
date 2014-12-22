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
            var data = from z in WeatherDB.GetData()
                        where z.WeatherInfoSource == WeatherInfoSource.WeatherService
                        where z.ReadingType == ReadingType.Temperature
                        orderby z.When descending
                        select new { When = z.When, Data = z.Reading };
            MainChart.Series[0].Points.DataBind(data, "When", "Data", "");
        }
    }
}