using System;
using System.Security.Cryptography;

namespace EventDriven.Domain.PoC.SharedKernel.Helpers.Random
{
    public static class RandomStringHelper
    {
        public static string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}