using System;
using Grpc.Core;

namespace SensoryCloud.Src
{
    public class Config
    {
        public string FullyQualifiedDomainName { get; private set; }
        public bool IsConnectionSecure { get; set; } = true;
        public string TenantId { get; private set; }
        public string DeviceId { get; private set; }

        private Channel Channel = null;

        public Config(string fullyQualifiedDomainName, string tenantId, string deviceId)
        {
            this.FullyQualifiedDomainName = fullyQualifiedDomainName;
            this.TenantId = tenantId;
            this.DeviceId = deviceId;
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
}
