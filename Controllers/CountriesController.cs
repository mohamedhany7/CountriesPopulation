using CountriesPopulation.Data;
using CountriesPopulation.Models;
using CountriesPopulation.Parse;
using CountriesPopulation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
//using Newtonsoft.Json.Linq;

namespace CountriesPopulation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _db;
        private readonly ILogger<CountriesController> _logger;
        //private readonly IRepository<Country> _repository;

        public CountriesController(ILogger<CountriesController> logger, DataContext db)
        {
            _logger = logger;
            _db = db;
           //_repository = repository;
        }

        [HttpGet(Name = "SyncCountries")]
        public async Task GetAsync()
        {
            HttpClient client = new HttpClient();
            string baseURL = $"https://countriesnow.space/api/v0.1/countries/population/cities";
            client.BaseAddress = new Uri(baseURL);

            HttpResponseMessage response = await client.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;

                var data = JsonConvert.DeserializeObject<ParseData>(content);
                if (data != null)
                {
                    var records = from m in _db.Countries
                                  select m;
                    var records2 = from m in _db.Populations
                                   select m;
                    foreach (var record in records)
                    {
                        _db.Countries.Remove(record);
                    }
                    foreach (var record in records2)
                    {
                        _db.Populations.Remove(record);
                    }
                    _db.SaveChanges();

                    foreach (var d in data.data)
                    {
                        var newCityRecords = new Country();
                        newCityRecords.city = d.city;
                        newCityRecords.countryName = d.country;

                        _db.Countries.Add(newCityRecords);
                    }

                    await _db.SaveChangesAsync();
                    foreach (var d in data.data)
                    { 
                        var customer = _db.Countries.Where(x => x.city == d.city).First();
                        var key = customer.Id;
                        foreach (var pop in d.populationCounts)
                        {
                            var newPopulationRecords = new Population();
                            newPopulationRecords.year = pop.year;
                            newPopulationRecords.value = pop.value;
                            newPopulationRecords.sex = pop.sex;
                            newPopulationRecords.reliability = pop.reliability;
                            newPopulationRecords.countryId = key;

                            _db.Populations.Add(newPopulationRecords);
                        }
                    }

                    await _db.SaveChangesAsync();
                }
            }
        }

        [HttpPost(Name = "ListCountries")]
        public async Task<ActionResult<List<String>>> GetCountries()
        {
            var list = _db.Countries.Select(m => m.countryName).Distinct();
            if (list == null)
                return BadRequest("list not found");

            return Ok(list.ToList());
        }

        [HttpGet("{countryName}")]
        public async Task<ActionResult<List<String>>> Get(string countryName)
        {
            var list = _db.Countries.FromSqlRaw("select Populations.year, SUM(CONVERT(INT,Populations.value)) AS 'Value' from Countries Join Populations on Countries.Id = Populations.countryId WHERE countryName = {0} Group By Populations.year;",countryName)
                        .ToList();
            return Ok(list);
        }
    }


}


