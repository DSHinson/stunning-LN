using Microsoft.Extensions.Caching.Memory;
using LexisNexis.Common.Cache;

namespace LexisNexis.Tests.Cache
{
    [TestFixture]
    public class CacheServiceTests
    {
        private IMemoryCache _memoryCache = null!;
        private CacheService _cacheService = null!;

        [SetUp]
        public void SetUp()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 100
            });

            _cacheService = new CacheService(_memoryCache);
        }

        [Test]
        public void CreateKey_SameInput_ProducesSameGuid()
        {
            var key1 = _cacheService.CreateKey(1, "abc", true);
            var key2 = _cacheService.CreateKey(1, "abc", true);

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void CreateKey_DifferentOrder_ProducesDifferentGuid()
        {
            var key1 = _cacheService.CreateKey(1, "abc");
            var key2 = _cacheService.CreateKey("abc", 1);

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void CreateKey_DifferentTypes_ProducesDifferentGuid()
        {
            var key1 = _cacheService.CreateKey(1);
            var key2 = _cacheService.CreateKey(1L);

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void CreateKey_WithGuidAndDateTime_IsDeterministic()
        {
            var guid = Guid.NewGuid();
            var date = DateTime.UtcNow;

            var key1 = _cacheService.CreateKey(guid, date);
            var key2 = _cacheService.CreateKey(guid, date);

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void CreateKey_NoParts_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _cacheService.CreateKey());
        }

        [Test]
        public void SetAndGet_ReturnsStoredValue()
        {
            var key = _cacheService.CreateKey("product", 1);
            var expected = "cached-value";

            _cacheService.Set(key, expected, TimeSpan.FromMinutes(1));

            var actual = _memoryCache.Get<string>(key);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Remove_DeletesCachedValue()
        {
            var key = _cacheService.CreateKey("remove-test");
            _cacheService.Set(key, 123, TimeSpan.FromMinutes(1));

            _cacheService.Remove(key);

            var exists = _memoryCache.TryGetValue(key, out _);

            Assert.That(exists, Is.False);
        }

        [Test]
        public void GetOrCreateCacheContainer_SameKey_ReturnsSameInstance()
        {
            var key = _cacheService.CreateKey("container");

            var container1 = _cacheService.GetOrCreateCacheContainer<int>(key);
            var container2 = _cacheService.GetOrCreateCacheContainer<int>(key);

            Assert.That(container1, Is.SameAs(container2));
        }

        [TearDown]
        public void TearDown()
        {
            _memoryCache.Dispose();
        }
    }
}
