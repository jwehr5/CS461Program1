﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geolocation;

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



    public static bool IterativeDeepeningDFS(City startingCity, City endCity, int steps)
    {
        List<string> travelPath = new List<string>();
        for (int i = 0; i < steps; i++)
        {
            travelPath.Clear();
            if(DLS(startingCity, endCity, i, ref travelPath) == true)
            {
                Console.WriteLine(i);
                foreach(string s in travelPath)
                {
                    Console.WriteLine(s);
                }
                return true;
            }

            Console.WriteLine();
            Console.WriteLine(i);
            foreach (string s in travelPath)
            {
                Console.WriteLine(s);
            }

        }

        return false;  
    }

    public static bool DLS(City origin, City endCity, int limit, ref List<string> travelPath)
    {
        
        if (origin.Name == endCity.Name)
        {
            return true; 
        }

        if(limit <= 0)
        {
            //travelPath.Clear();
            return false;
        }

        foreach(City adjacentCity in origin.AdjacentCities)
        {
            travelPath.Add(adjacentCity.Name);
            if(DLS(adjacentCity, endCity, limit - 1, ref travelPath) == true)
            {
                return true;
            }
        }

        return false;
    }


    public static double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        Coordinate origin = new Coordinate(x1, y1);
        Coordinate destination = new Coordinate(x2, y2);

        return GeoCalculator.GetDistance(origin, destination);
    }

    public static List<string> BestFirstSearch(List<City> cityList, string startingCity, string endCity)
    {
        List<string> travelPath = new List<string>();
        PriorityQueue<City, double> recentlyDiscoveredCities = new PriorityQueue<City, double>();


        travelPath.Add(startingCity);

        foreach(City city in cityList)
        {
            if(city.Name == startingCity)
            {
                foreach(City adjacentCity in city.AdjacentCities)
                {
                    recentlyDiscoveredCities.Enqueue(adjacentCity, CalculateDistance(city.XCoordinate, city.YCoordinate, adjacentCity.XCoordinate, adjacentCity.YCoordinate));
                
                }
            }
        }

      
        //Console.WriteLine(recentlyDiscoveredCities.Dequeue().Name);
        // Console.WriteLine(recentlyDiscoveredCities.Dequeue().Name);
        //Console.WriteLine(recentlyDiscoveredCities.Dequeue().Name);
        //Console.WriteLine(recentlyDiscoveredCities.Dequeue().Name);

        City currentCity = recentlyDiscoveredCities.Dequeue();

        while(currentCity.Name != endCity)
        {
            if (!travelPath.Contains(currentCity.Name))
            {
                travelPath.Add(currentCity.Name);
            }

            foreach (City adjacentCity in currentCity.AdjacentCities)
            {
                if (!travelPath.Contains(adjacentCity.Name))
                {
                    //Our Destination city is a neighbor of our current city, so go ahead and stop the search.
                    if(adjacentCity.Name == endCity)
                    {
                        travelPath.Add(adjacentCity.Name);
                        return travelPath;
                    }

                    recentlyDiscoveredCities.Enqueue(adjacentCity, CalculateDistance(currentCity.XCoordinate, currentCity.YCoordinate, adjacentCity.XCoordinate, adjacentCity.YCoordinate));
                    
                    


                }

            }
            

            currentCity = recentlyDiscoveredCities.Dequeue();
            
            

        }


        travelPath.Add(currentCity.Name);
        return travelPath;
    }

}
