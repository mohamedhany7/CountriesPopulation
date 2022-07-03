using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountriesPopulation.Models
{
    public class Population
    {
        [Key]
        public int Id { get; set; }
        public string year { get; set; }
        public string? value { get; set; }
        public string? sex { get; set; }
        public string? reliability { get; set; }

        [ForeignKey("countryId")]
        public int? countryId { get; set; }
        public Country? country { get; set; }
    }
}
