using System;
using System.Threading;

namespace SharedKernel.Helpers.Random;

public static class RandomNumberGenerator
{
    private static readonly ThreadLocal<System.Random> randomWrapper =
        new(() => new System.Random(Interlocked.Increment(ref Seed)));

    private static int Seed = Environment.TickCount;

    public static System.Random GetThreadRandom()
    {
        return randomWrapper.Value;
    }
}