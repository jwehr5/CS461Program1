
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geolocation;

public static class SearchMethods
{
    /**
     * Method for checking that the cities that the user entered is valid.
     */
    public static bool IsCityValid(List<City> cityList, string cityName)
    {
        bool cityFound = false;

        foreach (City c in cityList)
        {
            if (c.Name == cityName)
            {
                cityFound = true;
            }
        }

        return cityFound;
    }

    /**
     *  Depth First Search between two cities 
     */
    public static List<City> DepthFirstSearch(City startingCity, City endCity)
    {
        //Keep track of the cities we visit in this list.
        List<City> travelPath = new List<City>();

        //For every city we discover, we'll push it onto this stack.
        Stack<City> recentlyDiscoveredCities = new Stack<City>();

        travelPath.Add(startingCity);

        //Push all the cities adjacent to our staring city onto the stack. Start from the end of the list so that we'll be starting our search on the left side of the tree.
        for(int i = startingCity.AdjacentCities.Count - 1; i >= 0; i--)
        {
            recentlyDiscoveredCities.Push(startingCity.AdjacentCities[i]);
            
        }

        City currentCity = recentlyDiscoveredCities.Pop();

        //While we haven't found our destination, conduct DFS.
        while (currentCity.Name != endCity.Name)
        {
            
            //If our travel path doesn't contain the city we are currently at, add it to the travel path
            if (!travelPath.Contains(currentCity))
            {
                travelPath.Add(currentCity);
            }

            //Push the adjacent cities in reverse so that we'll be searching left to right.
            for (int i = currentCity.AdjacentCities.Count - 1; i >= 0; i--)
            {
                //To avoid ourselves from getting into a cycle don't add cities we have already visited.
                if (!travelPath.Contains(currentCity.AdjacentCities[i]))
                {
                    recentlyDiscoveredCities.Push(currentCity.AdjacentCities[i]);
                }
                

            }

            //If the stack is empty, then that means we weren't able to find a path
            if (recentlyDiscoveredCities.Count == 0)
            {
                travelPath.Clear();
                return travelPath;
            }

            currentCity = recentlyDiscoveredCities.Pop();

        }

        travelPath.Add(currentCity);

        return travelPath;

    }


    /*
     * Breadth First Search between the two cities
     */
    public static List<City> BreadthFirstSearch(City startingCity, City endCity)
    {
        //Keep track of the cities we visit in this list.
        List<City> travelPath = new List<City>();

        //For every city we discover, we'll push it enqueue it onto this queue.
        Queue<City> recentlyDiscoveredCities = new Queue<City>();

        travelPath.Add(startingCity);

        foreach (City adjacentCity in startingCity.AdjacentCities)
        {
            recentlyDiscoveredCities.Enqueue(adjacentCity);
        }
            
       
        City currentCity = recentlyDiscoveredCities.Dequeue();

        //While we haven't found our destination, conduct BFS
        while (currentCity.Name != endCity.Name)
        {

            //If our travel path doesn't contain the city we are currently at, add it to the travel path
            if (!travelPath.Contains(currentCity))
            {
                travelPath.Add(currentCity);
            }

            foreach (City adjacentCity in currentCity.AdjacentCities)
            {
                //To avoid ourselves from getting into a cycle don't add cities we have already visited.
                if (!travelPath.Contains(adjacentCity))
                {
                    recentlyDiscoveredCities.Enqueue(adjacentCity);
                }

            }

            //If the queue is empty, then that means we weren't able to find a path
            if (recentlyDiscoveredCities.Count == 0)
            {
                travelPath.Clear();
                return travelPath;
            }

            currentCity = recentlyDiscoveredCities.Dequeue();

        }

        travelPath.Add(currentCity);

        return travelPath;
    }

    /**
     * ID_DFS between the two ciites 
    */
    public static bool IterativeDeepeningDFS(City startingCity, City endCity, int steps, ref List<City> travelPath)
    {
        //The search is conducted one step at a time.
        for (int i = 0; i < steps; i++)
        {
            travelPath.Clear();
            travelPath.Add(startingCity);

            if (SearchSubtree(startingCity, endCity, i, ref travelPath) == true)
            {
                
                return true;
            }

        }

        return false;
    }

    private static bool SearchSubtree(City origin, City endCity, int limit, ref List<City> travelPath)
    {

        if (origin.Name == endCity.Name)
        {
            return true;
        }

        if (limit <= 0)
        {
            //travelPath.Clear();
            return false;
        }

        foreach (City adjacentCity in origin.AdjacentCities)
        {
            //To avoid ourselves from getting into a cycle don't add cities we have already visited.
            if (!travelPath.Contains(adjacentCity))
            {
                travelPath.Add(adjacentCity);

                //For each adjacent city search through all its children up to the specified depth
                if (SearchSubtree(adjacentCity, endCity, limit - 1, ref travelPath) == true)
                {
                    return true;
                }

            }

        }

        return false;
    }

    /*
     * Calculate the distance between two cities
     */
    public static double CalculateDistance(double lon1, double lat1, double lon2, double lat2)
    {
        Coordinate origin = new Coordinate(lon1, lat1);
        Coordinate destination = new Coordinate(lon2, lat2);

        return GeoCalculator.GetDistance(origin, destination);
    }

