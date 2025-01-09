using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace simpleauthenticator.Tests
{
    [TestClass]
    public class TotpTests
    {
        [TestMethod]
        public void GenerateBasicTotpToken()
        {
            byte[] secretKey = BaseEncodings.FromBase32String("ABCD EFGH IJKL");
            DateTimeOffset initialDateTime = 
                new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

            Assert.AreEqual(88855042, Totp.Generate(secretKey, initialDateTime, tokenLength: 8).Value);
            Assert.AreEqual(8855042, Totp.Generate(secretKey, initialDateTime, tokenLength: 7).Value);
            Assert.AreEqual(855042, Totp.Generate(secretKey, initialDateTime, tokenLength: 6).Value);
            Assert.AreEqual(55042, Totp.Generate(secretKey, initialDateTime, tokenLength: 5).Value);
            Assert.AreEqual(5042, Totp.Generate(secretKey, initialDateTime, tokenLength: 4).Value);
            Assert.AreEqual(42, Totp.Generate(secretKey, initialDateTime, tokenLength: 3).Value);
            Assert.AreEqual(42, Totp.Generate(secretKey, initialDateTime, tokenLength: 2).Value);
            Assert.AreEqual(2, Totp.Generate(secretKey, initialDateTime, tokenLength: 1).Value);
        }
    }
}
