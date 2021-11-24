using System.Collections.Generic;
using NUnit.Framework;
using SensoryCloud.Src.Services;

namespace Test
{
    [TestFixture]
    public class CryptoServiceTest
    {
        [Test]
        public void TestGetSecureRandomString()
        {
            int testNum = 100;
            HashSet<string> randomStrings = new HashSet<string>();

            for (int i = 0; i < testNum; i++)
            {
                string randomString = CryptoService.GetSecureRandomString(testNum + 1);
                Assert.AreEqual(randomString.Length, testNum + 1);
                randomStrings.Add(randomString);
            }

            Assert.AreEqual(randomStrings.Count, testNum);
        }
    }
}
