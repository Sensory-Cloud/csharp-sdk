using System;
using System.Threading.Tasks;
using Grpc.Core;
using Sensory.Api.V1.Audio;
using SensoryCloud.Src.TokenManager;

namespace SensoryCloud.Src.Services
{
    /// <summary>
    /// Handles all audio requests to Sensory Cloud
    /// </summary>
    public class AudioService
    {
        private readonly Config Config;
        private readonly ITokenManager TokenManager;
        private readonly AudioModels.AudioModelsClient AudioModelsClient;
        private readonly AudioBiometrics.AudioBiometricsClient AudioBiometricsClient;
        private readonly AudioEvents.AudioEventsClient AudioEventsClient;
        private readonly AudioTranscriptions.AudioTranscriptionsClient AudioTranscriptionsClient;

        public AudioService(Config config, ITokenManager tokenManager)
        {
            this.Config = config;
            this.TokenManager = tokenManager;
            this.AudioModelsClient = new AudioModels.AudioModelsClient(config.GetChannel());
            this.AudioBiometricsClient = new AudioBiometrics.AudioBiometricsClient(config.GetChannel());
            this.AudioEventsClient = new AudioEvents.AudioEventsClient(config.GetChannel());
            this.AudioTranscriptionsClient = new AudioTranscriptions.AudioTranscriptionsClient(config.GetChannel());
        }

        protected AudioService(
            Config config,
            ITokenManager tokenManager,
            AudioModels.AudioModelsClient audioModelsClient,
            AudioBiometrics.AudioBiometricsClient audioBiometricsClient,
            AudioEvents.AudioEventsClient audioEventsClient,
            AudioTranscriptions.AudioTranscriptionsClient audioTranscriptionsClient)
        {
            this.Config = config;
            this.TokenManager = tokenManager;
            this.AudioModelsClient = audioModelsClient;
            this.AudioBiometricsClient = audioBiometricsClient;
            this.AudioEventsClient = audioEventsClient;
            this.AudioTranscriptionsClient = audioTranscriptionsClient;
        }

        /// <summary>
        /// Fetch all the audio models supported by your instance of Sensory Cloud.
        /// </summary>
        /// <returns>the supported models</returns>
        public GetModelsResponse GetModels()
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            GetModelsRequest request = new GetModelsRequest();

            return this.AudioModelsClient.GetModels(request, metadata);
        }

