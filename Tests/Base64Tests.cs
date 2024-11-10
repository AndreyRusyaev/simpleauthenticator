using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace simpleauthenticator.Tests
{
    [TestClass]
    public class Base64Tests
    {
        [TestMethod]
        public void CanProperlyDecode()
        {
            // https://datatracker.ietf.org/doc/html/rfc4648#section-10
            var testCases = new List<(string, string)>
            {
                ("", ""),
                ("f", "Zg=="),
                ("fo", "Zm8="),
                ("foo", "Zm9v"),
                ("foob", "Zm9vYg=="),
                ("fooba", "Zm9vYmE="),
                ("foobar", "Zm9vYmFy"),
                ("message", "bWVzc2FnZQ==")
            };

            foreach (var testCase in testCases)
            {
                var input = testCase.Item2;
                var expectedResult = testCase.Item1;

                var actualResult = Encoding.UTF8.GetString(BaseEncodings.FromBase64String(input));

                Assert.AreEqual(expectedResult, actualResult, "TestCase failed.");
            }
        }
    
        [TestMethod]
        public void CanProperlyDecodeWithSpacesAndWithoutPadding()
        {
            var testCases = new List<(string, string)>
            {
                ("message", "bWVzc2FnZQ=="),
                ("message", "bWVz c2Fn ZQ=="),
                ("message", "bWVzc2FnZQ"),
                ("message", "bWVz c2Fn ZQ"),
                ("message", "bWVz\nc2Fn\t ZQ       "),
            };

            foreach (var testCase in testCases)
            {
                var input = testCase.Item2;
                var expectedResult = testCase.Item1;

                var actualResult = Encoding.UTF8.GetString(BaseEncodings.FromBase64String(input));

                Assert.AreEqual(expectedResult, actualResult, "TestCase failed.");
            }
        }
    }
}