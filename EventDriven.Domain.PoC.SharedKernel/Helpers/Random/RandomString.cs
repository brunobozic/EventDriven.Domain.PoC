using System.Linq;

namespace EventDriven.Domain.PoC.SharedKernel.Helpers.Random
{
    public static class RandomString
    {
        public static string GetRandomString(int length)
        {
            var random = new System.Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}