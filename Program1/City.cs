using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class City
    {
        [Index(0)]
        public string Name { get; set; }

        [Index(1)]
        public double XCoordinate { get; set; }

        [Index(2)]
        public double YCoordinate { get; set; }

        [Ignore]
        public List<City> AdjacentCities { get; set; }

        public City()
        {
            AdjacentCities = new List<City>();
        }
    }

