using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Country;

namespace TheOneCRM.Application.Interfaces.ICountry
{
    public interface ICountryService
    {
        Task<List<CountryJsonDto>> GetAllCountriesAsync();
    }
}
