using System.Net;

namespace ConventionBooking.Migration
{

    public class PopulateVenues {

        public async void Populate() {

            using (HttpClient client = new HttpClient())
            {
                using var response = await client.GetAsync("https://api.openbrewerydb.org/breweries?per_page=3");
                response.EnsureSuccessStatusCode();

                var breweries = await response.Content.ReadFromJsonAsync<List<Brewery>>();
                if (breweries == null) {
                    return;
                }

                foreach (Brewery b in breweries) {
                    Console.WriteLine($"brewery: {b.ID}");
                }

            }
        }
    }

    public class Brewery
    {
        public string? ID {get;set;}
        public string? Name {get;set;}
        public string? Street {get;set;}
        public string? City {get;set;}
        public string? State {get;set;}
        public string? Country {get;set;}
    }

}