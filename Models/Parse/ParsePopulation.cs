using System.ComponentModel.DataAnnotations;

namespace CountriesPopulation.Parse
{
    public class ParsePopulation
    {
        [Key]
        public int Id { get; set; }
        public string year { get; set; }
        public string? value { get; set; }
        public string? sex { get; set; }
        public string? reliability { get; set; }
    }
}
