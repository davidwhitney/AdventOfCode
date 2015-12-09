using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Dec4
{
    public class AdventCoinMiner
    {
        public int Mine(string stub, int leadingZeroCount = 5)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                var hash = Hash(stub + i);

                if (hash.StartsWith(string.Join("", Enumerable.Repeat("0", leadingZeroCount))))
                {
                    return i;
                }
            }

            return 0;
        }

        private static string Hash(string stub)
        {
            var encodedPassword = new UTF8Encoding().GetBytes(stub);
            var hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hash)
                .Replace("-", string.Empty)
                .ToLower();
        }
    }
}