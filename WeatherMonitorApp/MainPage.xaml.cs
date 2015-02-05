using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=391641

namespace WeatherMonitorApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Подготовьте здесь страницу для отображения.

            // TODO: Если приложение содержит несколько страниц, обеспечьте
            // обработку нажатия аппаратной кнопки "Назад", выполнив регистрацию на
            // событие Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Если вы используете NavigationHelper, предоставляемый некоторыми шаблонами,
            // данное событие обрабатывается для вас.
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var ind = StatusBar.GetForCurrentView().ProgressIndicator;
            await ind.ShowAsync();
            ind.Text = "Getting temperature...";
            TempService.Text = "--"; TempOut.Text = "--";
            var temp = await GetReading("temperature");
            foreach(var x in temp)
            {
                if (x.iWeatherInfoSource == 0)
                {
                    TempService.Text = x.Reading.ToString();
                    TempServiceCmt.Text = string.Format("Forecast: {0}", x.When.ToString());
                }
                else
                {
                    TempOutCmt.Text = string.Format("Outside: {0}", x.When.ToString());
                    TempOut.Text = x.Reading.ToString();
                }
            }
            ind.Text = "Getting luminocity...";
            var lum = await GetReading("luminocity");
            int rd = 500;
            if (lum.Length > 0) rd = (int)lum[0].Reading;
            Lum.Text = rd.ToString();
            var col = new Color();
            byte rd1 = (byte)(rd / 5);
            col.R = rd1; col.G = rd1; col.B = rd1; col.A = 0;
            LumR.Fill = new SolidColorBrush(col);

            ind.Text = "Getting pressure...";
            PresService.Text = "--"; PresOut.Text = "--";
            var pres = await GetReading("pressure");
            foreach (var x in pres)
            {
                if (x.iWeatherInfoSource == 0)
                {
                    PresService.Text = ((int)x.Reading).ToString();
                    PresServiceCmt.Text = string.Format("Forecast: {0}", x.When.ToString());
                }
                else
                {
                    PresOutCmt.Text = string.Format("Outside: {0}", x.When.ToString());
                    PresOut.Text = ((int)x.Reading).ToString();
                }
            }

            await ind.HideAsync();
        }

        public async Task<SimpleWeatherRecord[]> GetReading(string rtype)
        {
            var cli = new HttpClient();
            var str = string.Format("http://weathermon.cloudapp.net/api/{0}", rtype);
            log.Text += "Getting url: " + str+"...";
            var s = await cli.GetStringAsync(new Uri(str));
            log.Text += "Done\n";
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SimpleWeatherRecord[]>(s);
        }

    }
}
