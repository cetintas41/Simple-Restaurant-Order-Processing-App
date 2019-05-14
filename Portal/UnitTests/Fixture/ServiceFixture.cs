using Entities;
using Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Fixture
{
    public class ServiceFixture : IDisposable
    {
        public ApplicationDbContext MockApplicationDbContext { get; set; }

        public ServiceFixture()
        {
            SetupDbContext();
        }

        public void Dispose()
        {
            MockApplicationDbContext.Dispose();
        }

        private void SetupDbContext()
        {
            //Microsoft.EntityFrameworkCore.InMemory
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            MockApplicationDbContext = new ApplicationDbContext(dbContextOptions);
        }

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }

        public void SetupTables()
        {
            if (MockApplicationDbContext.Tables.Any()) return;

            MockApplicationDbContext.Tables.AddRange(new List<Table> {
                new Table{ Id = 1, BranchId = 1, Code = "ANH2FF", DateCreated = DateTime.Now, IsClosed = true, Name = "Table 1" },
                new Table{ Id = 2, BranchId = 1, Code = "ANH45K", DateCreated = DateTime.Now, IsClosed = false, Name = "Table 2" },
            });

            MockApplicationDbContext.SaveChanges();
        }

        public void SetupLanguages()
        {
            if (MockApplicationDbContext.Languages.Any()) return;

            MockApplicationDbContext.Languages.AddRange(new List<Language> {
                new Language { Id = 1, Code = "tr", DateCreated = DateTime.Now },
                new Language { Id = 2, Code = "en", DateCreated = DateTime.Now }
            });

            MockApplicationDbContext.SaveChanges();
        }

        public void SetupFirms()
        {
            if (MockApplicationDbContext.Firms.Any()) return;

            MockApplicationDbContext.Firms.AddRange(new List<Firm> {
                new Firm { Id = 1, IsActive = true, LogoPath = "sample_path_1", DateCreated = DateTime.Now, Name = "Firm 1", UserId = "ABCD"  },
                new Firm { Id = 2, IsActive = true, LogoPath = "sample_path_2", DateCreated = DateTime.Now, Name = "Firm 2", UserId = "DEFG"  }
            });

            MockApplicationDbContext.SaveChanges();
        }
    }
}
