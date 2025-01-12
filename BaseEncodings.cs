using System.Globalization;

namespace simpleauthenticator
{
    internal static class BaseEncodings
    {
        private static readonly IReadOnlyDictionary<char, int> Base32Alphabet = BuildAlphabetLookupTable("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567");
        private static readonly IReadOnlyDictionary<char, int> Base64Alphabet = BuildAlphabetLookupTable("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/");

        /// <summary>
        /// Implement base32 encoding with default alphabet. 
        /// Defined by RFC4448 (https://datatracker.ietf.org/doc/html/rfc4648).
        /// Input is tolerant to character case and allows space characters anywhere (' ', '\r', '\n\, '\t' ) and does not require padding.
        /// E.g. 'NVSXG43BM5SQ====', 'NVSX G43B M5SQ==' and 'nvsx G43B m5sq' considered as a valid and equivalent input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromBase32String(string input)
        {
            int filteredLength = 0;
            foreach (var ch in input)
            {
                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n' || ch == '=')
                {
                    // Skip all whitespaces, line separators and padding symbols
                    continue;
                }

                filteredLength += 1;
            }

            if (filteredLength == 0)
            {
                return Array.Empty<byte>();
            }

            var destination = new byte[filteredLength * 5 / 8];
            var destinationIndex = 0;

            var buffer = 0;
            var bitsLeft = 0;
            for (var inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                char ch = input[inputIndex];

                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n' || ch == '=')
                {
                    // Skip all whitespaces, line separators and padding symbols
                    continue;
                }

                ch = char.ToUpper(ch, CultureInfo.InvariantCulture);

                if (!Base32Alphabet.TryGetValue(ch, out int alphabetIndex))
                {
                    throw new FormatException($"Illegal character at position {inputIndex}: '{ch}'.");
                }

                buffer <<= 5;
                buffer |= alphabetIndex & 31;
                bitsLeft += 5;
                if (bitsLeft >= 8)
                {
                    destination[destinationIndex++] = (byte)(buffer >> (bitsLeft - 8));
                    bitsLeft -= 8;
                }
            }

            return destination;
        }

        /// <summary>
        /// Implement base64 encoding with default alphabet. 
        /// Defined by RFC4448 (https://datatracker.ietf.org/doc/html/rfc4648).
        /// Input allows space characters anywhere (' ', '\r', '\n\, '\t' ) and does not require padding.
        /// E.g. 'AAAAAAAA=' and 'AAAA AAAA' considered as a valid and equivalent input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromBase64String(string input)
        {
            int filteredLength = 0;
            foreach (var ch in input)
            {
                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n' || ch == '=')
                {
                    // Skip all whitespaces, line separators and padding symbols
                    continue;
                }

                filteredLength += 1;
            }

            if (filteredLength == 0)
            {
                return Array.Empty<byte>();
            }

            var destination = new byte[filteredLength * 6 / 8];
            var destinationIndex = 0;

            var buffer = 0;
            var bitsLeft = 0;
            for (var inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                char ch = input[inputIndex];

                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n' || ch == '=')
                {
                    // Skip all whitespaces, line separators and padding symbols
                    continue;
                }

                if (!Base64Alphabet.TryGetValue(ch, out int charIndex))
                {
                    throw new FormatException($"Illegal character at position {inputIndex}: '{ch}'.");
                }

                buffer <<= 6;
                buffer |= charIndex & 63;
                bitsLeft += 6;
                if (bitsLeft >= 8)
                {
                    destination[destinationIndex++] = (byte)(buffer >> (bitsLeft - 8));
                    bitsLeft -= 8;
                }
            }

            return destination;
        }
    
        private static IReadOnlyDictionary<char, int> BuildAlphabetLookupTable(string alphabet) 
        {
            var dict = new Dictionary<char, int>(alphabet.Length);
            for(int index = 0; index < alphabet.Length; index++)
            {
                dict.Add(alphabet[index], index);
            }
            return dict;
        }
    }
}
