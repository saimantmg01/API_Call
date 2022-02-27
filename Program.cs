//libraries used
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;


/*
// http://api.zippopotam.us/us/98121
GETZip Code Lookup -> looks up country and zip code.

Data looks liked this:
{
    "post code": "98121",
    "country": "United States",
    "country abbreviation": "US",
    "places": [
        {
            "place name": "Seattle",
            "longitude": "-122.3447",
            "state": "Washington",
            "state abbreviation": "WA",
            "latitude": "47.6151"
        }
    ]
}
*/

namespace Activity_5
{
    //setting up classes to put data into from API
    class Zipcode_info
    {
        //JSON PROPERTY attribute to control how data from API should be deserizalized as a property in a JSON object.
        [JsonProperty("post code")]
        public string Post_code { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("country abbreviation")]
        public string country_abbreviation { get; set; }

        [JsonProperty("places")]
        public List<Places> Places { get; set; }
    }

    public class Places
    {
        [JsonProperty("place name")]
        public string Place_name { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("state abbreviation")]
        public string State_abbreviation { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }
    }

    class Program
    {
        //creating a instance
        //HTTPclient allows us to send HTTP request and recieve HTTP responses from a resource idenitifed by a given URI.
        private static readonly HttpClient client = new HttpClient();

        //async keyword indicates to C# that method is asynchronous (it runs independently of main program flow and may use an arbitrary
        //number of await expresssions and will bind the result to a promise (Task))

        //TASK is object that represent some work that should be done. It can tell you if work is completed.
        //task class Represent single operation that does not
        //return a value and that usually execute asynchronously.
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //async method called 
            await ProcessRequestsandGiveResponse();
        }
        private static async Task ProcessRequestsandGiveResponse()
        {
            //continuously ask user for zipcode number they want data on, until they'd exit the program
            while (true)
            {
                //try to run program and if anything fails, catch returns the specified error if anything happens
                try
                {
                    Console.WriteLine("Enter Zipcode number. Press Enter without writing a name to quit the program.");
                    var zipcode_number = Console.ReadLine();
                    //in this case, if user hits the Enter key without entering anything, loop will break.
                    if (String.IsNullOrEmpty(zipcode_number))
                    {
                        break;
                    }
                    //Make API call with user input usinG HTTPClient GetAsync method
                    var result = await client.GetAsync("http://api.zippopotam.us/us/" + zipcode_number.ToLower());
                    //Console.WriteLine($"Result: {result}");

                    //use the content child's ReadAs StringAsync() method to serializes the Http content to a string as asynchronous operation.
                    //Serialization is process of converting an object into stream of bytes to store the object or transmit into a memory, a database
                    //or file. Main purpose is to save state of an object in order to be able to recreate it when needed.
                    var resultRead = await result.Content.ReadAsStringAsync();
                    //Console.WriteLine($"Result Read: {resultRead}");

                    //reversing the process of serialization is deseralization.


                    // deserialize with help of attributes we added to our classes. new variable created in this line becomes an instance of
                    //zipcode_info class we created.
                    var zipcode = JsonConvert.DeserializeObject<Zipcode_info>(resultRead);
                    
                    Console.WriteLine("-----");
                    Console.WriteLine("Post code #: " + zipcode.Post_code);
                    Console.WriteLine("Country: " + zipcode.country);
                    Console.WriteLine("Country Abbreviation: " + zipcode.country_abbreviation);
                    Console.WriteLine("Places: ");
                    zipcode.Places.ForEach(t => Console.WriteLine($" Place name: {t.Place_name} \n Longitude: {t.Longitude} \n Latitude: {t.Latitude}\n State: {t.State} \n State Abbreviation: {t.State_abbreviation}"));               
                    Console.WriteLine("\n--");

                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR! Please write a proper zipcode number!");
                }
            }
        }
    }
}
