using System.Text;

namespace simpleauthenticator
{
    internal static class BaseEncodings
    {
        /// <summary>
        /// Implement base32 encoding with default alphabet. 
        /// Defined by RFC4448 (https://datatracker.ietf.org/doc/html/rfc4648).
        /// Input is tolerant to character case and allows space characters anywhere (' ', '\r', '\n\, '\t' ) and does not require padding.
        /// E.g. 'NVSXG43BM5SQ====', 'NVSX G43B M5SQ==' and 'nvsx G43B m5sq' considered as a valid and eqivalent input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromBase32String(string input)
        {
            var base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var base32Dict = new Dictionary<char, int>(base32Alphabet.Length);
            for(int index = 0; index < base32Alphabet.Length; index++)
            {
                base32Dict.Add(base32Alphabet[index], index);
            }

            StringBuilder builder = new StringBuilder();
            foreach (var ch in input)
            {
                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n' || ch == '=')
                {
                    // Skip all whitespaces, line separators and padding
                    continue;
                }

                builder.Append(char.ToUpper(ch));
            }

            input = builder.ToString();
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

        /// <summary>
        /// Implement base64 encoding with default alphabet. 
        /// Defined by RFC4448 (https://datatracker.ietf.org/doc/html/rfc4648).
        /// Input allows space characters anywhere (' ', '\r', '\n\, '\t' ) and does not require padding.
        /// E.g. 'AAAAAAAA=' and 'AAAA AAAA' considered as a valid and eqivalent input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromBase64String(string input)
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var base64Dict = new Dictionary<char, int>(alphabet.Length);
            for(int index = 0; index < alphabet.Length; index++)
            {
                base64Dict.Add(alphabet[index], index);
            }

            StringBuilder builder = new StringBuilder();
            foreach (var ch in input)
            {
                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n' || ch == '=')
                {
                    // Skip all whitespaces, line separators and padding
                    continue;
                }

                builder.Append(ch);
            }

            input = builder.ToString();
            if (input.Length == 0)
            {
                return Array.Empty<byte>();
            }

            var destination = new byte[input.Length * 6 / 8];
            var destinationIndex = 0;

            var buffer = 0;
            var bitsLeft = 0;
            for (var sourceIndex = 0; sourceIndex < input.Length; sourceIndex++)
            {
                char ch = input[sourceIndex];
                if (!base64Dict.TryGetValue(ch, out int charIndex))
                {
                    throw new FormatException($"Illegal character at position {sourceIndex}: '{ch}'.");
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
    }
}
