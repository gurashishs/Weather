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

            this.myWeatherApp = myWeatherApp;
            this.suggestedCity = suggestedCity;
            this.suggestedCities = suggestedCities;


            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/img1.jpg")));

            this.Miniforecasts = new ObservableCollection<MiniForecast>();
            this.Miniforecasts.Add(setupMiniForecast(suggestedCity));

            MiniForecastList.DataContext = Miniforecasts;

            backgroundCompile(imageBackgrounds);
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
            for (int i = 1; i < 40; i++)
            {
                imageBackgrounds[i] = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/img" + i + ".jpg")));
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
                Miniforecasts.Add(new MiniForecast() { CityName = "Glen Dirty", Temp = 69 });
            }
            //IEditableCollectionView items = tabControl.Items; //Cast to interface
            //if (items.CanRemove)
            //{
            //    items.Remove(tabControl.SelectedItem);
            //}
            //MiniForecastList.DataContext = Miniforecasts;
        }
        private void ReturnHome(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StartupScreen());

        }
        private void Forecast_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Background = imageBackgrounds[counter];

            //Descript.Text = AllFiles[counter];
            counter++;
            if (counter == 4)
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
                //searchingText.Text = "   ...searching weather for " + suggestedCity.name;//searchTextBox.Text;
                //this.NavigationService.Navigate(new CityPage(myWeatherApp, suggestedCity, suggestedCities));
            }
        }
        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            //searchingText.Text = "   ...searching weather for " + searchTextBox.Text;
            //this.NavigationService.Navigate(new CityPage(myWeatherApp, suggestedCity, suggestedCities));
        }
    }
    public class MiniForecast
    {
        public string CityName { get; set; }
        public Weather.Simpleforecast SF { get; set; }
        public int Temp { get; set; }
        public string WULogo { get; set; }

        //public int Low { get; set; }
    }

}

