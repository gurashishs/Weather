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
        public void setWeatherAPI(Weather.WeatherUndergroundAPI weatherAPI){
            this.myWeatherApp = weatherAPI;
        }
        public async Task<MiniForecast> setupMiniForecast(Weather.City cityToAdd)
        {
            MiniForecast MF = new MiniForecast();
            Weather.Forecast theForecast = new Weather.Forecast();
            theForecast = await getForecast(cityToAdd);
            MF.conditions = theForecast.simpleforecast.forecastday[0].conditions;
            MF.CityName = cityToAdd.name;
            return MF;
        }
        private async Task<Forecast> getForecast(Weather.City suggestedCity)
        {
         return await this.myWeatherApp.getForecastForCity(suggestedCity);
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
            if (e.Key == Key.Return)
            {
                this.Miniforecasts.Add(await setupMiniForecast((City)this.searchTextBox.SelectedItem));
                MiniForecastList.SelectedIndex = 0;
                MiniForecastList.Focus();
            }
        }
        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            this.Miniforecasts.Add(await setupMiniForecast((City)this.searchTextBox.SelectedItem));
            MiniForecastList.SelectedIndex = 0;
            MiniForecastList.Focus();
        }
    }
    public class MiniForecast
    {
        public string CityName { get; set; }
        public string currentTemp { get; set; }
        public string conditions { get; set; }
        public string rainPercentage { get; set; }
        public string windspeed { get; set; }
        public string mean5DayTemp { get; set; }

        public string dayOfTheWeek1 { get; set; }
        public string month1 { get; set; }
        public string day1 { get; set; }
        public string icon1 { get; set; }
        public string high1 { get; set; }
        public string low1 { get; set; }
        public string conditions1 { get; set; }
        public string rainPercentage1 { get; set; }
        public string windSpeed1 { get; set; }

        public string dayOfTheWeek2 { get; set; }
        public string month2 { get; set; }
        public string day2 { get; set; }
        public string icon2 { get; set; }
        public string high2 { get; set; }
        public string low2 { get; set; }
        public string conditions2 { get; set; }
        public string rainPercentage2 { get; set; }
        public string windSpeed2 { get; set; }

        public string dayOfTheWeek3 { get; set; }
        public string month3 { get; set; }
        public string day3 { get; set; }
        public string icon3 { get; set; }
        public string high3 { get; set; }
        public string low3 { get; set; }
        public string conditions3 { get; set; }
        public string rainPercentage3 { get; set; }
        public string windSpeed3 { get; set; }

        public string dayOfTheWeek4 { get; set; }
        public string month4 { get; set; }
        public string day4 { get; set; }
        public string icon4 { get; set; }
        public string high4 { get; set; }
        public string low4 { get; set; }
        public string conditions4 { get; set; }
        public string rainPercentage4 { get; set; }
        public string windSpeed4 { get; set; }

        public string dayOfTheWeek5 { get; set; }
        public string month5 { get; set; }
        public string day5 { get; set; }
        public string icon5 { get; set; }
        public string high5 { get; set; }
        public string low5 { get; set; }
        public string conditions5 { get; set; }
        public string rainPercentage5 { get; set; }
        public string windSpeed5 { get; set; }
    }
}

