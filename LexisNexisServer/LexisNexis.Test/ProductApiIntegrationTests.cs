using LexisNexis.API;
using LexisNexis.Common.DTO;
using LexisNexis.DAL.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace LexisNexis.Tests.Integration
{
    [TestFixture]
    public class ProductApiIntegrationTests
    {
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;

        [OneTimeSetUp]
        public void Setup()
        {
            // Initialize the WebApplicationFactory with your Program class
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetProducts_ShouldReturnPaginatedProducts()
        {
            // Act
            var response = await _client.GetAsync("/api/products?page=1&pageSize=5");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<ProductPageResponse>();
            Assert.That(result.Page, Is.EqualTo(1));
            Assert.That(result.PageSize, Is.EqualTo(5));
            Assert.That(result.Data.Count, Is.LessThanOrEqualTo(5));
        }

        [Test]
        public async Task GetProductById_ShouldReturnProduct()
        {
            // Arrange: pick a seeded product id
            int testProductId = 1;

            // Act
            var response = await _client.GetAsync($"/api/products/{testProductId}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var product = await response.Content.ReadFromJsonAsync<Product>();
            Assert.That(product, Is.Not.Null);
            Assert.That(product.Id, Is.EqualTo(testProductId));
        }

        [Test]
        public async Task PutProduct_ShouldUpdateProduct()
        {
            // Arrange
            int testProductId = 1;
            var updateDto = new
            {
                Name = "Updated Product",
                Description = "Updated description",
                SKU = "UPD123",
                Price = 99.99m,
                Quantity = 10,
                CategoryId = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{testProductId}", updateDto);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var updatedProduct = await response.Content.ReadFromJsonAsync<Product>();
            Assert.That(updatedProduct.Name, Is.EqualTo("Updated Product"));
        }

        [Test]
        public async Task DeleteProduct_ShouldReturnNoContent()
        {
            // Arrange
            int testProductId = 2;

            // Act
            var response = await _client.DeleteAsync($"/api/products/{testProductId}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Verify deleted
            var getResponse = await _client.GetAsync($"/api/products/{testProductId}");
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}

