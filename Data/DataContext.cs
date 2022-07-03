using CountriesPopulation.Models;
using Microsoft.EntityFrameworkCore;

namespace CountriesPopulation.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Population> Populations { get; set; }
    }
}
