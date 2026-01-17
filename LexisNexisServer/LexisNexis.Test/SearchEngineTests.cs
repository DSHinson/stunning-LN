using LexisNexis.BLL.SearchEngine;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Seed;
using LexisNexis.DAL.Storage;
using LexisNexis.DAL.Storage.InMemory;
using System.Collections.Concurrent;

namespace LexisNexis.Tests.SearchEngine
{
    public class SearchEngineTests
    {
        private SearchEngine<Product, int> _searchEngine = null!;
        private IReadRepository<Product, int> _readRepo = null!;

        [SetUp]
        public void Setup()
        {
            IIdGenerator<int> categoryIdGenerator = new IntIdGenerator(1);
            IIdGenerator<int> productIdGenerator = new IntIdGenerator(1);

            ConcurrentDictionary<int, Product> productStore = new ConcurrentDictionary<int, Product>();
            ConcurrentDictionary<int, Category> categoryStore = new ConcurrentDictionary<int, Category>();

            // Use the seeded products from your ProductSeeder
            var categories = CategorySeeder.SeedCategories();
            var products = ProductSeeder.SeedProducts(categories);

            // Use in-memory repository with the seeded products
            _readRepo = new InMemoryRepository<Product, int>(productStore, categoryIdGenerator);
            IWriteRepository<Product, int> _writeRepo = (IWriteRepository<Product, int>)_readRepo;

            foreach (var product in products)
            {
                _writeRepo.AddAsync(product).ConfigureAwait(false);
            }

            _searchEngine = new SearchEngine<Product, int>(_readRepo);
        }

        [Test]
        public async Task SearchEngine_ShouldReturnProductsMatchingQuery()
        {
            // Arrange
            string query = "lptop"; // intentionally misspelled to test fuzzy scoring

            // Act
            var results = await _searchEngine.Search(query);

            // Assert
            Assert.IsNotNull(results);

            if (results is Result<IEnumerable<Product>>.Failure failure)
            {
                throw new Exception(failure.FailureMessage);
            }

            var success = (Result<IEnumerable<Product>>.Success)results;

            var resultList = success.Data.ToList();

            // Expect "Laptop" and "Laptop Case" to be matched
            Assert.IsTrue(resultList.Any(p => p.Name.Contains("Laptop")));
            Assert.IsTrue(resultList.Any(p => p.Name.Contains("Ultrabook Laptop")));

            // Products unrelated to query should not appear
            Assert.IsFalse(resultList.Any(p => p.Name.Contains("Smartphone")));
        }

        [Test]
        public async Task SearchEngine_ShouldReturnAll_WhenQueryIsEmpty()
        {
            // Arrange
            string query = "";

            // Act
            var results = await _searchEngine.Search(query);

            if (results is Result<IEnumerable<Product>>.Failure failure)
            {
                throw new Exception(failure.FailureMessage);
            }

            var success = (Result<IEnumerable<Product>>.Success)results;

            var resultList = success.Data.ToList();

            // Assert
            Assert.That(resultList.Count(), Is.EqualTo(_readRepo.GetAllAsync().Result is Result<IEnumerable<Product>>.Success s ? s.Data.Count() : 0));
        }
    }
}


