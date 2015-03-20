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

namespace Weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// Trying Git
    public partial class MainWindow : Window
    {
        StartupScreen myStartup;
        public MainWindow()
        {
            InitializeComponent();
            this.myStartup = new StartupScreen();
            mainFrame.Navigate(myStartup);
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
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