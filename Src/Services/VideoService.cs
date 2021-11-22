﻿using System;
using System.Threading.Tasks;
using Grpc.Core;
using Sensory.Api.V1.Video;
using SensoryCloud.Src.TokenManager;

namespace SensoryCloud.Src.Services
{
    public class VideoService
    {
        private readonly Config Config;
        private readonly ITokenManager TokenManager;
        private readonly VideoModels.VideoModelsClient VideoModelsClient;
        private readonly VideoBiometrics.VideoBiometricsClient VideoBiometricsClient;
        private readonly VideoRecognition.VideoRecognitionClient VideoRecognitionClient;

        public VideoService(Config config, ITokenManager tokenManager)
        {
            this.Config = config;
            this.TokenManager = tokenManager;
            this.VideoModelsClient = new VideoModels.VideoModelsClient(config.GetChannel());
            this.VideoBiometricsClient = new VideoBiometrics.VideoBiometricsClient(config.GetChannel());
            this.VideoRecognitionClient = new VideoRecognition.VideoRecognitionClient(config.GetChannel());
        }

        /// <summary>
        /// Fetch all the video models supported by your instance of Sensory Cloud.
        /// </summary>
        /// <returns>the supported models</returns>
        public GetModelsResponse GetModels()
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            GetModelsRequest request = new GetModelsRequest();

            return this.VideoModelsClient.GetModels(request, metadata);
        }

        /// <summary>
        /// Stream images to Sensory Cloud as a means for user enrollment.
        /// </summary>
        /// <param name="description">a description of this enrollment. Useful if a user could have multiple enrollments, as it helps differentiate between them.</param>
        /// <param name="userId">the unique userId for this enrollment.</param>
        /// <param name="modelName">the exact name of the model you intend to enroll into. This model name can be retrieved from the getModels() call.</param>
        /// <param name="isLivenessEnabled">indicates if liveness is enabled for this request</param>
        /// <param name="threshold">the liveness threshold (if liveness is enabled). Defaults to HIGH.</param>
        /// <returns>a bidirectional stream where CreateEnrollmentRequests can be passed to the cloud and CreateEnrollmentResponses are passed back</returns>
        public async Task<AsyncDuplexStreamingCall<CreateEnrollmentRequest, CreateEnrollmentResponse>> StreamEnrollment(
            string description, string userId, string modelName, bool isLivenessEnabled, RecognitionThreshold threshold = RecognitionThreshold.High)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<CreateEnrollmentRequest, CreateEnrollmentResponse> enrollmentStream = this.VideoBiometricsClient.CreateEnrollment(metadata);

            CreateEnrollmentConfig config = new CreateEnrollmentConfig
            {
                Description = description,
                UserId = userId,
                ModelName = modelName,
                IsLivenessEnabled = isLivenessEnabled,
                LivenessThreshold = threshold,
            };

            CreateEnrollmentRequest request = new CreateEnrollmentRequest { Config = config };

            await enrollmentStream.RequestStream.WriteAsync(request);
            return enrollmentStream;
        }

        /// <summary>
        /// Stream images to sensory cloud in order to authenticate a user against an existing enrollment.
        /// </summary>
        /// <param name="enrollmentId">the unique enrollment ID</param>
        /// <param name="isLivenessEnabled">indicates if liveness is enabled for this request.</param>
        /// <param name="threshold">the liveness threshold (if liveness is enabled) Defaults to HIGH.</param>
        /// <returns>a bidirectional stream where AuthenticateRequests can be passed to the cloud and AuthenticateResponses are passed back.</returns>
        public async Task<AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse>> StreamAuthentication(
            string enrollmentId, bool isLivenessEnabled, RecognitionThreshold threshold = RecognitionThreshold.High)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse> authenticateStream = this.VideoBiometricsClient.Authenticate(metadata);

            AuthenticateConfig config = new AuthenticateConfig
            {
                EnrollmentId = enrollmentId,
                IsLivenessEnabled = isLivenessEnabled,
                LivenessThreshold = threshold,
            };

            AuthenticateRequest request = new AuthenticateRequest { Config = config };

            await authenticateStream.RequestStream.WriteAsync(request);
            return authenticateStream;
        }

        /// <summary>
        /// Stream images to Sensory Cloud in order to recognize "liveness" of a particular image.
        /// </summary>
        /// <param name="userId">The unique user identifier making the request.</param>
        /// <param name="modelName">the exact name of the model you intend to use for recognition. This model name can be retrieved from the getModels() call.</param>
        /// <param name="threshold">the threshold. Defaults to HIGH.</param>
        /// <returns>a bidirectional stream where ValidateRecognitionRequests can be passed to the cloud and LivenessRecognitionResponses are passed back.</returns>
        public async Task<AsyncDuplexStreamingCall<ValidateRecognitionRequest, LivenessRecognitionResponse>> StreamLivenessRecognition(
            string userId, string modelName, RecognitionThreshold threshold = RecognitionThreshold.High)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AsyncDuplexStreamingCall<ValidateRecognitionRequest, LivenessRecognitionResponse> authenticateStream = this.VideoRecognitionClient.ValidateLiveness(metadata);

            ValidateRecognitionConfig config = new ValidateRecognitionConfig
            {
                UserId = userId,
                ModelName = modelName,
                Threshold = threshold,
            };

            ValidateRecognitionRequest request = new ValidateRecognitionRequest { Config = config };

            await authenticateStream.RequestStream.WriteAsync(request);
            return authenticateStream;
        }
    }
}