        /// <summary>
        /// Stream audio to Sensory Cloud as a means for user enrollment.
        /// Only biometric-typed models are supported by the method.
        /// </summary>
        /// <param name="audioConfig">the audio-specifc configuration. Currently only LIN16 at 16000Hz is supported.</param>
        /// <param name="description">a description of this enrollment. Useful if a user could have multiple enrollments, as it helps differentiate between them.</param>
        /// <param name="userId">the unique userId for this enrollment.</param>
        /// <param name="modelName">the exact name of the model you intend to enroll into. This model name can be retrieved from the getModels() call.</param>
        /// <param name="isLivenessEnabled">indicates if liveness is enabled for this request</param>
        /// <returns>a bidirectional stream where CreateEnrollmentRequests can be passed to the cloud and CreateEnrollmentResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<CreateEnrollmentRequest, CreateEnrollmentResponse>> StreamEnrollment(
            AudioConfig audioConfig, string description, string userId, string modelName, bool isLivenessEnabled)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<CreateEnrollmentRequest, CreateEnrollmentResponse> enrollmentStream = this.AudioBiometricsClient.CreateEnrollment(metadata);

            CreateEnrollmentConfig config = new CreateEnrollmentConfig
            {
                Audio = audioConfig,
                Description = description,
                UserId = userId,
                DeviceId = this.Config.DeviceId,
                ModelName = modelName,
                IsLivenessEnabled = isLivenessEnabled,
            };

            CreateEnrollmentRequest request = new CreateEnrollmentRequest{ Config = config };

            await enrollmentStream.RequestStream.WriteAsync(request);
            return enrollmentStream;
        }

        /// <summary>
        /// Authenticate against an existing audio enrollment in Sensory Cloud.
        /// Only biometric-typed models are supported by the method.
        /// </summary>
        /// <param name="audioConfig">the audio-specifc configuration. Currently only LIN16 at 16000Hz is supported.</param>
        /// <param name="enrollmentId">the ID assocaited with the enrollment</param>
        /// <param name="isLivenessEnabled">indicates if liveness is enabled for this request.</param>
        /// <param name="sensitivity">the sensitivity of the recognition engine. Defaults to medium.</param>
        /// <param name="security">the security threshold enforced by the recognition engine. Defaults to high.</param>
        /// <returns>a bidirectional stream where AuthenticateRequests can be passed to the cloud and AuthenticateResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse>> StreamAuthentication(
            AudioConfig audioConfig, string enrollmentId,  bool isLivenessEnabled, ThresholdSensitivity sensitivity = ThresholdSensitivity.Medium, AuthenticateConfig.Types.ThresholdSecurity security = AuthenticateConfig.Types.ThresholdSecurity.High)
        {
            AuthenticateConfig authenticateConfig = new AuthenticateConfig
            {
                Audio = audioConfig,
                EnrollmentId = enrollmentId,
                Sensitivity = sensitivity,
                Security = security,
                IsLivenessEnabled = isLivenessEnabled
            };
            
            return await this.StreamAuthentication(authenticateConfig);
        }

        /// <summary>
        /// Authenticate against an existing audio enrollment in Sensory Cloud.
        /// </summary>
        /// <param name="audioConfig">the audio-specifc configuration. Currently only LIN16 at 16000Hz is supported.</param>
        /// <param name="enrollmentGroupId">the ID assocaited with the enrollment group</param>
        /// <param name="isLivenessEnabled">indicates if liveness is enabled for this request.</param>
        /// <param name="sensitivity">the sensitivity of the recognition engine. Defaults to medium.</param>
        /// <param name="security">the security threshold enforced by the recognition engine. Defaults to high.</param>
        /// <returns>a bidirectional stream where AuthenticateRequests can be passed to the cloud and AuthenticateResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse>> StreamGroupAuthentication(
            AudioConfig audioConfig, string enrollmentGroupId, bool isLivenessEnabled, ThresholdSensitivity sensitivity = ThresholdSensitivity.Medium, AuthenticateConfig.Types.ThresholdSecurity security = AuthenticateConfig.Types.ThresholdSecurity.High)
        {
            AuthenticateConfig authenticateConfig = new AuthenticateConfig
            {
                Audio = audioConfig,
                EnrollmentGroupId = enrollmentGroupId,
                Sensitivity = sensitivity,
                Security = security,
                IsLivenessEnabled = isLivenessEnabled
            };

            return await this.StreamAuthentication(authenticateConfig);
        }

        /// <summary>
        /// Stream audio to Sensory Cloud in order to recognize a specific phrase or sound
        /// </summary>
        /// <param name="audioConfig">the audio-specifc configuration. Currently only LIN16 at 16000Hz is supported.</param>
        /// <param name="userId">the unique userId for the user requesting this event</param>
        /// <param name="modelName">the exact name of the model you intend to recognize against. This model name can be retrieved from the getModels() call.</param>
        /// <param name="sensitivity">the sensitivity of the recognition engine. Defaults to medium.</param>
        /// <returns>a bidirectional stream where ValidateEventRequests can be passed to the cloud and ValidateEventResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<ValidateEventRequest, ValidateEventResponse>> StreamEvent(
            AudioConfig audioConfig, string userId, string modelName, ThresholdSensitivity sensitivity = ThresholdSensitivity.Medium)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<ValidateEventRequest, ValidateEventResponse> eventStream = this.AudioEventsClient.ValidateEvent(metadata);

            ValidateEventConfig config = new ValidateEventConfig
            {
                Audio = audioConfig,
                UserId = userId,
                ModelName = modelName,
                Sensitivity = sensitivity
            };

            ValidateEventRequest request = new ValidateEventRequest { Config = config };

            await eventStream.RequestStream.WriteAsync(request);
            return eventStream;
        }

        /// <summary>
        /// Stream audio to Sensory Cloud as a means for audio enrollment.
        /// This method is similar to StreamEnrollment, but does not have the same
        /// time limits or model type restrictions.
        /// Biometric model types are not supported by this function.
        /// This endpoint cannot be used to establish device trust.
        /// </summary>
        /// <param name="audioConfig">the audio-specifc configuration. Currently only LIN16 at 16000Hz is supported.</param>
        /// <param name="description">a description of this enrollment. Useful if a user could have multiple enrollments, as it helps differentiate between them.</param>
        /// <param name="userId">the unique userId for this enrollment.</param>
        /// <param name="modelName">the exact name of the model you intend to enroll into. This model name can be retrieved from the getModels() call.</param>
        /// <param name="isLivenessEnabled">indicates if liveness is enabled for this request</param>
        /// <returns>a bidirectional stream where CreateEnrolledEventRequests can be passed to the cloud and CreateEnrollmentResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<CreateEnrolledEventRequest, CreateEnrollmentResponse>> StreamCreateEnrolledEvent(
            AudioConfig audioConfig, string description, string userId, string modelName)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<CreateEnrolledEventRequest, CreateEnrollmentResponse> enrollmentStream = this.AudioEventsClient.CreateEnrolledEvent(metadata);

            CreateEnrollmentEventConfig config = new CreateEnrollmentEventConfig
            {
                Audio = audioConfig,
                Description = description,
                UserId = userId,
                ModelName = modelName,
            };

            CreateEnrolledEventRequest request = new CreateEnrolledEventRequest { Config = config };

            await enrollmentStream.RequestStream.WriteAsync(request);
            return enrollmentStream;
        }

        /// <summary>
        /// Validate an existing event enrollment in Sensory Cloud.
        /// This method is similar to Authenticate, but does not have the same
        /// time limits or model type restrictions. Additionally, the server will
        /// never close the stream, and thus a client may validate an enrolled sound
        /// as many times as they'd like.
        /// Any model types are supported by this function.
        /// This endpoint cannot be used to establish device trust.
        /// </summary>
        /// <param name="audioConfig">the audio-specifc configuration. Currently only LIN16 at 16000Hz is supported.</param>
        /// <param name="enrollmentId">the ID assocaited with the enrollment</param>
        /// <param name="sensitivity">the sensitivity of the recognition engine. Defaults to medium.</param>
        /// <returns>a bidirectional stream where ValidateEnrolledEventRequests can be passed to the cloud and ValidateEnrolledEventResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<ValidateEnrolledEventRequest, ValidateEnrolledEventResponse>> StreamValidateEnrolledEvent(
            AudioConfig audioConfig, string enrollmentId, ThresholdSensitivity sensitivity = ThresholdSensitivity.Medium)
        {
            ValidateEnrolledEventConfig config = new ValidateEnrolledEventConfig
            {
                Audio = audioConfig,
                EnrollmentId = enrollmentId,
                Sensitivity = sensitivity,
            };

            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<ValidateEnrolledEventRequest, ValidateEnrolledEventResponse> authenticationStream = this.AudioEventsClient.ValidateEnrolledEvent(metadata);

            ValidateEnrolledEventRequest request = new ValidateEnrolledEventRequest { Config = config };

            await authenticationStream.RequestStream.WriteAsync(request);
            return authenticationStream;
        }

        /// <summary>
        /// Validate an existing groupd of events in Sensory Cloud.
        /// This method is similar to GroupAuthenticate, but does not have the same
        /// time limits or model type restrictions. Additionally, the server will
        /// never close the stream, and thus a client may validate an enrolled group
        /// as many times as they'd like.
        /// Any model types are supported by this function.
        /// This endpoint cannot be used to establish device trust.
        /// </summary>
        /// <param name="audioConfig">the audio-specifc configuration. Currently only LIN16 at 16000Hz is supported.</param>
        /// <param name="enrollmentGroupId">the ID assocaited with the enrollment group</param>
        /// <param name="sensitivity">the sensitivity of the recognition engine. Defaults to medium.</param>
        /// <returns>a bidirectional stream where ValidateEnrolledEventRequests can be passed to the cloud and ValidateEnrolledEventResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<ValidateEnrolledEventRequest, ValidateEnrolledEventResponse>> StreamGroupValidateEnrolledEvent(
            AudioConfig audioConfig, string enrollmentGroupId, ThresholdSensitivity sensitivity = ThresholdSensitivity.Medium)
        {
            ValidateEnrolledEventConfig config = new ValidateEnrolledEventConfig
            {
                Audio = audioConfig,
                EnrollmentGroupId = enrollmentGroupId,
                Sensitivity = sensitivity,
            };

            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<ValidateEnrolledEventRequest, ValidateEnrolledEventResponse> authenticationStream = this.AudioEventsClient.ValidateEnrolledEvent(metadata);

            ValidateEnrolledEventRequest request = new ValidateEnrolledEventRequest { Config = config };

            await authenticationStream.RequestStream.WriteAsync(request);
            return authenticationStream;
        }

        /// <summary>
        /// Stream audio to Sensory Cloud in order to transcribe spoken words
        /// </summary>
        /// <param name="audioConfig">the audio-specifc configuration. Currently only LIN16 at 16000Hz is supported.</param>
        /// <param name="userId">the unique userId for the user requesting this event</param>
        /// <param name="modelName">the exact name of the model you intend use for transcription. This model name can be retrieved from the getModels() call.</param>
        /// <param name="transcribeConfig">optional wakeword model. This model name can be retrieved from the getModels() call.</param>
        /// <returns>a bidirectional stream where TranscribeRequests can be passed to the cloud and TranscribeResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<TranscribeRequest, TranscribeResponse>> StreamTranscription(TranscribeConfig config)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<TranscribeRequest, TranscribeResponse> transcriptionStream = this.AudioTranscriptionsClient.Transcribe(metadata);

            TranscribeRequest request = new TranscribeRequest { Config = config };

            await transcriptionStream.RequestStream.WriteAsync(request);
            return transcriptionStream;
        }

        /// <summary>
        /// Stream audio to Sensory Cloud in order to authenticate audio
        /// </summary>
        /// <param name="config">the specifc configuration.</param>
        /// <returns>a bidirectional stream where AuthenticateRequest can be passed to the cloud and AuthenticateResponse are passed back</returns>
        private async Task<AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse>> StreamAuthentication(AuthenticateConfig config)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse> authenticationStream = this.AudioBiometricsClient.Authenticate(metadata);

            AuthenticateRequest request = new AuthenticateRequest { Config = config };

            await authenticationStream.RequestStream.WriteAsync(request);
            return authenticationStream;
        }
    }
}
