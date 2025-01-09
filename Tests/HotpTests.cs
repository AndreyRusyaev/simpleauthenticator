using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace simpleauthenticator.Tests
{
    [TestClass]
    public class HotpTests
    {
        [TestMethod]
        public void GenerateBasicHotpToken()
        {
            byte[] secretKey = BaseEncodings.FromBase32String("ABCD EFGH IJKL");
            long counter = 123456;

            Assert.AreEqual(47973251, Hotp.Generate(secretKey, counter, tokenLength: 8).Value);
            Assert.AreEqual(7973251, Hotp.Generate(secretKey, counter, tokenLength: 7).Value);
            Assert.AreEqual(973251, Hotp.Generate(secretKey, counter, tokenLength: 6).Value);
            Assert.AreEqual(73251, Hotp.Generate(secretKey, counter, tokenLength: 5).Value);
            Assert.AreEqual(3251, Hotp.Generate(secretKey, counter, tokenLength: 4).Value);
            Assert.AreEqual(251, Hotp.Generate(secretKey, counter, tokenLength: 3).Value);
            Assert.AreEqual(51, Hotp.Generate(secretKey, counter, tokenLength: 2).Value);
            Assert.AreEqual(1, Hotp.Generate(secretKey, counter, tokenLength: 1).Value);
        }
    }
}
