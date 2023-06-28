using System;
using Grpc.Core;
using Sensory.Api.V1.Assistant;
using Sensory.Api.V1.Management;
using SensoryCloud.Src.TokenManager;

namespace SensoryCloud.Src.Services
{
    /// <summary>
    /// Handles cryptographic operations
    /// </summary>
    public class AssistantService
    {
        private readonly Config Config;
        private readonly ITokenManager TokenManager;
        private readonly Sensory.Api.V1.Assistant.AssistantService.AssistantServiceClient AssistantClient; 

        public AssistantService(Config config, ITokenManager tokenManager)
        {
            this.Config = config;
            this.TokenManager = tokenManager;
            this.AssistantClient = new Sensory.Api.V1.Assistant.AssistantService.AssistantServiceClient(config.GetChannel());
        }

        /// <summary>
        /// Send a Chat request to SensoryCloud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TextChatResponse Chat(TextChatRequest request)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            return this.AssistantClient.TextChat(request, metadata);
        }

    }
}
