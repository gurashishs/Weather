using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Net;
using System.Xml;
using Newtonsoft.Json;

namespace Weather
{
    // WeatherUnderground API to interface with wunderground.com API
    public class WeatherUndergroundAPI
    {
        private const string API_KEY = "219620ed851cd61a";
        private const string GEO_LOOKUP_STR = "http://api.wunderground.com/api/" + API_KEY + "/geolookup/q/autoip.json";
        private const string FORECAST_STR = "http://api.wunderground.com/api/" + API_KEY + "/forecast/q/";

        private const string QUERY_STR = "http://autocomplete.wunderground.com/aq?c=US&h=0&format=JSON&query=";
        
        private HttpClient httpClient;

        public WeatherUndergroundAPI()
        {
            // Create new HTTP client
            this.httpClient = new HttpClient();
        }

        // Desc: Get forecast for the given city
        // Input: City to get forecast for
        // Output: Forecast for city. Returns async task. Needs to "await"ed
        public async Task<Forecast> getForecastForCity(City city)
        {
            ForecastResults forecastResults = await GetJsonObject<ForecastResults>(FORECAST_STR);
            return forecastResults.forecast;
        }

        // Desc: Get City using the IP address of user using GeoLookup feature
        // Output: City closest to user's IP address. Returns async task. Needs to "await"ed
        public async Task<City> getCityByGeoLookup()
        {
            GeoLookup geoLookup = await GetJsonObject<GeoLookup>(GEO_LOOKUP_STR);
            return LocationToCity(geoLookup.location);
        }

        // Desc: Find the closest city name matches to given query using AutoCompler feature
        // Input: String to query
        // Output: List of cities with names closest to query. Returns async task. Needs to "await"ed
        public async Task<List<City>> getCitiesByNameQuery(string query)
        {
            CityResults cityResults = await this.GetJsonObject<CityResults>(QUERY_STR + query);
            return cityResults.RESULTS;
        }

        // Desc: Convert Location object to City object.
        // Input: Location returned by GeoLookup
        // Output: City object that is a translation of input
        private City LocationToCity(Location location)
        {
            return new City()
            {
                name = location.city + ", " + location.state,
                type = location.type,
                c    = location.country,
                zmw  = location.zip + "." + location.magic + "." + location.wmo,
                tz   = location.tz_long,
                tzs  = location.tz_short,
                l    = location.l,
                ll   = location.lat + " " + location.lon,
                lat  = location.lat,
                lon  = location.lon
            };
        }

        // Desc: Gets JSON object given HTTP URL request. Very Helpful Resource = http://json2csharp.com/
        // Input: HTTP request URL 
        // Output: Returns the type of object requested. Returns async task. Needs to "await"ed
        private async Task<T> GetJsonObject<T>(string uri)
        {
            string result = await GetAsync(uri);
            return JsonConvert.DeserializeObject<T>(result);
        }

        // Desc: Make asynchronous HTTP request
        // Input: HTTP request URL 
        // Output: Returns result string of HTTP request
        private async Task<string> GetAsync(string uri)
        {            
            return await this.httpClient.GetStringAsync(uri);
        }
    }
}
