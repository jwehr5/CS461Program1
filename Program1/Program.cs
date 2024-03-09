using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Net;


//Initialize each city along with their x and y coordinates
var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    HasHeaderRecord = false,
};

using var sr = new StreamReader(@"./coordinates (1).csv");
using var csv = new CsvReader(sr, config);

var citys = csv.GetRecords<City>().ToList();


//Initialize the adjacent cities of each city
string[] lines = File.ReadAllLines(@"./Adjacencies.txt");

foreach (City city1 in citys)
{   
    foreach(string line in lines)
    {
        string leftHandCity = line.Substring(0, line.IndexOf(" "));
        string rightHandCity = line.Substring(line.IndexOf(" ") + 1).TrimEnd(' ');
        
        foreach(City city2 in citys)
        {
            if(city1.Name == leftHandCity && city2.Name == rightHandCity){

                if (!city1.AdjacentCities.Contains(city2))
                {
                    city1.AdjacentCities.Add(city2);
                }
                

            }else if(city1.Name == rightHandCity && city2.Name == leftHandCity)
            {
                if (!city1.AdjacentCities.Contains(city2))
                {
                    city1.AdjacentCities.Add(city2);
                }
                
            }
        }

        /*
        if((city1 == city.Name) && (!city.AdjacentCities.Contains(city2)))
        {
            
            city.AdjacentCities.Add(city2);

        }
        
        if((city2 == city.Name) && (!city.AdjacentCities.Contains(city1)))
        {
            
            city.AdjacentCities.Add(city1);
        }
        */

    }

}

//Print out the adjacent cities of a city, used for testing

/*
foreach (City city in citys)
{
    if (city.Name == "McPherson")
    {
        foreach (City adjacentCity in city.AdjacentCities)
        {
            Console.WriteLine(adjacentCity.Name);

        }
    }
}
*/




bool programDone = false;
string startingCity = "";
string destinationCity = "";

City originCity = null;
City endCity = null;

//Variables for timing to execution of the search methods
Stopwatch sw = new Stopwatch();
double ts;

