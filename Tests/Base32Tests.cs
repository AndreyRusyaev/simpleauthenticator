using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace simpleauthenticator.Tests
{
    [TestClass]
    public class Base32Tests
    {
        [TestMethod]
        public void CanProperlyDecodeBase32()
        {
            // https://datatracker.ietf.org/doc/html/rfc4648#section-10
            var testCases = new List<(string, string)>
            {
                ("", ""),
                ("f", "MY"),
                ("fo", "MZXQ===="),
                ("foo", "MZXW6==="),
                ("foob", "MZXW6YQ="),
                ("fooba", "MZXW6YTB"),
                ("foobar", "MZXW6YTBOI======"),
                ("message", "NVSXG43BM5SQ====")
            };

            foreach (var testCase in testCases)
            {
                var input = testCase.Item2;
                var expectedResult = testCase.Item1;

                var actualResult = Encoding.UTF8.GetString(BaseEncodings.FromBase32String(input));

                Assert.AreEqual(expectedResult, actualResult, "TestCase failed.");
            }
        }

        [TestMethod]
        public void CanProperlyDecodeWithSpacesAndWithoutPadding()
        {
            var testCases = new List<(string, string)>
            {
                ("message", "NVSXG43BM5SQ===="),
                ("message", "NVSX G43B M5SQ===="),
                ("message", "NVSX G43B M5SQ"),
                ("message", "nvsxg43bm5sq===="),
                ("message", "nvsxg43bm5sq"),
                ("message", "nvsx g43b m5sq"),
                ("message", "NVSX\ng43b\tM5SQ     "),
            };

            foreach (var testCase in testCases)
            {
                var input = testCase.Item2;
                var expectedResult = testCase.Item1;

                var actualResult = Encoding.UTF8.GetString(BaseEncodings.FromBase32String(input));

                Assert.AreEqual(expectedResult, actualResult, "TestCase failed.");
            }
        }
    }
}