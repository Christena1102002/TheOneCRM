using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TheOneCRM.Application.Interfaces.ICountry;
using TheOneCRM.Domain.Models.DTOs.Country;
using System.Text.Json;
namespace TheOneCRM.Application.Services.Country
{
    public class CountryService:ICountryService
    {
        private readonly IWebHostEnvironment _env;

        public CountryService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<List<CountryJsonDto>> GetAllCountriesAsync()
        {
            var path = Path.Combine(_env.WebRootPath, "data", "countries.json");

            if (!File.Exists(path))
                throw new KeyNotFoundException("countries.json file was not found.");


            var json = await File.ReadAllTextAsync(path);

            var countries = JsonSerializer.Deserialize<List<CountryJsonDto>>(json);

            if (countries == null || !countries.Any())
                throw new KeyNotFoundException("No countries were found in countries.json.");

            return countries;

        }
    }
}
