using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SensoryCloud.Src;
using SensoryCloud.Src.FileParser;

namespace Test
{
    [TestFixture]
    public class FileParserTest
    {

        [Test]
        public void TestParseIniFile()
        {
            var testDataRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Split("bin")[0] + "TestData/";

            // Ini file not found - throw error
            Assert.Throws<FileNotFoundException>(delegate
            {
                var iniFile = new IniFile(testDataRoot + "does_not_exist.ini");
                var test = iniFile.Load();
            }, "Load() should thow an error if the file does not exist");


            // Ini file missing entries
            Assert.Throws<KeyNotFoundException>(delegate
            {
                var iniFile = new IniFile(testDataRoot + "missing_data.ini");
                var test = iniFile.LoadConfig();
            }, "LoadConfig() should thow an error if the file is missing entries");

            // Ini file correct
            var iniFile = new IniFile(testDataRoot + "config.ini");
            var config = iniFile.LoadConfig();
            Assert.AreEqual(config.Credential, "credential");
            Assert.AreEqual(config.IsConnectionSecure, true);
            Assert.AreEqual(config.TenantId, "tenant");
            Assert.AreEqual(config.DeviceId, "deviceID");
            Assert.AreEqual(config.DeviceName, "deviceName");
            Assert.AreEqual(config.FullyQualifiedDomainName, "fqdn");
            Assert.AreEqual(config.EnrollmentType, EnrollmentType.None);
        }
    }
}
