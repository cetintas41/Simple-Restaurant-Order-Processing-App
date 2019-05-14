using API.Models;
using FluentAssertions;
using IntegrationTests.Fixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class CustomerControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<API.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<API.Startup> _factory;

        public CustomerControllerIntegrationTest(CustomWebApplicationFactory<API.Startup> factory)
        {
            _factory = factory;

            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false

            });
        }

        [Fact]
        public async Task Can_Get_Locations()
        {
            var response = await _client.GetAsync("api/customer/location");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response.Content.ReadAsStringAsync().Result.Should().NotBeNull();
        }

        [Fact]
        public async Task Can_DeleteOrder()
        {
            var orders = new List<string> { "aBj78IfsgLV", "bdssOk9ihdG" };

            var model = new DeleteOrderModel
            {
                IPAddress = "SAMPLE_IP_ADDRESS",
                MacAddress = "SAMPLE_MAC_ADDRESS",
                Orders = orders                   
            };

            var json = JsonConvert.SerializeObject(model);

            var response = await _client.PostAsync("api/customer/cancel", new StringContent(json, encoding: Encoding.UTF8, mediaType: "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response.Content.ReadAsStringAsync().Result.Should().NotBeNull();
        }


    }
}
