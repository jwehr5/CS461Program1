using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class SearchMethods
{

    public static List<string> DepthFirstSearch(List<City> cityList, string startingCity, string endCity)
    {
        //Keep track of the cities we visit in this list.
        List<string> travelPath = new List<string>();

        //For every city we discover, we'll push it onto this stack.
        Stack<City> recentlyDiscoveredCities = new Stack<City>();

        travelPath.Add(startingCity);

        foreach(City city in cityList)
        {
            if(city.Name == startingCity)
            {
                foreach(City adjacentCity in city.AdjacentCities)
                {
                    recentlyDiscoveredCities.Push(adjacentCity);
                }
            }
        }

        City currentCity = recentlyDiscoveredCities.Pop();
        
        
        while(currentCity.Name != endCity)
        {
            //Console.WriteLine(currentCity.Name);

            if (!travelPath.Contains(currentCity.Name))
            {
                travelPath.Add(currentCity.Name);
            }

            foreach(City adjacentCity in currentCity.AdjacentCities)
            {
                if(!travelPath.Contains(adjacentCity.Name))
                {
                    recentlyDiscoveredCities.Push(adjacentCity);
                }
                
            }

            currentCity = recentlyDiscoveredCities.Pop();  
            
        }

        travelPath.Add(currentCity.Name);

        return travelPath;

    }



    public static List<string> BreadthFirstSearch(List<City> cityList, string startingCity, string endCity)
    {
        List<string> travelPath = new List<string>();
        Queue<City> recentlyDiscoveredCities = new Queue<City>();

        travelPath.Add(startingCity);

        foreach (City city in cityList)
        {
            if (city.Name == startingCity)
            {
                foreach (City adjacentCity in city.AdjacentCities)
                {
                    recentlyDiscoveredCities.Enqueue(adjacentCity);
                }
            }
        }

        City currentCity = recentlyDiscoveredCities.Dequeue();


        while (currentCity.Name != endCity)
        {
            //Console.WriteLine(currentCity.Name);

            if (!travelPath.Contains(currentCity.Name))
            {
                travelPath.Add(currentCity.Name);
            }

            foreach (City adjacentCity in currentCity.AdjacentCities)
            {
                if (!travelPath.Contains(adjacentCity.Name))
                {
                    recentlyDiscoveredCities.Enqueue(adjacentCity);
                }

            }

            currentCity = recentlyDiscoveredCities.Dequeue();

        }

        travelPath.Add(currentCity.Name);




        return travelPath;
    }

}
