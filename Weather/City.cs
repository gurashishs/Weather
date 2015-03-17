using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    public class City : IComparable
    {
        public string name { get; set; }
        public string type { get; set; }
        public string c { get; set; }
        public string zmw { get; set; }
        public string tz { get; set; }
        public string tzs { get; set; }
        public string l { get; set; }
        public string ll { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is City)
            {
                return this.name.CompareTo((obj as City).name);
            }
            throw new ArgumentException("Object is not a City");
        }
    }

    public class CityResults
    {
        public List<City> RESULTS { get; set; }
    }   
}