using Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTests.Fixture;
using Xunit;

namespace UnitTests
{
    public class TableServiceTest : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _fixture;
        private readonly TableService _tableService;

        public TableServiceTest(ServiceFixture fixture)
        {
            _fixture = fixture;
            _fixture.SetupLanguages();
            _fixture.SetupTables();

            _tableService = new TableService(_fixture.MockApplicationDbContext);
        }

        [Fact]
        public void Can_GetTableById_ReturnsTable()
        {
            var table = _tableService.GetTableById(1, 1);
            Assert.True(table.Id == 1);
        }

        [Fact]
        public void Can_GetTableById_ReturnsNull()
        {
            var table = _tableService.GetTableById(1, 2);
            Assert.True(table == null);
        }
    }
}
