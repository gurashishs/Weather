using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Weather;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WeatherUndergroundAPI_TB
{
    [TestClass]
    public class FunctionalTests
    {
        [TestMethod]
        public void API_KeyCheck()
        {
            WeatherUndergroundAPI api = new WeatherUndergroundAPI();
            string expectedKey = "219620ed851cd61a";
            string key = api.getAPIkey();
            Assert.AreEqual(expectedKey, key, true);
        }

        [TestMethod]
        public async Task API_GeoLookupCheck()
        {
            WeatherUndergroundAPI api = new WeatherUndergroundAPI();
            City city = await api.getCityByGeoLookup();
            string cityName = city.name;
            string expectedCityName = "Baltimore, MD";
            Assert.AreEqual(expectedCityName, cityName, true);
        }

        [TestMethod]
        public async Task API_CurrentObservationCheck()
        {
            string cityName = "Baltimore";
            string cityState = "MD";

            WeatherUndergroundAPI api = new WeatherUndergroundAPI();
            List<City> cities = await api.getCitiesByNameQuery(cityName + ", " + cityState);
            City city = cities[0];
            
            CurrentObservation observation = await api.getCurrentObservationForCity(city);
            string forecastURL = observation.forecast_url;
            string expectedForecastURL = "http://www.wunderground.com/US/" + cityState + "/" + cityName + ".html";
            Assert.AreEqual(expectedForecastURL, forecastURL, true);
        }
    }
}
