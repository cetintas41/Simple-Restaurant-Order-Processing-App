using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Fixture
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<API.Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(new string[0])
                .UseStartup<FakeStartup>();
        }
    }
}
