using Microsoft.Extensions.Caching.Memory;
using System.Buffers;
using System.Text;
using System.IO.Hashing;

namespace LexisNexis.Common.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        public Guid CreateKey(params object[] keyParts)
        {
            if (keyParts == null || keyParts.Length == 0)
            {
                throw new ArgumentException("At least one key part is required.");
            }

            int totalLength = 0;
            foreach (var part in keyParts)
            {
                totalLength += sizeof(int);
                totalLength += GetByteLength(part);
            }

            byte[] buffer = ArrayPool<byte>.Shared.Rent(totalLength);
            int offset = 0;

            try
            {
                foreach (var part in keyParts)
                {
                    byte[] bytes = GetBytes(part);

                    BitConverter.TryWriteBytes(buffer.AsSpan(offset, sizeof(int)),bytes.Length);

                    offset += sizeof(int);
                    bytes.CopyTo(buffer, offset);
                    offset += bytes.Length;
                }

                Span<byte> hash = stackalloc byte[16];
                XxHash3.Hash(buffer.AsSpan(0, offset), hash);

                return new Guid(hash);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private static int GetByteLength(object value)
        {
            return value switch
            {
                int => sizeof(int),
                long => sizeof(long),
                short => sizeof(short),
                byte => sizeof(byte),
                bool => sizeof(bool),
                Guid => 16,
                DateTime => sizeof(long),
                string s => Encoding.UTF8.GetByteCount(s),
                _ => Encoding.UTF8.GetByteCount((value ?? "").ToString()!)
            };
        }

        private static byte[] GetBytes(object value)
        {
            return value switch
            {
                int i => BitConverter.GetBytes(i),
                long l => BitConverter.GetBytes(l),
                short s => BitConverter.GetBytes(s),
                byte b => new[] { b },
                bool b => BitConverter.GetBytes(b),
                Guid g => g.ToByteArray(),
                DateTime dt => BitConverter.GetBytes(dt.ToBinary()),
                string s => Encoding.UTF8.GetBytes(s),
                _ => Encoding.UTF8.GetBytes((value ?? "").ToString()!)
            };
        }

        public CacheContainer<T> GetOrCreateCacheContainer<T>(Guid key)
        {
            return _cache.GetOrCreate(key, entry => new CacheContainer<T>(), new MemoryCacheEntryOptions() { Size = 1 });
        }

        public void Remove(Guid key)
        {
            _cache.Remove(key);
        }

        public void Set<T>(Guid key, T value, TimeSpan cacheDuration)
        {
          _cache.Set(key, value, new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = cacheDuration, Size = 1});
        }
    }
}
