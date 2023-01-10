using System.Runtime.Serialization.Json;

namespace simpleauthenticator
{
    internal static class BaseEncodings
    {
        public static byte[] FromBase32String(string input)
        {
            var base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var base32Dict = new Dictionary<char, int>(base32Alphabet.Length);
            for(int index = 0; index < base32Alphabet.Length; index++)
            {
                base32Dict.Add(base32Alphabet[index], index);
            }

            input = input.Trim().TrimEnd('=').ToUpper();
            if (input.Length == 0)
            {
                return Array.Empty<byte>();
            }

            var destination = new byte[input.Length * 5 / 8];
            var destinationIndex = 0;

            var buffer = 0;
            var bitsLeft = 0;
            for (var sourceIndex = 0; sourceIndex < input.Length; sourceIndex++)
            {
                char ch = input[sourceIndex];
                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n')
                {
                    // Skip all whitespaces and line separators
                    continue;
                }

                if (!base32Dict.TryGetValue(ch, out int charIndex))
                {
                    throw new FormatException($"Illegal character at position {sourceIndex}: '{ch}'.");
                }

                buffer <<= 5;
                buffer |= charIndex & 31;
                bitsLeft += 5;
                if (bitsLeft >= 8)
                {
                    destination[destinationIndex++] = (byte)(buffer >> (bitsLeft - 8));
                    bitsLeft -= 8;
                }
            }

            return destination;
        }

        public static byte[] FromBase64String(string input)
        {
            return Convert.FromBase64String(input);
        }
    }
}
