using System;
using Grpc.Core;

namespace SensoryCloud.Src
{
    public struct SDKInitConfig
    {
        public string FullyQualifiedDomainName;
        public bool IsConnectionSecure;
        public string TenantId;
        public EnrollmentType EnrollmentType;
        public string Credential;
        public string DeviceId;
        public string DeviceName;
    }

    public class Config
    {
        public string FullyQualifiedDomainName { get; private set; }
        public bool IsConnectionSecure { get; set; } = true;
        public string TenantId { get; private set; }
        public string DeviceId { get; private set; }

        private Channel Channel = null;

        public Config(SDKInitConfig config)
        {
            this.FullyQualifiedDomainName = config.FullyQualifiedDomainName;
            this.IsConnectionSecure = config.IsConnectionSecure;
            this.TenantId = config.TenantId;
            this.DeviceId = config.DeviceId;
        }

        public Config Connect()
        {
            if (this.IsConnectionSecure)
            {
                this.Channel = new Channel(this.FullyQualifiedDomainName, ChannelCredentials.SecureSsl);
            } else
            {
                this.Channel = new Channel(this.FullyQualifiedDomainName, ChannelCredentials.Insecure);
            }

            return this;
        }

        public Channel GetChannel()
        {
            if (this.Channel == null)
            {
                throw new ArgumentNullException("no connection has been established with Sensory Cloud. did you forget to call Connect()?");
            }

            return this.Channel;
        }
    }

    public enum EnrollmentType
    {
        None,
        SharedSecret,
        JWT
    }
}
