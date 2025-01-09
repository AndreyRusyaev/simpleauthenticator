using System.Security.Cryptography;

namespace simpleauthenticator
{

    /// <summary>
    /// Implements HOTP tokens generation according RFC4226: https://www.rfc-editor.org/rfc/rfc4226.
    /// </summary>
    public static class Hotp
    {
        public const int DefaultTokenLength = 6;

        public const HmacAlgorithm DefaultHmacAlgorithm = HmacAlgorithm.Sha1;

        public static HotpToken Generate(
            byte[] secretKey,
            long counter,
            int tokenLength = DefaultTokenLength,
            HmacAlgorithm hashAlgorithm = DefaultHmacAlgorithm)
        {
            var counterBytes = ToBigEndianBytes(counter);

            return Generate(secretKey, counterBytes, tokenLength, hashAlgorithm);
        }

        public static HotpToken Generate(
            byte[] secretKey,
            byte[] counter,
            int tokenLength = DefaultTokenLength,
            HmacAlgorithm hashAlgorithm = DefaultHmacAlgorithm)
        {
            if (counter.Length > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(counter), counter.Length, "Counter byte array size must be exactly 8 bytes");
            }

            if (tokenLength > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(tokenLength), tokenLength, "Token length must be between 1 and 8");
            }

            var transformResult = GenerateDigest(secretKey, counter, hashAlgorithm);

            return new HotpToken(
                (int)Truncate(transformResult, tokenLength));
        }

        private static byte[] GenerateDigest(byte[] key, byte[] data, HmacAlgorithm hashFunction)
        {
            KeyedHashAlgorithm keyedHashAlgorithm;
            switch (hashFunction)
            {
                case HmacAlgorithm.Md5:
                    keyedHashAlgorithm = new HMACMD5(key);
                    break;
                case HmacAlgorithm.Sha1:
                    keyedHashAlgorithm = new HMACSHA1(key);
                    break;
                case HmacAlgorithm.Sha2_256:
                    keyedHashAlgorithm = new HMACSHA256(key);
                    break;
                case HmacAlgorithm.Sha2_512:
                    keyedHashAlgorithm = new HMACSHA512(key);
                    break;
                case HmacAlgorithm.Sha3_256:
                    keyedHashAlgorithm = new HMACSHA3_256(key);
                    break;
                case HmacAlgorithm.Sha3_512:
                    keyedHashAlgorithm = new HMACSHA3_512(key);
                    break;
                default:
                    throw new InvalidOperationException($"Algorithm '{hashFunction}' is not supported.");
            }

            return keyedHashAlgorithm.ComputeHash(data);
        }

        private static uint Truncate(byte[] digest, int tokenLength)
        {
            var offset = digest[digest.Length - 1] & 0xf; // last byte

            var token = (uint)((digest[offset] & 0x7f) << 24)
                | (uint)((digest[offset + 1] & 0xff) << 16)
                | (uint)((digest[offset + 2] & 0xff) << 8)
                | (uint)(digest[offset + 3] & 0xff);

            return token % (uint)Math.Pow(10, tokenLength);
        }

        private static byte[] ToBigEndianBytes(long input)
        {
            var bytes = new byte[8];
            bytes[0] = (byte)(input >> 56);
            bytes[1] = (byte)(input >> 48);
            bytes[2] = (byte)(input >> 40);
            bytes[3] = (byte)(input >> 32);
            bytes[4] = (byte)(input >> 24);
            bytes[5] = (byte)(input >> 16);
            bytes[6] = (byte)(input >> 8);
            bytes[7] = (byte)input;
            return bytes;
        }
    }
}
