using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace CountriesPopulation.Services
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        public Repository()
        {
        }

        public async Task<T> Fetch()
        {
            HttpClient client = new HttpClient();
            string baseURL = $"https://countriesnow.space/api/v0.1/countries/population/cities";
            client.BaseAddress = new Uri(baseURL);

            HttpResponseMessage response = await client.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;

                var data = JsonConvert.DeserializeObject<T>(content);

                return await Task.FromResult(data);
            }
            else
            {
                return null;
            }
        }
    }
}
