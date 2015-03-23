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

// Desc: Class and methods for Page to show Cities and information
// Interactive Logic for CityPage.xaml
// by: Brian Stevens
namespace Weather
{
    // Class to display City data and hold forecast list of multiple cities
    public delegate void EventHandler();
    public partial class CityPage : Page
    {
        private Weather.WeatherUndergroundAPI myWeatherApp;
        public ImageBrush[] imageBackgrounds = new ImageBrush[40];
        public ObservableCollection<MiniForecast> Miniforecasts = new ObservableCollection<MiniForecast>();
        public Weather.ForecastResults myForecast; 
        private List<string> savedCities = new List<string>(); 
        public static event EventHandler forceUpdate; 
        StartupScreen homePage; 
        // Desc: Constructor to set up City Page after initial search
        // Input: homePage - Link to homePage(StartupScreen)
        // Dependancy: SavedCities.txt - text document to hold cities to be saved and reinitialized on startup
        // Output: none
        public CityPage(StartupScreen homePage) 
        {
            InitializeComponent();
            this.homePage = homePage;
            backgroundCompile(imageBackgrounds);

            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/WaitingPage.jpg")));

            MiniForecastList.DataContext = Miniforecasts;
        }
        // Desc: Constructor to set up City Page when SavedCities.txt has data
        // Input: homePage - Link to homePage(StartupScreen), savedCityNames - List of strings of city names to initialize
        // Dependancy: SavedCities.txt - text document to hold cities to be saved and reinitialized on startup
        // Output: none
        public CityPage(StartupScreen homePage, List<string> savedCityNames) //Object forecastResults, List<Weather> cityPages
        {
            InitializeComponent();
            this.homePage = homePage;
            backgroundCompile(imageBackgrounds);

            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/WaitingPage.jpg")));

            MiniForecastList.DataContext = Miniforecasts;
            this.savedCities = savedCityNames;
            forceUpdate += new EventHandler(setSavedCities);
            forceUpdate.Invoke();
        }
        // Desc: Method to find background string for image lookup
        // Input: string weatherCondition - condition of the weather from API, int hour - time period in military time to use night backgrounds
        // Output: string for background image lookup in Resource folder
        private string backgroundLookup(string weatherCondition, int hour)
        {

            switch (weatherCondition)
            {
                case "Chance of Flurries":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img14.jpg";
                    return "/Resources/img14.jpg";
                case "Chance of Rain":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img29.jpg";
                    return "/Resources/img8.jpg";
                case "Chance Rain":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img25.jpg";
                    return "/Resources/img9.jpg";
                case "Chance of Freezing Rain":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img11.jpg";
                    return "/Resources/img10.jpg";
                case "Chance of Sleet":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img11.jpg";
                    return "/Resources/img11.jpg";
                case "Chance of Snow":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img7.jpg";
                    return "/Resources/img7.jpg";
                case "Chance of Thunderstorms":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img20.jpg";
                    return "/Resources/img19.jpg";
                case "Chance of a Thunderstorm":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img20.jpg";
                    return "/Resources/img19.jpg";
                case "Clear":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img35.jpg";
                    return "/Resources/img34.jpg";
                case "Cloudy":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img29.jpg";
                    return "/Resources/img30.jpg";
                case "Flurries":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img5.jpg";
                    return "/Resources/img5.jpg";
                case "Fog":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img22.jpg";
                    return "/Resources/img23.jpg";
                case "Haze":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img22.jpg";
                    return "/Resources/img23.jpg";
                case "Mostly Cloudy":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img29.jpg";
                    return "/Resources/img30.jpg";
                case "Mostly Sunny":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img35.jpg";
                    return "/Resources/img28.jpg";
                case "Partly Cloudy":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img31.jpg";
                    return "/Resources/img32.jpg";
                case "Partly Sunny":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img31.jpg";
                    return "/Resources/img30.jpg";
                case "Freezing Rain":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img4.jpg";
                    return "/Resources/img4.jpg";
                case "Rain":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img37.jpg";
                    return "/Resources/img6.jpg";
                case "Sleet":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img3.jpg";
                    return "/Resources/img3.jpg";
                case "Snow":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img12.jpg";
                    return "/Resources/img12.jpg";
                case "Sunny":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img35.jpg";
                    return "/Resources/img34.jpg";
                case "Thunderstorms":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img2.jpg";
                    return "/Resources/img1.jpg";
                case "Thunderstorm":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img2.jpg";
                    return "/Resources/img1.jpg";
                case "Unknown":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img18.jpg";
                    return "/Resources/img18.jpg";
                case "Overcast":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img31.jpg";
                    return "/Resources/img17.jpg";
                case "Scattered Clouds":
                    if (hour < 7 || hour >= 19)
                        return "/Resources/img31.jpg";
                    return "/Resources/img32.jpg";
                default:
                    return "/Resources/img39.jpg";

            }
        }
        // Desc: Method to generate names of all currently searched cities for saving cities on window close
        // Input: none
        // Output: List<string> compile current forecasts names as strings
        public List<string> getMiniForecastList()
        {
            List<string> saveMF = new List<string>();
            foreach (var MF in Miniforecasts)
            {
                saveMF.Add(MF.CityName);
            }
            return saveMF;
        }
        // Desc: Method to set Weather API
        // Input: none
        // Output: none
        public void setWeatherAPI(Weather.WeatherUndergroundAPI weatherAPI)
        {
            this.myWeatherApp = weatherAPI;
        }
        // Desc: Method to setup MiniForecast for city that was searchedc
        // Input: Weather.City cityToAdd - City that will be added to forecast list
        // Dependancy: Uses tasks
        // Output: MF - mini forecast for display
        public async Task<MiniForecast> setupMiniForecast(Weather.City cityToAdd)
        {
            MiniForecast MF = new MiniForecast();
            Weather.Forecast the10DayForecast = new Weather.Forecast();
            the10DayForecast = await get10DayForecast(cityToAdd);
            Weather.CurrentObservation theCurrentForecast = new Weather.CurrentObservation();
            theCurrentForecast = await getCurrentForecast(cityToAdd);
            MF.CityName = cityToAdd.name;

            MF.WUlogo = "http://icons.wxug.com/graphics/wu2/logo_130x80.png";
            MF.BINGlogo = "http://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/Bing_logo.svg/2000px-Bing_logo.svg.png";
            MF.MICROlogo = "http://upload.wikimedia.org/wikipedia/commons/thumb/9/94/M_box.svg/2000px-M_box.svg.png";
            MF.UMBClogo = "http://cdn.bennettrank.com/wp-content/uploads/umbc-logo.png";
            MF.currentTemp = theCurrentForecast.temp_f.ToString();
            MF.dF = "°F";
            MF.conditions = theCurrentForecast.weather;
            string hour = (((theCurrentForecast.observation_time.Split(','))[1]).Split(' ')[1]).Split(':')[0];
            string AMorPM = (((theCurrentForecast.observation_time.Split(','))[1]).Split(' ')[2]);
            if ((Int32.Parse(hour) != 12 && AMorPM == "PM") || (Int32.Parse(hour) == 12 && AMorPM == "AM"))
            {
                MF.hour = Int32.Parse(hour) + 12;
            }
            else
            {
                MF.hour = Int32.Parse(hour);
            }
            MF.currentTime = " Current Time: " + (theCurrentForecast.observation_time.Split(','))[1];
            MF.Background = backgroundLookup(theCurrentForecast.weather, MF.hour);

            MF.feelsLike = "Feels Like: " + theCurrentForecast.feelslike_f + "°";
            MF.date = "Today's " + the10DayForecast.simpleforecast.forecastday[0].date.weekday + ": " + the10DayForecast.simpleforecast.forecastday[0].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[0].date.day.ToString();
            MF.stats = (the10DayForecast.simpleforecast.forecastday[0].qpf_allday.@in * 100).ToString() + "%P " + the10DayForecast.simpleforecast.forecastday[0].avewind.mph + "mph " + the10DayForecast.simpleforecast.forecastday[0].avewind.dir;
            MF.temp = the10DayForecast.simpleforecast.forecastday[0].high.fahrenheit + "° / " + the10DayForecast.simpleforecast.forecastday[0].low.fahrenheit + "°";
            MF.mean = "Mean: " + ((Int32.Parse(the10DayForecast.simpleforecast.forecastday[0].high.fahrenheit) + Int32.Parse(the10DayForecast.simpleforecast.forecastday[0].low.fahrenheit)) / 2).ToString() + "°";
            //DAY 1         
            MF.date1 = the10DayForecast.simpleforecast.forecastday[1].date.weekday + " " + the10DayForecast.simpleforecast.forecastday[1].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[1].date.day.ToString();
            MF.icon1 = the10DayForecast.simpleforecast.forecastday[1].icon_url;
            MF.temperature1 = the10DayForecast.simpleforecast.forecastday[1].high.fahrenheit + "° / " + the10DayForecast.simpleforecast.forecastday[1].low.fahrenheit + "°";
            MF.conditions1 = the10DayForecast.simpleforecast.forecastday[1].conditions;
            MF.stats1 = (the10DayForecast.simpleforecast.forecastday[1].qpf_allday.@in * 100).ToString() + "%P " + the10DayForecast.simpleforecast.forecastday[1].avewind.mph + "mph " + the10DayForecast.simpleforecast.forecastday[1].avewind.dir;
            MF.mean1 = "Mean: " + ((Int32.Parse(the10DayForecast.simpleforecast.forecastday[1].high.fahrenheit) + Int32.Parse(the10DayForecast.simpleforecast.forecastday[1].low.fahrenheit)) / 2).ToString() + "°";
            //DAY 2
            MF.date2 = the10DayForecast.simpleforecast.forecastday[2].date.weekday + " " + the10DayForecast.simpleforecast.forecastday[2].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[2].date.day.ToString();
            MF.icon2 = the10DayForecast.simpleforecast.forecastday[2].icon_url;
            MF.temperature2 = the10DayForecast.simpleforecast.forecastday[2].high.fahrenheit + "° / " + the10DayForecast.simpleforecast.forecastday[2].low.fahrenheit + "°";
            MF.conditions2 = the10DayForecast.simpleforecast.forecastday[2].conditions;
            MF.stats2 = (the10DayForecast.simpleforecast.forecastday[2].qpf_allday.@in * 100).ToString() + "%P " + the10DayForecast.simpleforecast.forecastday[2].avewind.mph + "mph " + the10DayForecast.simpleforecast.forecastday[2].avewind.dir;            ////DAY 3
            MF.mean2 = "Mean: " + ((Int32.Parse(the10DayForecast.simpleforecast.forecastday[2].high.fahrenheit) + Int32.Parse(the10DayForecast.simpleforecast.forecastday[2].low.fahrenheit)) / 2).ToString() + "°";
            //DAY 3
            MF.date3 = the10DayForecast.simpleforecast.forecastday[3].date.weekday + " " + the10DayForecast.simpleforecast.forecastday[3].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[3].date.day.ToString();
            MF.icon3 = the10DayForecast.simpleforecast.forecastday[3].icon_url;
            MF.temperature3 = the10DayForecast.simpleforecast.forecastday[3].high.fahrenheit + "° / " + the10DayForecast.simpleforecast.forecastday[3].low.fahrenheit + "°";
            MF.conditions3 = the10DayForecast.simpleforecast.forecastday[3].conditions;
            MF.stats3 = (the10DayForecast.simpleforecast.forecastday[3].qpf_allday.@in * 100).ToString() + "%P " + the10DayForecast.simpleforecast.forecastday[3].avewind.mph + "mph " + the10DayForecast.simpleforecast.forecastday[3].avewind.dir;
            MF.mean3 = "Mean: " + ((Int32.Parse(the10DayForecast.simpleforecast.forecastday[3].high.fahrenheit) + Int32.Parse(the10DayForecast.simpleforecast.forecastday[3].low.fahrenheit)) / 2).ToString() + "°";
            //DAY 4
            MF.date4 = the10DayForecast.simpleforecast.forecastday[4].date.weekday + " " + the10DayForecast.simpleforecast.forecastday[4].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[4].date.day.ToString();
            MF.icon4 = the10DayForecast.simpleforecast.forecastday[4].icon_url;
            MF.temperature4 = the10DayForecast.simpleforecast.forecastday[4].high.fahrenheit + "° / " + the10DayForecast.simpleforecast.forecastday[4].low.fahrenheit + "°";
            MF.conditions4 = the10DayForecast.simpleforecast.forecastday[4].conditions;
            MF.stats4 = (the10DayForecast.simpleforecast.forecastday[4].qpf_allday.@in * 100).ToString() + "%P " + the10DayForecast.simpleforecast.forecastday[4].avewind.mph + "mph " + the10DayForecast.simpleforecast.forecastday[4].avewind.dir;            ////DAY 5
            MF.mean4 = "Mean: " + ((Int32.Parse(the10DayForecast.simpleforecast.forecastday[4].high.fahrenheit) + Int32.Parse(the10DayForecast.simpleforecast.forecastday[4].low.fahrenheit)) / 2).ToString() + "°";
            //DAY 5
            MF.date5 = the10DayForecast.simpleforecast.forecastday[5].date.weekday + " " + the10DayForecast.simpleforecast.forecastday[5].date.month.ToString() + "/" + the10DayForecast.simpleforecast.forecastday[5].date.day.ToString();
            MF.icon5 = the10DayForecast.simpleforecast.forecastday[5].icon_url;
            MF.temperature5 = the10DayForecast.simpleforecast.forecastday[5].high.fahrenheit + "° / " + the10DayForecast.simpleforecast.forecastday[5].low.fahrenheit + "°";
            MF.conditions5 = the10DayForecast.simpleforecast.forecastday[5].conditions;
            MF.stats5 = (the10DayForecast.simpleforecast.forecastday[5].qpf_allday.@in * 100).ToString() + "%P " + the10DayForecast.simpleforecast.forecastday[5].avewind.mph + "mph " + the10DayForecast.simpleforecast.forecastday[5].avewind.dir;            ////DAY 5
            MF.mean5 = "Mean: " + ((Int32.Parse(the10DayForecast.simpleforecast.forecastday[5].high.fahrenheit) + Int32.Parse(the10DayForecast.simpleforecast.forecastday[5].low.fahrenheit)) / 2).ToString() + "°";

            return MF;
        }
        // Desc: Method to get 10 day forecast for a City
        // Input: Weather.City suggestedCity - gets Forecast for City using API
        // Dependancy: Uses tasks
        // Output: forecast for city
        private async Task<Forecast> get10DayForecast(Weather.City suggestedCity)
        {
            return await this.myWeatherApp.getForecastForCity(suggestedCity);
        }
        // Desc: Method to get current day forecast for a City
        // Input: Weather.City suggestedCity - gets Forecast for City using API
        // Dependancy: Uses tasks
        // Output: forecast for city
        private async Task<CurrentObservation> getCurrentForecast(Weather.City suggestedCity)
        {
            return await this.myWeatherApp.getCurrentObservationForCity(suggestedCity);
        }
        // Desc: Method to load all backgrounds into an array
        // Input: ImageBrush[] imageBackgrounds - imagebrush array of images for easy background lookup
        // Output: none
        private void backgroundCompile(ImageBrush[] imageBackgrounds)
        {
            for (int i = 0; i < 39; i++)
            {
                imageBackgrounds[i] = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/img" + (i + 1) + ".jpg")));
            }

        }
        // Desc: Method to delete a city for a button push on the Listbox item for given MiniForecast
        // Input: Sender and event when user clicks a delete button for City
        // Dependancy: returns to startup page if last city is deleted
        // Output: none
        private void cmdDeleteUser_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is MiniForecast && (Miniforecasts.Count > 1))
            {
                MiniForecast deleteme = (MiniForecast)cmd.DataContext;
                int indexOfDelete = MiniForecastList.Items.IndexOf(deleteme);
                if (indexOfDelete == MiniForecastList.SelectedIndex)
                {
                    MiniForecastList.SelectedIndex = indexOfDelete - 1;
                    MiniForecastList.Focus();
                }
                Miniforecasts.Remove(deleteme);

            }
            else
            {
                MiniForecast deleteme = (MiniForecast)cmd.DataContext;
                Miniforecasts.Remove(deleteme);
                homePage.setCurrentCity();
                this.NavigationService.Navigate(homePage);
                MiniForecastList.SelectedIndex = 0;
                MiniForecastList.Focus();
            }
        }
        // Desc: Method to use a return home button(Not utilized)
        // Input: Sender and event when user click a return home button
        // Dependancy: returns to startup page if button pressed
        // Output: none
        private void ReturnHome(object sender, RoutedEventArgs e)
        {
            homePage.setCurrentCity();
            this.NavigationService.Navigate(homePage);
        }
        // Desc: Method to change display data when a forecast changes
        // Input: Sender and event when user clicks an item in the MiniForecast Listbox
        // Dependancy: changes content on CityPage for selected city
        // Output: none
        private void Forecast_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = Math.Abs(MiniForecastList.SelectedIndex);
            if (Miniforecasts.Count > i)
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), Miniforecasts[i].Background)));
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
        // Desc: Method to reinitialize saved cities in SavedCities.txt
        // Input: none
        // Dependancy: Uses Tasks, sets up saved cities on CityPage, skips homePage
        // Output: none
        private async void setSavedCities()
        {
            this.myWeatherApp = new Weather.WeatherUndergroundAPI();

            foreach (string SCN in this.savedCities)
            {
                List<Weather.City> cityList = await this.myWeatherApp.getCitiesByNameQuery(SCN);
                Miniforecasts.Add(await setupMiniForecast(cityList[0]));

            }
            MiniForecastList.SelectedIndex = 0;
            MiniForecastList.Focus();
            int i = Math.Abs(MiniForecastList.SelectedIndex);
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), Miniforecasts[i].Background)));

        }
        // Desc: Method to search city in combo box using enter key
        // Input: Sender and event when user pushes enter key in combobox
        // Dependancy: Uses tasks, triggers city search
        // Output: none
        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && this.searchTextBox.Text != "" && this.searchTextBox.Text.Length >= 5)
            {
                try
                {
                    int foundDuplicate = 0;
                    MiniForecast miniForecast = await setupMiniForecast((City)this.searchTextBox.SelectedItem);
                    foreach (MiniForecast MF in Miniforecasts)
                    {
                        if (MF.CityName == miniForecast.CityName)
                            foundDuplicate = 1;
                    }
                    if (foundDuplicate == 0)
                    {
                        this.Miniforecasts.Add(miniForecast);
                        MiniForecastList.SelectedIndex = MiniForecastList.Items.Count - 1;
                        MiniForecastList.Focus();
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
            if (this.searchTextBox.Text != "" && this.searchTextBox.Text.Length >= 5)
            {
                try
                {
                    int foundDuplicate = 0;
                    MiniForecast miniForecast = await setupMiniForecast((City)this.searchTextBox.SelectedItem);
                    foreach (MiniForecast MF in Miniforecasts)
                    {
                        if (MF.CityName == miniForecast.CityName)
                            foundDuplicate = 1;
                    }
                    if (foundDuplicate == 0)
                    {
                        this.Miniforecasts.Add(miniForecast);
                        MiniForecastList.SelectedIndex = MiniForecastList.Items.Count - 1;
                        MiniForecastList.Focus();
                    }
                }
                catch (Exception t)
                {

                }

            }
        }
    }
    // Desc: MiniForecast displayed in Listbox and holds data from Weather API displayed on CityPage
    public class MiniForecast
    {
        public string CityName { get; set; }
        public string Background { get; set; }
        public string WUlogo { get; set; }
        public string BINGlogo { get; set; }
        public string MICROlogo { get; set; }
        public string UMBClogo { get; set; }
        public string currentTemp { get; set; }
        public string dF { get; set; }
        public string conditions { get; set; }
        public string stats { get; set; }
        public string temp { get; set; }
        public string mean { get; set; }
        public string date { get; set; }
        public string feelsLike { get; set; }
        public string rainPercent { get; set; }
        public int hour { get; set; }
        public string currentTime { get; set; }

        public string date1 { get; set; }
        public string icon1 { get; set; }
        public string temperature1 { get; set; }
        public string conditions1 { get; set; }
        public string stats1 { get; set; }
        public string mean1 { get; set; }

        public string date2 { get; set; }
        public string icon2 { get; set; }
        public string temperature2 { get; set; }
        public string conditions2 { get; set; }
        public string stats2 { get; set; }
        public string mean2 { get; set; }

        public string date3 { get; set; }
        public string icon3 { get; set; }
        public string temperature3 { get; set; }
        public string conditions3 { get; set; }
        public string stats3 { get; set; }
        public string mean3 { get; set; }

        public string date4 { get; set; }
        public string icon4 { get; set; }
        public string temperature4 { get; set; }
        public string conditions4 { get; set; }
        public string stats4 { get; set; }
        public string mean4 { get; set; }

        public string date5 { get; set; }
        public string icon5 { get; set; }
        public string temperature5 { get; set; }
        public string conditions5 { get; set; }
        public string stats5 { get; set; }
        public string mean5 { get; set; }
    }
}