while (!programDone)
{
    //Ask for the starting city
    bool startingCityIsValid = false;
    while (!startingCityIsValid)
    {
        Console.WriteLine("Enter your starting city");
         startingCity = Console.ReadLine();

        //If the city has a space, replace it with an underscore
        if (startingCity.Contains(" "))
        {
            startingCity = startingCity.Replace(" ", "_");
        }

        startingCityIsValid = SearchMethods.IsCityValid(citys, startingCity);
        if(startingCityIsValid == false)
        {
            Console.WriteLine("City not found in the database");
        }
    }

    

    Console.WriteLine();

    //Ask for the ending city
    bool endCityIsValid = false;
    while (!endCityIsValid)
    {
        Console.WriteLine("Enter your destination city");
         destinationCity = Console.ReadLine();

        //If the city has a space, replace it with an underscore
        if (destinationCity.Contains(" "))
        {
            destinationCity = destinationCity.Replace(" ", "_");
        }

        endCityIsValid = SearchMethods.IsCityValid(citys, destinationCity);
        if (endCityIsValid == false)
        {
            Console.WriteLine("City not found in the database");
        }
    }

    

    Console.WriteLine();


    //Initialize the starting and ending cities
    foreach (City c in citys)
    {
        if (c.Name == startingCity)
        {
            originCity = c;
        }

        if (c.Name == destinationCity)
        {
            endCity = c;
        }
    }

    //Display the options and get the user's input
    Console.WriteLine("Which search would you like to perform? Enter the corresponding key. \n" + "1 - Depth First Search \n" + "2 - Breadth First Search \n"
                      + "3 - Iterative Deepening - DFS \n" + "4 - Best First Search \n" + "5 - A* Search \n");
    int userChoice = Int32.Parse(Console.ReadLine());

    switch (userChoice)
    {
        //Depth First Search
        case 1:
            {
                Console.WriteLine("Performing Depth First Search...");

                sw.Start();
                List<City> dfs = SearchMethods.DepthFirstSearch(originCity, endCity);
                sw.Stop();
                
                ts = sw.Elapsed.TotalSeconds;
                Console.WriteLine("Total time of search is " + ts + " seconds");
                sw.Reset();

                if (dfs.Count != 0)
                {
                    Console.WriteLine("The route between the two cities is: ");
                    DisplayRouteAndTotalDistance(dfs);
                    Console.WriteLine();
                }

                break;
            }

        //Breadth First Search
        case 2:
            {
                Console.WriteLine("Performing Breadth First Search...");

                sw.Start();
                List<City> bfs = SearchMethods.BreadthFirstSearch(originCity, endCity);
                sw.Stop();

                ts = sw.Elapsed.TotalSeconds;
                Console.WriteLine("Total time of search is " + ts + " seconds");
                sw.Reset();

                if (bfs.Count != 0)
                {
                    Console.WriteLine("The route between the two cities is: ");
                    DisplayRouteAndTotalDistance(bfs);
                    Console.WriteLine();
                }

                break;

            }
        //ID-DFS
        case 3:
            {
                Console.WriteLine("Performing ID-DFS... ");
                Console.WriteLine("How many steps deep would you like to search?");
                int steps = Int32.Parse(Console.ReadLine());

                sw.Start();
                List<City> iddfs = new List<City>();
                bool iddfsSuccess = SearchMethods.IterativeDeepeningDFS(originCity, endCity, steps, ref iddfs);
                sw.Stop();

                ts = sw.Elapsed.TotalSeconds;
                Console.WriteLine("Total time of search is " + ts + " seconds");
                sw.Reset();

                if (iddfsSuccess)
                {
                    Console.WriteLine("The route between the two cities is: ");
                    DisplayRouteAndTotalDistance(iddfs);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Could not find a route looking " + steps + " steps deep.");
                }

                break;
            }
        //Best first Search
        case 4:
            {
                Console.WriteLine("Performing Best First Search...");

                sw.Start();
                List<City> bestFirstSearch = SearchMethods.BestFirstSearch(originCity, endCity);
                sw.Stop();

                ts = sw.Elapsed.TotalSeconds;
                Console.WriteLine("Total time of search is " + ts + " seconds");
                sw.Reset();

                if(bestFirstSearch.Count != 0)
                {
                    Console.WriteLine("The route between the two cities is: ");
                    DisplayRouteAndTotalDistance(bestFirstSearch);
                    Console.WriteLine();
                }

                break;
            }
        // A* Search
        case 5:
            {
                Console.WriteLine("Performing A* Search...");

                sw.Start();
                List<City> aStarSearchResult = SearchMethods.AStarSearch(originCity, endCity);
                sw.Stop();

                ts = sw.Elapsed.TotalSeconds;
                Console.WriteLine("Total time of search is " + ts + " seconds");
                sw.Reset();

                if(aStarSearchResult.Count != 0)
                {
                    Console.WriteLine("The route between the two cities is: ");
                    DisplayRouteAndTotalDistance(aStarSearchResult);
                    Console.WriteLine();
                }

                break;
            }
        default:
            {
                Console.WriteLine("Invalid Option");
                break;
            }
    }



    Console.WriteLine("Would you like to perform another search? " + "Type Y for yes. Type N for no");
    string yesOrNo = Console.ReadLine().ToLower();

    if(yesOrNo == "n")
    {
        programDone = true;
    }



}

 static void DisplayRouteAndTotalDistance(List<City> route)
{
    double totalDistance = 0;
    Console.WriteLine(route[0].Name);
    for (int i = 0, j = 1; i < route.Count - 1; i++, j++)
    {
        Console.WriteLine(route[j].Name);

        totalDistance += SearchMethods.CalculateDistance(route[i].Longitude, route[i].Latitude, route[j].Longitude, route[j].Latitude);
    }

    Console.WriteLine("The total distance is: " + totalDistance.ToString("0.00") + " miles");

}


