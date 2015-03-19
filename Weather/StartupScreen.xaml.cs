﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Weather
{

    public partial class StartupScreen : Page
    {
        private Weather.WeatherUndergroundAPI myWeatherApp;
        private Weather.City suggestedCity;
        private List<Weather.City> suggestedCities;
        private CityPage cityWeatherPage = new CityPage();

        public StartupScreen()
        {
            this.suggestedCities = new List<Weather.City>();
            this.myWeatherApp = new Weather.WeatherUndergroundAPI();
            InitializeComponent();
            setCurrentCity();
            cityWeatherPage.setWeatherAPI(this.myWeatherApp);
            
        }
        

        private async void setCurrentCity()
        {
            this.suggestedCity = await this.myWeatherApp.getCityByGeoLookup();
            if (!searchingText.Equals(""))
                searchTextBox.Text = this.suggestedCity.name;

            this.suggestedCities.Add(suggestedCity);
            searchTextBox.IsEnabled = false;
            searchTextBox.ItemsSource = this.suggestedCities;
            searchTextBox.SelectedIndex = 0;
            searchTextBox.Focus();
            searchTextBox.IsEnabled = true;
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
                if (suggestedCity != null)
                {
                    cityWeatherPage.Miniforecasts.Add(await cityWeatherPage.setupMiniForecast((City)this.searchTextBox.SelectedItem));
                    this.NavigationService.Navigate(cityWeatherPage);
                }
            }
        }
        private async void myButton_Click(object sender, RoutedEventArgs e)
        {

            if (suggestedCity != null)
            {
                cityWeatherPage.Miniforecasts.Add(await cityWeatherPage.setupMiniForecast((City)this.searchTextBox.SelectedItem));
                cityWeatherPage.MiniForecastList.SelectedIndex = 0;
                cityWeatherPage.MiniForecastList.Focus();
                this.NavigationService.Navigate(cityWeatherPage);
            }
        }
        public string suggestedCityString { get; set; }
    }
}
