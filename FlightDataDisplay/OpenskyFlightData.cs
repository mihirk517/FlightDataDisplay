using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using FlightDataDisplay.Domain;
using Newtonsoft.Json;
using Ibistic.Public.OpenAirportData;
using Ibistic.Public.OpenAirportData.OpenFlightsData;
using Ibistic.Public.OpenAirportData.MemoryDatabase;
namespace FlightDataDisplay.Infrastructure
{
    class OpenskyFlightData : IFlightDataRepository
    {
        public static HttpClient client = new HttpClient();
        public string icoa = "EDDF";

        string clientId = "mihirk517-api-client";
        string clientSecret = "CAZL39X78pd7oN9WGFw0ek4keXQkK1tz";
        string token = string.Empty;
        List<FlightArrival> Flights = new List<FlightArrival>();
        OpenFlightsDataAirportProvider airportProvider;
        IEnumerable<Airport> airports;
        AirportIataCodeDatabase airportCodes;



        System.Timers.Timer OpenSkytimer = new System.Timers.Timer() { Interval = TimeSpan.FromMinutes(30).TotalMilliseconds };
        public OpenskyFlightData()
        {
            OpenSkytimer.Enabled = true;
            OpenSkytimer.Elapsed += GetOpenSkyData;
            GetOpenSkyData(null, null);
            OpenSkytimer.Start();

            airportProvider = new OpenFlightsDataAirportProvider("airports.cache");
            airports = airportProvider.GetAllAirports();
            airportCodes = new AirportIataCodeDatabase();
	        airportCodes.AddOrUpdateAirports(airportProvider.GetAllAirports(), true, true);
        }

        private async void GetOpenSkyData(object sender, ElapsedEventArgs e)
        {
            token = await GetOpenSkyToken(clientId, clientSecret);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // OpenSky requires Unix timestamps (seconds)
            long begin = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds();
            long end = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string url = $"https://opensky-network.org/api/flights/arrival?airport={icoa}&begin={begin}&end={end}";

            HttpResponseMessage response;
            string content = string.Empty;
            try
            {
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode) content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(response.StatusCode + response.Content.ToString());

            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error in getting data from Opensky {ex.Message}");
            }
            //Console.WriteLine(content);
            Flights = JsonConvert.DeserializeObject<List<FlightArrival>>(content);
        }

        public async Task<string> GetOpenSkyToken(string clientId, string clientSecret)
        {
            using HttpClient client = new HttpClient();
            try
            {
                var requestData = new FormUrlEncodedContent(new[]
     {
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        new KeyValuePair<string, string>("client_id", clientId),
        new KeyValuePair<string, string>("client_secret", clientSecret)
    });    // OpenSky OAuth2 token endpoint
                var response = await client.PostAsync("https://auth.opensky-network.org/auth/realms/opensky-network/protocol/openid-connect/token", requestData);
                var json = await response.Content.ReadAsStringAsync();

                // Extract access_token from JSON response
                using (var doc = JsonDocument.Parse(json))
                {
                    token = doc.RootElement.GetProperty("access_token").GetString();
                }

            }
            catch (HttpRequestException e)
            {
                Console.Error.WriteLine($"Error in getting Opensky Token {e.Message}");
            }
            return token;
        }
        public async Task<BaggageInfo> GetAllAsync()
        {
            string flight = Flights.FirstOrDefault().CallSign;
            string from = Flights.FirstOrDefault().EstDepartureAirport;
            //airportCodes.TryGetAirport(from,out Airport airport);


            Flights.RemoveAll(x => x.CallSign == flight);
            return new BaggageInfo
            {
                flight = flight,
                from = from,
                carousel = Faker.RandomNumber.Next(1, 15)

            };

        }

        public record FlightArrival(string icao24, long firstSeen, string EstDepartureAirport,
                            long lastSeen, string estArrivalAirport, string CallSign);
    }

}
