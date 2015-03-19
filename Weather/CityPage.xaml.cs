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
        private Weather.City suggestedCity;
        private List<Weather.City> suggestedCities;

        public ImageBrush[] imageBackgrounds = new ImageBrush[40];
        public ObservableCollection<MiniForecast> Miniforecasts;
        public Weather.ForecastResults myForecast;

        int counter = 0;
        public CityPage(Weather.WeatherUndergroundAPI myWeatherApp, Weather.City suggestedCity, List<Weather.City> suggestedCities) //Object forecastResults, List<Weather> cityPages
        {
            InitializeComponent();
            searchTextBox.Text = "Enter City";
            backgroundCompile(imageBackgrounds);

            this.myWeatherApp = myWeatherApp;
            this.suggestedCity = suggestedCity;
            this.suggestedCities = suggestedCities;

            searchTextBox.IsEnabled = false;
            searchTextBox.ItemsSource = this.suggestedCities;
            searchTextBox.SelectedIndex = 0;
            searchTextBox.Focus();
            searchTextBox.IsEnabled = true;

            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/img1.jpg")));

            this.Miniforecasts = new ObservableCollection<MiniForecast>();
            this.Miniforecasts.Add(setupMiniForecast(suggestedCity));

            MiniForecastList.DataContext = Miniforecasts;

            
        }
        private MiniForecast setupMiniForecast(Weather.City suggestedCity)
        {
            MiniForecast MF = new MiniForecast();
            Weather.Forecast theForecast = new Weather.Forecast();
            getForecast(suggestedCity, theForecast);
            MF.SF = theForecast.simpleforecast;
            MF.CityName = suggestedCity.name;
            return MF;
        }
        private async void getForecast(Weather.City suggestedCity, Weather.Forecast theForecast)
        {

            theForecast = await this.myWeatherApp.getForecastForCity(suggestedCity);
            //
        }
        private void backgroundCompile(ImageBrush[] imageBackgrounds)
        {
            for (int i = 0; i < 39; i++)
            {
                imageBackgrounds[i] = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/img" + (i + 1) + ".jpg")));
            }
             
        }
        private Weather.ForecastResults returnForecastResults(Weather.ForecastResults myForecast)
        {

            Weather.Forecastday2 todayForecast = new Weather.Forecastday2();

            todayForecast.date.day = 17;
            todayForecast.date.month = 3;
            todayForecast.date.year = 2015;
            myForecast.forecast.simpleforecast.forecastday.Add(todayForecast);

            return myForecast;
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
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.Miniforecasts.Add(setupMiniForecast((City)this.searchTextBox.SelectedItem));
            }
        }
        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            this.Miniforecasts.Add(setupMiniForecast((City)this.searchTextBox.SelectedItem));
        }
    }
    public class MiniForecast
    {
        public string CityName { get; set; }
        public Weather.Simpleforecast SF { get; set; }
        //public string currentTemp { get; set; }
        //public string windspeed { get; set; }
        //public List<DayForecast> weekForecast { get; set; }
    }
    //public class DayForecast
    //{
    //    public string dayOfTheWeek { get; set; }
    //    public string month { get; set; }
    //    public string day { get; set; }
    //    public string year { get; set; }
    //    public string high { get; set; }
    //    public string low { get; set; }
    //    public string icon { get; set; }
    //    public string icon { get; set; }
    //    public string icon { get; set; }
    //    public string rainPercentage { get; set; }
    //    public string windSpeed { get; set; }
    //}
}

