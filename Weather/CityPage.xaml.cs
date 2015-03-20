using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; 
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using System.ComponentModel;

namespace Weather
{

    public partial class CityPage : Page
    {
        private Weather.WeatherUndergroundAPI myWeatherApp;

        public ImageBrush[] imageBackgrounds = new ImageBrush[40];
        public ObservableCollection<MiniForecast> Miniforecasts = new ObservableCollection<MiniForecast>();
        public Weather.ForecastResults myForecast;

        int counter = 0;
        public CityPage() //Object forecastResults, List<Weather> cityPages
        {
            InitializeComponent();
            
            backgroundCompile(imageBackgrounds);
            
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/img1.jpg")));
      
            MiniForecastList.DataContext = Miniforecasts;
        }
        public List<string> getMiniForecastList()
        {
            List<string> saveMF = new List<string>();
            foreach (var MF in Miniforecasts)
            {
                saveMF.Add(MF.CityName);
            }
            return saveMF;
        }
        public void setWeatherAPI(Weather.WeatherUndergroundAPI weatherAPI){
            this.myWeatherApp = weatherAPI;
        }
        public async Task<MiniForecast> setupMiniForecast(Weather.City cityToAdd)
        {
            MiniForecast MF = new MiniForecast();
            Weather.Forecast the10DayForecast = new Weather.Forecast();
            the10DayForecast = await get10DayForecast(cityToAdd);
            Weather.CurrentObservation theCurrentForecast= new Weather.CurrentObservation();
            theCurrentForecast = await getCurrentForecast(cityToAdd);
            //CurrentTemp
            MF.CityName = cityToAdd.name;
            MF.WUlogo = "http://icons.wxug.com/graphics/wu2/logo_130x80.png";
            MF.BINGlogo = "http://www.adsmartonline.com/blog/wp-content/uploads/2013/01/bing.png";
            MF.MICROlogo = "http://blogs.microsoft.com/wp-content/uploads/2012/08/8867.Microsoft_5F00_Logo_2D00_for_2D00_screen.jpg";
            MF.UMBClogo = "http://upload.wikimedia.org/wikipedia/commons/6/66/UMBC_Seal.png";
            MF.currentTemp = theCurrentForecast.temp_f.ToString();
            MF.conditions = theCurrentForecast.weather;
            MF.stats = theCurrentForecast.precip_today_metric;
            MF.mean5DayTemp = "";
            //DAY 1         
            MF.date1 = the10DayForecast.simpleforecast.forecastday[1].date.weekday + the10DayForecast.simpleforecast.forecastday[1].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[1].date.day.ToString();
            MF.icon1 = the10DayForecast.simpleforecast.forecastday[1].icon_url;
            MF.temperature1 = the10DayForecast.simpleforecast.forecastday[1].high.fahrenheit + "* /" + the10DayForecast.simpleforecast.forecastday[1].low.fahrenheit + "*";
            MF.conditions1 = the10DayForecast.simpleforecast.forecastday[1].conditions;
            MF.stats1 = (the10DayForecast.simpleforecast.forecastday[1].qpf_allday.@in * 100).ToString() + "%R" + the10DayForecast.simpleforecast.forecastday[1].avewind.mph + " mph " + the10DayForecast.simpleforecast.forecastday[1].avewind.dir;
            //DAY 2
            MF.date2 = the10DayForecast.simpleforecast.forecastday[2].date.weekday + the10DayForecast.simpleforecast.forecastday[2].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[2].date.day.ToString();
            MF.icon2 = the10DayForecast.simpleforecast.forecastday[2].icon_url;
            MF.temperature2 = the10DayForecast.simpleforecast.forecastday[2].high.fahrenheit + "* /" + the10DayForecast.simpleforecast.forecastday[2].low.fahrenheit + "*";
            MF.conditions2 = the10DayForecast.simpleforecast.forecastday[2].conditions;
            MF.stats2 = (the10DayForecast.simpleforecast.forecastday[2].qpf_allday.@in * 100).ToString() + "%R" + the10DayForecast.simpleforecast.forecastday[2].avewind.mph + " mph " + the10DayForecast.simpleforecast.forecastday[2].avewind.dir;            ////DAY 3
            //DAY 3
            MF.date3 = the10DayForecast.simpleforecast.forecastday[3].date.weekday + the10DayForecast.simpleforecast.forecastday[3].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[3].date.day.ToString();
            MF.icon3 = the10DayForecast.simpleforecast.forecastday[3].icon_url;
            MF.temperature3 = the10DayForecast.simpleforecast.forecastday[3].high.fahrenheit + "* /" + the10DayForecast.simpleforecast.forecastday[3].low.fahrenheit + "*";
            MF.conditions3 = the10DayForecast.simpleforecast.forecastday[3].conditions;
            MF.stats3 = (the10DayForecast.simpleforecast.forecastday[3].qpf_allday.@in * 100).ToString() + "%R" + the10DayForecast.simpleforecast.forecastday[3].avewind.mph + " mph " + the10DayForecast.simpleforecast.forecastday[3].avewind.dir;
            //DAY 4
            MF.date4 = the10DayForecast.simpleforecast.forecastday[4].date.weekday + the10DayForecast.simpleforecast.forecastday[4].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[4].date.day.ToString();
            MF.icon4 = the10DayForecast.simpleforecast.forecastday[4].icon_url;
            MF.temperature4 = the10DayForecast.simpleforecast.forecastday[4].high.fahrenheit + "* /" + the10DayForecast.simpleforecast.forecastday[4].low.fahrenheit + "*";
            MF.conditions4 = the10DayForecast.simpleforecast.forecastday[4].conditions;
            MF.stats4 = (the10DayForecast.simpleforecast.forecastday[4].qpf_allday.@in * 100).ToString() + "%R" + the10DayForecast.simpleforecast.forecastday[4].avewind.mph + " mph " + the10DayForecast.simpleforecast.forecastday[4].avewind.dir;            ////DAY 5
            //DAY 5
            MF.date5 = the10DayForecast.simpleforecast.forecastday[5].date.weekday + the10DayForecast.simpleforecast.forecastday[5].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[5].date.day.ToString();
            MF.icon5 = the10DayForecast.simpleforecast.forecastday[5].icon_url;
            MF.temperature5 = the10DayForecast.simpleforecast.forecastday[5].high.fahrenheit + "* /" + the10DayForecast.simpleforecast.forecastday[5].low.fahrenheit + "*";
            MF.conditions5 = the10DayForecast.simpleforecast.forecastday[5].conditions;
            MF.stats5 = (the10DayForecast.simpleforecast.forecastday[5].qpf_allday.@in * 100).ToString() + "%R" + the10DayForecast.simpleforecast.forecastday[5].avewind.mph + " mph " + the10DayForecast.simpleforecast.forecastday[5].avewind.dir;            ////DAY 5



            return MF;
        }
        private async Task<Forecast> get10DayForecast(Weather.City suggestedCity)
        {
         return await this.myWeatherApp.getForecastForCity(suggestedCity);
        }
        private async Task<CurrentObservation> getCurrentForecast(Weather.City suggestedCity)
        {
            return await this.myWeatherApp.getCurrentObservationForCity(suggestedCity);
        }
        private void backgroundCompile(ImageBrush[] imageBackgrounds)
        {
            for (int i = 0; i < 39; i++)
            {
                imageBackgrounds[i] = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/img" + (i + 1) + ".jpg")));
            }
             
        }
        private void cmdDeleteUser_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is MiniForecast)
            {
                MiniForecast deleteme = (MiniForecast)cmd.DataContext;
                Miniforecasts.Remove(deleteme);
                //Miniforecasts.Add(new MiniForecast() { CityName = "Glen Dirty" });
            }
            searchTextBox.SelectedIndex = 0;
        }
        private void ReturnHome(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StartupScreen());

        }
        private void Forecast_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Background = imageBackgrounds[counter];
            counter++;
            if (counter == 39)
                counter = 0;
        }
        private async void ComboBox_City_TextChanged(object sender, TextChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.IsEnabled = false;

            int initLength = comboBox.Text.Length;
            if (comboBox.Text.Length == 5)
            {
                searchTextBox.ItemsSource = await this.myWeatherApp.getCitiesByNameQuery(comboBox.Text);
                comboBox.SelectedIndex = 0;
                comboBox.IsDropDownOpen = true;
                comboBox.Focus();
            }

            comboBox.IsEnabled = true;
        }
        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && this.searchTextBox.Text != "" && this.searchTextBox.Text.Length >= 5)
            {
                this.Miniforecasts.Add(await setupMiniForecast((City)this.searchTextBox.SelectedItem));
                MiniForecastList.SelectedIndex = 0;
                MiniForecastList.Focus();
            }
        }
        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.searchTextBox.Text != "" && this.searchTextBox.Text.Length >= 5)
            {
                this.Miniforecasts.Add(await setupMiniForecast((City)this.searchTextBox.SelectedItem));
                MiniForecastList.SelectedIndex = 0;
                MiniForecastList.Focus();
            }
        }
    }
    public class MiniForecast
    {
        public string CityName { get; set; }
        public string WUlogo { get; set; }
        public string BINGlogo { get; set; }
        public string MICROlogo { get; set; }
        public string UMBClogo { get; set; }
        public string currentTemp { get; set; }
        public string conditions { get; set; }
        public string stats { get; set; }
        public string mean5DayTemp { get; set; }

        public string date1 { get; set; }
        public string icon1 { get; set; }
        public string temperature1 { get; set; }
        public string conditions1 { get; set; }
        public string stats1 { get; set; }

        public string date2 { get; set; }
        public string icon2 { get; set; }
        public string temperature2 { get; set; }
        public string conditions2 { get; set; }
        public string stats2 { get; set; }

        public string date3 { get; set; }
        public string icon3 { get; set; }
        public string temperature3 { get; set; }
        public string conditions3 { get; set; }
        public string stats3 { get; set; }

        public string date4 { get; set; }
        public string icon4 { get; set; }
        public string temperature4 { get; set; }
        public string conditions4 { get; set; }
        public string stats4 { get; set; }

        public string date5 { get; set; }
        public string icon5 { get; set; }
        public string temperature5 { get; set; }
        public string conditions5 { get; set; }
        public string stats5 { get; set; }
    }
}

