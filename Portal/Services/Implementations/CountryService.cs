using Entities;
using Entities.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext _ctx;

        public CountryService(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IList<City> GetCitiesOfCountry(int countrId)
        {
            return _ctx.Cities.Where(i => i.CountryId == countrId).ToList();
        }

        public IList<Country> GetCountries()
        {
            return _ctx.Countries.Include(i => i.Translations).ToList(); 
        }

        public string GetCurrencyByBranchId(int branchId)
        {
           var country =  _ctx.Branches.Where(i => i.Id == branchId)
                .Include(i => i.City)
                .ThenInclude(i => i.Country)
                .Select(i => i.City.Country)
                .FirstOrDefault();

            if (country == null)
                return string.Empty;

            return country.Currency;
        }

        public Language GetLanguageByCode(string lang)
        {
            return _ctx.Languages.First(i => i.Code == lang);
        }

        public IList<Country> GetLocations()
        {
            return _ctx.Countries
                 .Include(i => i.Translations)
                 .Include(i => i.Cities)
                 .Where(i => i.Cities.Count() > 0)
                 .ToList();
        }
    }
}
