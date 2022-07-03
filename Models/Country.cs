using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountriesPopulation.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string city { get; set; }
        public string countryName { get; set; }
        public ICollection<Population> Populations { get; set; }
    }
}
