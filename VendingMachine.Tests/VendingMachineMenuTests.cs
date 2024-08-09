using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace VendingMachine.Tests
{
    public class VendingMachineMenuTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public VendingMachineMenuTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async void MenuPage_ShouldDisplayProductListWithPricesAndQuantities()
        {
            // Act
            var response = await _httpClient.GetAsync("/Products");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Assert
            
        }
    }
}