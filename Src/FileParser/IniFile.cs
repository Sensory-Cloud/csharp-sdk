using System.Collections.Generic;
using System.IO;
using System.Linq;


// Change this to match your program's normal namespace
namespace SensoryCloud.Src.FileParser
{
    class IniConfigKey
    {
        public static string FullyQualifiedDomain = "fullyQualifiedDomainName";
        public static string IsConnectionSecure = "isSecure";
        public static string TenantId = "tenantID";
        public static string EnrollmentType = "enrollmentType";
        public static string EnrollmentCredential = "credential";
        public static string DeviceId = "deviceID";
        public static string DeviceName = "deviceName";
    }

    class IniConfigEnrollmentCredentialType
    {
        public static string None = "none";
        public static string SharedSecret = "sharedSecret";
        public static string JWT = "jwt";
    }

    public class IniFile
    {
        string Path;
        private Dictionary<string, string> Contents;

        public SDKInitConfig LoadConfig()
        {
            Dictionary<string, string> Config = this.Load();
            bool IsConnectionSecure;
            bool.TryParse(Config[IniConfigKey.IsConnectionSecure], out IsConnectionSecure);

            EnrollmentType enrollmentType = EnrollmentType.None;
            string EnrollmentTypeString = Config[IniConfigKey.EnrollmentType];
            if (EnrollmentTypeString.Equals(IniConfigEnrollmentCredentialType.SharedSecret))
            {
                enrollmentType = EnrollmentType.SharedSecret;
            }
            else if (EnrollmentTypeString.Equals(IniConfigEnrollmentCredentialType.JWT))
            {
                enrollmentType = EnrollmentType.JWT;
            }

            return new SDKInitConfig {
                FullyQualifiedDomainName = Config[IniConfigKey.FullyQualifiedDomain],
                IsConnectionSecure = IsConnectionSecure,
                TenantId = Config[IniConfigKey.TenantId],
                EnrollmentType = enrollmentType,
                Credential = Config[IniConfigKey.EnrollmentCredential],
                DeviceId = Config[IniConfigKey.DeviceId],
                DeviceName = Config[IniConfigKey.DeviceName],
            };
        }

        public IniFile(string path)
        {
            this.Path = path;
        }

        public Dictionary<string, string> Load()
        {
            this.Contents = File.ReadLines(this.Path).Select(s => s.Split('=')).Select(s => new {
                key = s[0],
                value = string.Join("=", s.Select((o, n) => new {
                    n,
                    o
                }).Where(o => o.n > 0).Select(o => o.o))
            }).ToDictionary(o => o.key, o => o.value);

            return this.Contents;
        }

    }
}