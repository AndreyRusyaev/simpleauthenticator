using System.Security.Cryptography;

namespace simpleauthenticator
{
    public static class Hotp
    {
        public static int Generate(byte[] secretKey, long counter, int tokenLength = 6)
        {
            var counterBytes = ToBigEndianBytes(counter);

            return (int)Truncate(HMACSHA1.HashData(secretKey, counterBytes), tokenLength);
        }

        public static int Generate(byte[] secretKey, byte[] counter, int tokenLength = 6)
        {
            if (counter.Length > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(counter), counter.Length, "Counter byte array size must be exactly 8 bytes");
            }

            if (tokenLength > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(tokenLength), tokenLength, "Token length must be between 1 and 8");
            }

            return (int)Truncate(HMACSHA1.HashData(secretKey, counter), tokenLength);
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

        static byte[] ToBigEndianBytes(long input)
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
