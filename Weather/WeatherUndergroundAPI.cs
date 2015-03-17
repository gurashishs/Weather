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

namespace Autocomplete
{
    class WeatherUndergroundAPI
    {
        private string GEO_LOOKUP_STR = "http://api.wunderground.com/api/219620ed851cd61a/geolookup/q/autoip.json";
        private string QUERY_STR = "http://autocomplete.wunderground.com/aq?c=US&h=0&format=JSON&query=";
        private HttpClient httpClient;

        public WeatherUndergroundAPI()
        {
             this.httpClient = new HttpClient();
        }

        public async Task<City> getCityByGeoLookup()
        {
            GeoLookup geoLookup = await GetJsonObject<GeoLookup>(GEO_LOOKUP_STR);
            return LocationToCity(geoLookup.location);
        }

        public async Task<List<City>> getCitiesByNameQuery(string query)
        {
            CityResults cityResults = await this.GetJsonObject<CityResults>(QUERY_STR + query);
            return cityResults.RESULTS;
        }

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

        // Very Helpful Resource = http://json2csharp.com/
        private async Task<T> GetJsonObject<T>(string uri)
        {
            string result = await GetAsync(uri);
            return JsonConvert.DeserializeObject<T>(result);
        }

        private async Task<string> GetAsync(string uri)
        {            
            return await this.httpClient.GetStringAsync(uri);
        }
    }
}
