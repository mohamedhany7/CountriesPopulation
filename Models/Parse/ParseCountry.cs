using System.ComponentModel.DataAnnotations;

namespace CountriesPopulation.Parse
{
    public class ParseCountry
    {
        [Key]
        public int Id { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public ParsePopulation[] populationCounts { get; set; }
    }
}
