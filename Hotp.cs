using System.Security.Cryptography;

namespace simpleauthenticator
{
    public enum HmacHashAlgorithm
    {
        Md5,
        Sha1,
        Sha256,
        Sha512
    }

    public static class Hotp
    {
        public static int Generate(byte[] secretKey, long counter, int tokenLength = 6)
        {
            var counterBytes = ToBigEndianBytes(counter);

            return Generate(secretKey, counterBytes, tokenLength);
        }

        public static int Generate(byte[] secretKey, byte[] counter, int tokenLength = 6, HmacHashAlgorithm hashAlgorithm = HmacHashAlgorithm.Sha1)
        {
            if (counter.Length > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(counter), counter.Length, "Counter byte array size must be exactly 8 bytes");
            }

            if (tokenLength > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(tokenLength), tokenLength, "Token length must be between 1 and 8");
            }

            return (int)Truncate(Transform(secretKey, counter, hashAlgorithm), tokenLength);
        }

        private static byte[] Transform(byte[] key, byte[] data, HmacHashAlgorithm hashFunction)
        {
            KeyedHashAlgorithm keyedHashAlgorithm;
            switch (hashFunction)
            {
                case HmacHashAlgorithm.Md5:
                    keyedHashAlgorithm = new HMACMD5(key);
                    break;
                case HmacHashAlgorithm.Sha1:
                    keyedHashAlgorithm = new HMACSHA1(key);
                    break;
                case HmacHashAlgorithm.Sha256:
                    keyedHashAlgorithm = new HMACSHA256(key);
                    break;
                case HmacHashAlgorithm.Sha512:
                    keyedHashAlgorithm = new HMACSHA512(key);
                    break;
                default:
                    throw new InvalidOperationException($"Algorithm '{hashFunction}' is not supported.");
            }

            return keyedHashAlgorithm.ComputeHash(data);
        }

        private static uint Truncate(byte[] digest, int tokenLength)
        {
            var offset = digest[19] & 0xf;

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
