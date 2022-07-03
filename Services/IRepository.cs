using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesPopulation.Services
{
    public interface IRepository<T> where T : class
    {
        Task<T> Fetch();
    }
}
