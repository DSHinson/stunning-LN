using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.DAL.Storage.InMemory
{
    public sealed class IntIdGenerator : IIdGenerator<int>
    {
        private int _current;

        public IntIdGenerator(int startAt = 1)
        {
            _current = startAt - 1;
        }

        public int Next() => Interlocked.Increment(ref _current);
    }

}
