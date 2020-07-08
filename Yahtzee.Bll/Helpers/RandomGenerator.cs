using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Yahtzee.Bll.Helpers
{
    public static class RandomGenerator
    {
        private static int SeedCount = 0;
        public static ThreadLocal<Random> TlRng = new ThreadLocal<Random>(() => new Random(GenerateSeed()));

        private static int GenerateSeed()
        {
            // note the usage of Interlocked, remember that in a shared context we can't just "SeedCount++"
            return (int)((DateTime.Now.Ticks << 4) + Interlocked.Increment(ref SeedCount));
        }
    }
}