    /**
     *  Best First Search between the two cities
     */
    public static List<City> BestFirstSearch(City startingCity, City endCity)
    {
        List<City> travelPath = new List<City>();
        PriorityQueue<City, double> recentlyDiscoveredCities = new PriorityQueue<City, double>();


        travelPath.Add(startingCity);

           
        foreach (City adjacentCity in startingCity.AdjacentCities)
        {
            //The priority value for each city will be its distance from our starting city to the adjacent city.
            recentlyDiscoveredCities.Enqueue(adjacentCity, CalculateDistance(startingCity.Longitude, startingCity.Latitude, adjacentCity.Longitude, adjacentCity.Latitude));

        }
            
       

        City currentCity = recentlyDiscoveredCities.Dequeue();

        while (currentCity.Name != endCity.Name)
        {
            if (!travelPath.Contains(currentCity))
            {
                travelPath.Add(currentCity);
            }

            foreach (City adjacentCity in currentCity.AdjacentCities)
            {
                //To avoid ourselves from getting into a cycle don't add cities we have already visited.
                if (!travelPath.Contains(adjacentCity))
                {
                    //Our Destination city is a neighbor of our current city, so go ahead and stop the search.
                    if (adjacentCity.Name == endCity.Name)
                    {
                        travelPath.Add(adjacentCity);
                        return travelPath;
                    }

                    recentlyDiscoveredCities.Enqueue(adjacentCity, CalculateDistance(currentCity.Longitude, currentCity.Latitude, adjacentCity.Longitude, adjacentCity.Latitude));


                }

            }

            //If the queue is empty, then that means we weren't able to find a path
            if (recentlyDiscoveredCities.Count == 0)
            {
                travelPath.Clear();
                return travelPath;
            }

            currentCity = recentlyDiscoveredCities.Dequeue();


        }


        travelPath.Add(currentCity);
        return travelPath;
    }


    //Calculates the Euclidean distance between two longitude and latitude coordinates
    public static double CalculateEuclideanDistance(double currentLon, double currentLat, double goalLon, double goalLat)
    {
        // Earth radius in kilometers
        const double R = 6371;


        double currentLonRad = ToRadians(currentLon);
        double currentLatRad = ToRadians(currentLat);
        double goalLonRad = ToRadians(goalLon);
        double goalLatRad = ToRadians(goalLat);
        
        double currentX = R * Math.Cos(currentLonRad) * Math.Cos(currentLatRad);
        double currentY = R * Math.Cos(currentLonRad) * Math.Sin(currentLatRad);
        double goalX = R * Math.Cos(goalLonRad) * Math.Cos(goalLatRad);
        double goalY = R * Math.Cos(goalLonRad) * Math.Sin(goalLatRad);



        double distance = Math.Sqrt(Math.Pow((goalX - currentX), 2) + Math.Pow((goalY - currentY), 2));
        distance = Math.Round(distance, 1);


        return distance;
    }

    // Convert degrees to radians
    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
        
    }

    /*
     * A* search between the two ciites
     */
    public static List<City> AStarSearch(City startingCity, City endCity)
    {
        List<City> travelPath = new List<City>();
        travelPath.Add(startingCity);

        PriorityQueue<City, double> recentlyDiscoveredCities = new PriorityQueue<City, double>();

        foreach(City adjacentCity in startingCity.AdjacentCities)
        {
            /*
             * The priority value for each destination city will be the distance from the starting city to the adjacent city, 
             * plus the distance from the adjacent city to the destination city.
             */
            recentlyDiscoveredCities.Enqueue(adjacentCity, CalculateDistance(startingCity.Longitude, startingCity.Latitude, adjacentCity.Longitude, adjacentCity.Latitude)
                                                          + CalculateEuclideanDistance(adjacentCity.Longitude, adjacentCity.Latitude, endCity.Longitude, endCity.Latitude));
        }

        City currentCity = recentlyDiscoveredCities.Dequeue();


        while (currentCity.Name != endCity.Name)
        {
            if (!travelPath.Contains(currentCity))
            {
                travelPath.Add(currentCity);
            }

            foreach (City adjacentCity in currentCity.AdjacentCities)
            {
                //To avoid ourselves from getting into a cycle don't add cities we have already visited.
                if (!travelPath.Contains(adjacentCity))
                {
                    //Our Destination city is a neighbor of our current city, so go ahead and stop the search.
                    if (adjacentCity.Name == endCity.Name)
                    {
                        travelPath.Add(adjacentCity);
                        return travelPath;
                    }

                    recentlyDiscoveredCities.Enqueue(adjacentCity, CalculateDistance(currentCity.Longitude, currentCity.Latitude, currentCity.Longitude, currentCity.Latitude)
                                                          + CalculateEuclideanDistance(adjacentCity.Longitude, adjacentCity.Latitude, endCity.Longitude, endCity.Latitude));


                }

            }

            //If the queue is empty, then that means we weren't able to find a path
            if (recentlyDiscoveredCities.Count == 0)
            {
                travelPath.Clear();
                return travelPath;
            }

            currentCity = recentlyDiscoveredCities.Dequeue();


        }


        travelPath.Add(currentCity);
        return travelPath;
    }

}
