using Entities.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface ICountryService
    {
        IList<Country> GetCountries();
        IList<City> GetCitiesOfCountry(int countrId);
        Language GetLanguageByCode(string lang);
        IList<Country> GetLocations();
        string GetCurrencyByBranchId(int branchId);
    }
}
