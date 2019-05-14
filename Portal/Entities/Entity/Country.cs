using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Entities.Entity
{
    public class Country : EntityBase
    {
        public virtual ICollection<CountryTranslation> Translations { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public string Currency { get; set; }

        public string GetName(int langId)
        {
            var translation = Translations.FirstOrDefault(i => i.LanguageId == langId);
            return translation?.Name ?? string.Empty;
        }
    }
    public class CountryTranslation : EntityBase
    {

        [ForeignKey("Country")]
        public int CountryId  { get; set; }
        public virtual Country Country   { get; set; }

        [ForeignKey("Language")]
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public string Name { get; set; }
    }
}
