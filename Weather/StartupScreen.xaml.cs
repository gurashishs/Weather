using System;
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

// Desc: Class and methods for Page to show Startup Screen and search bar
// Interactive Logic for StartupScreen.xaml
// by: Brian Stevens
namespace Weather
{
    // Startup Screen for Applicaiton, also returns to this screen when all cities are deleted from list, home screen
    public partial class StartupScreen : Page
    {
        private Weather.WeatherUndergroundAPI myWeatherApp;
        private Weather.City suggestedCity;
        private List<Weather.City> suggestedCities;
        private CityPage cityWeatherPage;
        // Desc: Constructor to set up Startup Page with no SavedCities file
        // Input: none
        // Dependancy: SavedCities.txt - text document to hold cities to be saved and reinitialized on startup
        // Output: none
        public StartupScreen()
        {
            this.cityWeatherPage = new CityPage(this);
            this.suggestedCities = new List<Weather.City>();
            this.myWeatherApp = new Weather.WeatherUndergroundAPI();
            InitializeComponent();
            setCurrentCity();
            cityWeatherPage.setWeatherAPI(this.myWeatherApp);
            
        }
        // Desc: Constructor to set up Startup Page with SavedCities file, redirects to CityPage, skips StartupPage
        // Input: List<string> savedCityNames - saved cities in SavedCities.txt
        // Dependancy: SavedCities.txt - text document to hold cities to be saved and reinitialized on startup
        // Output: none
        public StartupScreen(List<string> savedCityNames)
        {
            this.cityWeatherPage = new CityPage(this, savedCityNames);
            this.suggestedCities = new List<Weather.City>();
            this.myWeatherApp = new Weather.WeatherUndergroundAPI();
            InitializeComponent();
            cityWeatherPage.setWeatherAPI(this.myWeatherApp);
        }
        // Desc: Returns city weather page for access
        // Input: none
        // Output: CityPage - for access
        public CityPage getPage()
        {
            return this.cityWeatherPage;
        }
        // Desc: Method to get forecast names as strings
        // Input: List<string> lists of forecast names as strings
        // Output: none
        public List<string> getMiniForecastList()
        {
            return this.cityWeatherPage.getMiniForecastList();
        }
        // Desc: Sets combobox for city based on Geolookup, or the users IP connection
        // Input: none
        // Dependancy: Used when searching initial city
        // Output: none
        public async void setCurrentCity()
        {
            this.suggestedCity = await this.myWeatherApp.getCityByGeoLookup();
            if (!searchTextBox.Text.Equals(""))
                searchTextBox.Text = this.suggestedCity.name;

            this.suggestedCities.Add(suggestedCity);
            searchTextBox.IsEnabled = false;
            searchTextBox.ItemsSource = this.suggestedCities;
            searchTextBox.SelectedIndex = 0;
            searchTextBox.Focus();
            searchTextBox.IsEnabled = true;
        }
        // Desc: Method for when text changes to display options of cities to choose from
        // Input: Sender and event when user types 5 characters in search bar
        // Dependancy: Uses Tasks, will guess city and show city options in combobox
        // Output: none
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
        // Desc: Method to search city in combo box using enter key
        // Input: Sender and event when user pushes enter key in combobox
        // Dependancy: Uses tasks, triggers city search
        // Output: none
        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    int foundDuplicate = 0;
                    MiniForecast miniForecast = await cityWeatherPage.setupMiniForecast((City)this.searchTextBox.SelectedItem);
                    foreach (MiniForecast MF in cityWeatherPage.Miniforecasts)
                    {
                        if (MF.CityName == miniForecast.CityName)
                            foundDuplicate = 1;
                    }
                    if (foundDuplicate == 0)
                    {
                        cityWeatherPage.Miniforecasts.Add(miniForecast);
                        cityWeatherPage.MiniForecastList.SelectedIndex = cityWeatherPage.MiniForecastList.Items.Count - 1;
                        cityWeatherPage.MiniForecastList.Focus();
                        this.NavigationService.Navigate(cityWeatherPage);
                    }
                }
                catch (Exception t)
                {

                }
            }
        }
        // Desc: Method to search city in combo box when search button pressed
        // Input: Sender and event when user pushes search button
        // Dependancy: Uses tasks, triggers city search
        // Output: none
        private async void myButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int foundDuplicate = 0;
                MiniForecast miniForecast = await cityWeatherPage.setupMiniForecast((City)this.searchTextBox.SelectedItem);
                foreach (MiniForecast MF in cityWeatherPage.Miniforecasts)
                {
                    if (MF.CityName == miniForecast.CityName)
                        foundDuplicate = 1;
                }
                if (foundDuplicate == 0)
                {
                    cityWeatherPage.Miniforecasts.Add(miniForecast);
                    cityWeatherPage.MiniForecastList.SelectedIndex = cityWeatherPage.MiniForecastList.Items.Count - 1;
                    cityWeatherPage.MiniForecastList.Focus();
                    this.NavigationService.Navigate(cityWeatherPage);
                }
            }
            catch(Exception t){
                
            }
        }
        public string suggestedCityString { get; set; }
    }
}
