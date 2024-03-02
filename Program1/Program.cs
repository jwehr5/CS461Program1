using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;


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
    if (city.Name == "Hillsboro")
    {
        foreach (City adjacentCity in city.AdjacentCities)
        {
            Console.WriteLine(adjacentCity.Name);

        }
    }
}



Console.WriteLine(SearchMethods.CalculateDistance(38.3494571, -97.2156415, 37.8098997, -96.8943313));






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



Console.WriteLine();

//Depth First Search
//List<string> dfsTest = SearchMethods.DepthFirstSearch(citys, startingCity, destinationCity);

/*
foreach(string city in dfsTest)
{
    Console.WriteLine(city);
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
List<string> IDDFS = SearchMethods.IterativeDeepeningDFS(citys, startingCity, destinationCity, 9);

if(IDDFS.Count == 0)
{
    Console.WriteLine("Could not find target within the specified depth");
}


//Best First Search
//List<string> bestFirstSearch = SearchMethods.BestFirstSearch(citys, startingCity, destinationCity);

/*
foreach(String s in bestFirstSearch)
{
    Console.WriteLine(s);
}
*/


//Console.WriteLine(SearchMethods.CalculateDistance(38.9220277, -97.2666667, 39.0379342, -96.8799338));
//Console.WriteLine(SearchMethods.CalculateDistance(38.9220277, -97.2666667, 38.8254325, -97.702327));
//Console.WriteLine(SearchMethods.CalculateDistance(38.9220277, -97.2666667, 38.3589767, -97.0267385));
//Console.WriteLine(SearchMethods.CalculateDistance(38.9220277, -97.2666667, 38.88509, -99.326202));









