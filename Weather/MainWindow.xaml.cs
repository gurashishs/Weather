using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

// Desc: Class and methods for Window to setup Framework for Pages in project
// Interactive Logic for MainWindow.xaml
// by: Brian Stevens
namespace Weather
{
    // Main Window for Framework of Pages StartupScreen and CityPage
    public partial class MainWindow : Window
    {
        StartupScreen myStartup;
        int startSearchLock = 0;

        // Desc: Sets up Framework for Pages in project, checks to see if there are saved cities to skip startup page
        // Input: none
        // Dependancy: SavedCities.txt - text document to hold cities to be saved and reinitialized on startup
        // Output: none
        public MainWindow()
        {
            
            InitializeComponent();
            if (File.Exists("./SavedCities.txt"))
            {
                List<string> savedCities = (File.ReadLines("./SavedCities.txt")).ToList();
                if (savedCities.Count == 0)
                {
                    this.myStartup = new StartupScreen();
                    mainFrame.Navigate(myStartup);
                    startSearchLock = 1;
                }
                else
                {
                    this.myStartup = new StartupScreen(savedCities);
                    mainFrame.Navigate(myStartup.getPage());
                    startSearchLock = 1;
                }
            }
            else {
             
                this.myStartup = new StartupScreen();
                mainFrame.Navigate(myStartup);
                startSearchLock = 1;
            }
            
        }
        // Desc: Sets up Framework for Pages in project, checks to see if there are saved cities to skip startup page
        // Input: Sender and event when Window closes
        // Output: Saves cities to SavedCities.txt that are in forecast list
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (startSearchLock == 1) {
                string createText = "";
                List<string> forecastsToSave = this.myStartup.getMiniForecastList();
                foreach (var MF in forecastsToSave)
                {
                    createText += MF + Environment.NewLine; ;
                }
                File.WriteAllText("./SavedCities.txt", createText);
            }
        }
    }
}