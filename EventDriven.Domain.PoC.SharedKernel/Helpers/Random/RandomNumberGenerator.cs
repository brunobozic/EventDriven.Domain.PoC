using System;
using System.Threading;

namespace EventDriven.Domain.PoC.SharedKernel.Helpers.Random
{
    public static class RandomNumberGenerator
    {
        private static int Seed = Environment.TickCount;

        private static readonly ThreadLocal<System.Random> randomWrapper =
            new(() => new System.Random(Interlocked.Increment(ref Seed)));

        public static System.Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}