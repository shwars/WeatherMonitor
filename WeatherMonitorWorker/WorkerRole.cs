using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using WeatherMonitorLib;
using Microsoft.WindowsAzure.Storage.Table;
using OpenWeatherMap;

namespace WeatherMonitorWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WeatherMonitorWorker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        OpenWeatherMapClient WeatherClient;
        string TargetCity;

        int NumMinutes = 10;

        public override bool OnStart()
        {
            // Задайте максимальное число одновременных подключений
            ServicePointManager.DefaultConnectionLimit = 12;

            bool result = base.OnStart();

            // Open Azure Storage connection
            TargetCity = CloudConfigurationManager.GetSetting("City");
            WeatherClient = new OpenWeatherMapClient(Config.OpenWeatherMapAPIKey);
            NumMinutes = int.Parse(CloudConfigurationManager.GetSetting("NumMinutes"));

            Trace.TraceInformation("WeatherMonitorWorker has been started");
            Trace.TraceInformation("Polling interval {0} mins", NumMinutes);

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WeatherMonitorWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WeatherMonitorWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: замените следующее собственной логикой.
            while (!cancellationToken.IsCancellationRequested)
            {
                var Weather = await WeatherClient.CurrentWeather.GetByName(TargetCity,MetricSystem.Metric);
                WeatherDB.Record(ReadingType.Temperature, WeatherInfoSource.WeatherService, Weather.Temperature.Value);
                WeatherDB.Record(ReadingType.Pressure, WeatherInfoSource.WeatherService, Weather.Pressure.Value);
                WeatherDB.Record(ReadingType.Humidity, WeatherInfoSource.WeatherService, Weather.Humidity.Value);
                await Task.Delay(1000*60*NumMinutes);
            }
        }
    }
}
