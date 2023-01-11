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
            var currentTime = 
                new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);

            Assert.AreEqual(46176038, Totp.Generate(secretKey, currentTime, tokenLength: 8));
            Assert.AreEqual(6176038, Totp.Generate(secretKey, currentTime, tokenLength: 7));
            Assert.AreEqual(176038, Totp.Generate(secretKey, currentTime, tokenLength: 6));
            Assert.AreEqual(76038, Totp.Generate(secretKey, currentTime, tokenLength: 5));
            Assert.AreEqual(6038, Totp.Generate(secretKey, currentTime, tokenLength: 4));
            Assert.AreEqual(38, Totp.Generate(secretKey, currentTime, tokenLength: 3));
            Assert.AreEqual(38, Totp.Generate(secretKey, currentTime, tokenLength: 2));
            Assert.AreEqual(8, Totp.Generate(secretKey, currentTime, tokenLength: 1));
        }
    }
}
