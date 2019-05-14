using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTests.Fixture;
using Xunit;

namespace UnitTests
{
    public class FirmServiceTest : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _fixture;
        private readonly FirmService _firmService;

        public FirmServiceTest(ServiceFixture fixture)
        {
            _fixture = fixture;
            _fixture.SetupLanguages();
            _fixture.SetupTables();
            _fixture.SetupFirms();

            //mocks
            var mockHostingEnvironment = new Mock<IHostingEnvironment>();
            mockHostingEnvironment.Setup(i => i.WebRootPath).Returns("sample_web_root_path");

            var mockUserManager = new Mock<UserManager<ApplicationUser>>();

            _firmService = new FirmService(mockUserManager.Object, _fixture.MockApplicationDbContext, mockHostingEnvironment.Object);
        }

        [Fact]
        public void Can_GetFirmById_ReturnsFirm()
        {
            var firm = _firmService.GetFirmById(1);
            Assert.True(firm.Id == 1);
        }
    }
}
