using Entities.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Firm> Firms { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuType> MenuTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<CountryTranslation> CountryTranslations { get; set; }

        public IConfiguration _config { get; set; }

        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole()
            {
                Id = "1cce1d2a-7f96-422a-a743-95ad1be71512",
                Name = "Firm",
                NormalizedName = "FIRM",
                ConcurrencyStamp = "a8d53fca-f0cb-465b-9b84-8ea4946d5ff7"
            });

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole()
            {
                Id = "300ec059-1c06-498b-a2b4-b9248feb05ed",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "f5b7f778-9bf8-4b12-88a2-82e37c663bae"
            });

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole()
            {
                Id = "540b569f-096c-49cb-9921-2a6c3b392c7b",
                Name = "Branch",
                NormalizedName = "BRANCH",
                ConcurrencyStamp = "98ade3c8-66fb-450a-816b-708772c852d9"
            });

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole()
            {
                Id = "7a980e68-79af-4bd0-ad6c-fe437fabe30a",
                Name = "Waiter",
                NormalizedName = "WAITER",
                ConcurrencyStamp = "087a6444-9e79-4dd0-8e8f-973058f0b8c7"
            });

            // Müşteri silinirse ilgili siparişler silinmez.
            builder.Entity<Customer>().HasMany(i => i.Orders).WithOne(i => i.Customer).OnDelete(DeleteBehavior.ClientSetNull);
            // Firma Kullancısı silinirse ilgili Firma objesi silinmez.
            builder.Entity<Firm>().HasOne(i => i.User).WithOne(i => i.Firm).OnDelete(DeleteBehavior.SetNull);
            // Menü silinirse ilgili siparişler silinmez.
            builder.Entity<Menu>().HasMany(i => i.Orders).WithOne(i => i.Menu).OnDelete(DeleteBehavior.ClientSetNull);
            // Garson silinirse ilgilili siparişler silinmez.
            builder.Entity<ApplicationUser>().HasMany(i => i.Orders).WithOne(i => i.Waiter).OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Country>().HasData(new Country { Id = 1, DateCreated = DateTime.Now, Currency = "TL" });
            builder.Entity<Country>().HasData(new Country { Id = 2, DateCreated = DateTime.Now, Currency = "USD" });

            builder.Entity<Language>().HasData(new Language { Id = 1, DateCreated = DateTime.Now, Code = "tr" });
            builder.Entity<Language>().HasData(new Language { Id = 2, DateCreated = DateTime.Now, Code = "en" });

            builder.Entity<CountryTranslation>().HasData(new CountryTranslation { Id = 1, CountryId = 1, LanguageId = 1, Name = "Türkiye" });
            builder.Entity<CountryTranslation>().HasData(new CountryTranslation { Id = 2, CountryId = 1, LanguageId = 2, Name = "Turkey" });
            builder.Entity<CountryTranslation>().HasData(new CountryTranslation { Id = 3, CountryId = 2, LanguageId = 1, Name = "ABD" });
            builder.Entity<CountryTranslation>().HasData(new CountryTranslation { Id = 4, CountryId = 2, LanguageId = 2, Name = "USA" });

            builder.Entity<City>().HasData(new City { Id = 1, Name = "Istanbul", CountryId = 1, DateCreated = DateTime.Now });
            builder.Entity<City>().HasData(new City { Id = 2, Name = "Ankara", CountryId = 1, DateCreated = DateTime.Now });
            builder.Entity<City>().HasData(new City { Id = 3, Name = "New York", CountryId = 2, DateCreated = DateTime.Now });
            builder.Entity<City>().HasData(new City { Id = 4, Name = "Washington", CountryId = 2, DateCreated = DateTime.Now });
        }
    }
}
