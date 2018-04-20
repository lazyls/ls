using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls.Common.Utility
{
    public class EqualityComparerFactory<T> : EqualityComparer<T>
    {
        private Func<T, T, bool> comparer;

        public override bool Equals(T x, T y)
        {
            var k = comparer(x, y);
            return comparer(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return 1;
        }

        public EqualityComparerFactory(Func<T, T, bool> comparer)
        {
            this.comparer = comparer;
        }
    }
}
