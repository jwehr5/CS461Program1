using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;


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


foreach (City city in citys)
{
    if (city.Name == "Andover")
    {
        foreach (City adjacentCity in city.AdjacentCities)
        {
            Console.WriteLine(adjacentCity.Name);

        }
    }
}




//Console.WriteLine(SearchMethods.CalculateDistance(38.3704302, -97.6917722, 38.2434672, -96.9378672));

//Console.WriteLine(SearchMethods.CalculateDistance(38.3704302, -97.6917722, 38.3494571, -97.2156415));

//Console.WriteLine(SearchMethods.CalculateDistance(38.3704302,-97.6917722, 38.0353742, -97.4239353));

//Console.WriteLine(SearchMethods.CalculateDistance(38.3704302,-97.6917722, 38.8254325, -97.702327));

//Console.WriteLine(SearchMethods.CalculateDistance(38.3704302,-97.6917722, 38.0572062, -97.9414547));

//Console.WriteLine(SearchMethods.CalculateDistance(38.8254325, -97.702327, 37.6868403, -97.1657752));

//Console.WriteLine(SearchMethods.CalculateDistance(38.0572062, -97.9414547, 37.6868403, -97.1657752));

Console.WriteLine(SearchMethods.CalculateDistance(38.9220277, -97.2666667, 37.6868403, -97.1657752));
Console.WriteLine(SearchMethods.CalculateEuclideanDistance(38.9220277, -97.2666667, 37.6868403, -97.1657752));



Console.WriteLine("Enter your starting city");
string startingCity = Console.ReadLine();
Console.WriteLine();

Console.WriteLine("Enter your destination city");
string destinationCity = Console.ReadLine();
Console.WriteLine();

if (startingCity.Contains(" "))
{
    startingCity = startingCity.Replace(" ", "_");
}

if (destinationCity.Contains(" "))
{
    destinationCity = destinationCity.Replace(" ", "_");
}

City originCity = null;
City endCity = null;

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



Console.WriteLine();

Stopwatch sw = new Stopwatch();

//Depth First Search
//List<City> dfsTest = SearchMethods.DepthFirstSearch(originCity, endCity);
/*
if(dfsTest.Count != 0)
{
    foreach (City city in dfsTest)
    {
        Console.WriteLine(city.Name);
    }
}
*/



//Breadth First Search
//List<string> bfsTest = SearchMethods.BreadthFirstSearch(citys, startingCity, destinationCity);

/*
foreach(string city in bfsTest)
{
    Console.WriteLine(city);
}
*/

//ID-DFS
//Console.WriteLine(SearchMethods.IterativeDeepeningDFS(originCity, endCity, 5));


//Best First Search
sw.Start();
List<string> bestFirstSearch = SearchMethods.BestFirstSearch(citys, startingCity, destinationCity);
sw.Stop();
TimeSpan ts = sw.Elapsed;
Console.WriteLine(ts.ToString());
sw.Reset();

/*
foreach(String s in bestFirstSearch)
{
    Console.WriteLine(s);
}
*/






//A* Search
sw.Start();
List<City> aStarSearchResult = SearchMethods.AStarSearch(originCity, endCity);
sw.Stop();
 ts = sw.Elapsed;
Console.WriteLine(ts.ToString());

foreach(City c in aStarSearchResult)
{
    Console.WriteLine(c.Name);
}










