using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.Cache
{
    public class CacheContainer<T>
    {
        private readonly SemaphoreSlim _lock;
        public T Data { get; set; }
        public CacheContainer()
        {
            _lock = new(1, 1);
        }
        public async Task LockAsync() => await _lock.WaitAsync();

        public void Release() => _lock.Release();
    }
}
