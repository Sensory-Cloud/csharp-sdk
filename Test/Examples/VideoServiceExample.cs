using System;
using System.Threading.Tasks;
using Grpc.Core;
using Sensory.Api.V1.Video;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using SensoryCloud.Src.TokenManager;

namespace Test.Examples
{
    public class VideoServiceExample
    {
        public static VideoService GetVideoService()
        {
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
            string deviceId = "a-hardware-identifier-unique-to-your-device";

            // Configuration specific to your tenant
            Config config = new Config("https://your-inference-server.com", sensoryTenantId, deviceId);

            ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
            IOauthService oauthService = new OauthService(config, credentialStore);
            ITokenManager tokenManager = new TokenManager(oauthService);

            return new VideoService(config, tokenManager);
        }

        public static void GetVideoModels()
        {
            VideoService videoService = GetVideoService();
            GetModelsResponse videoModels = videoService.GetModels();
        }

        public static async void EnrollVideo()
        {
            VideoService videoService = GetVideoService();

            // Set basic enrollment information
            string enrollmentDescription = "My Enrollment";
            string userId = "72f286b8-173f-436a-8869-6f7887789ee9";
            string deviceId = "f483b33a-5d83-4960-8e1c-b854992f47c9"; // device ID used in OAuth registration
            string modelName = "wakeword-16kHz-open_sesame.ubm";
            bool isLivenessEnabled = false;

            // Stream is of type AsyncDuplexStreamingCall<CreateEnrollmentRequest, CreateEnrollmentResponse>
            var stream = await videoService.StreamEnrollment(
                enrollmentDescription,
                userId,
                deviceId,
                modelName,
                isLivenessEnabled
                );

            // EnrollmentId if enrollment is successful
            string enrollmentId = null;

            // Start blocking while streaming up image data.
            while (String.IsNullOrEmpty(enrollmentId))
            {
                // Populate with real video data. This example creates an empty bytestring.
                var lin16VideoData = Google.Protobuf.ByteString.Empty;
                var message = new CreateEnrollmentRequest { ImageContent = lin16VideoData };

                try
                {
                    await stream.RequestStream.WriteAsync(message);
                }
                catch (RpcException ex)
                {
                    // Do something with the error. An error
                }

                // Wait for the response from the server
                try
                {
                    await stream.ResponseStream.MoveNext();

                    // Response contains information about the enrollment status.
                    CreateEnrollmentResponse response = stream.ResponseStream.Current;
                    enrollmentId = response.EnrollmentId;
                }
                catch (RpcException ex)
                {
                    // Do something with the error - generally this is due to the server closing the stream
                }
            }

            await stream.RequestStream.CompleteAsync();
        }

        public static async void AuthenticateVideo()
        {
            VideoService videoService = GetVideoService();

            // Set basic enrollment information
            string enrollmentId = "72f286b8-173f-436a-8869-6f7887789ee9";
            bool isLivenessEnabled = false;

            // Stream is of type AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse>
            var stream = await videoService.StreamAuthentication(
                enrollmentId,
                isLivenessEnabled
                );

            // Indicates if the authentication is successful
            bool authenticateSuccess = false;

            // Start blocking while streaming up image data.
            while (String.IsNullOrEmpty(enrollmentId))
            {
                // Populate with real video data. This example creates an empty bytestring.
                var lin16VideoData = Google.Protobuf.ByteString.Empty;
                var message = new AuthenticateRequest { ImageContent = lin16VideoData };

                try
                {
                    await stream.RequestStream.WriteAsync(message);
                }
                catch (RpcException ex)
                {
                    // Do something with the error. An error
                }

                // Wait for the response from the server
                try
                {
                    await stream.ResponseStream.MoveNext();

                    // Response contains information about the enrollment status.
                    AuthenticateResponse response = stream.ResponseStream.Current;
                    authenticateSuccess = response.Success;
                }
                catch (RpcException ex)
                {
                    // Do something with the error - generally this is due to the server closing the stream
                }
            }

            await stream.RequestStream.CompleteAsync();
        }

        public static async void VideoLiveness()
        {
            VideoService videoService = GetVideoService();

            // Set basic enrollment information
            string userId = "72f286b8-173f-436a-8869-6f7887789ee9";
            string modelName = "sound-16kHz-door_bell.trg";

            // Stream is of type AsyncDuplexStreamingCall<ValidateRecognitionRequest, LivenessRecognitionResponse>
            var stream = await videoService.StreamLivenessRecognition(
                userId,
                modelName
                );

            // Indicates if the event is recognized
            bool isAlive = false;

            // Start blocking while streaming up image data.
            // Loop until user is determined to be alive. Looping behavior is entirely up to your application logic. This is a simple example of one use case.
            while (!isAlive)
            {
                // Populate with real video data. This example creates an empty bytestring.
                var lin16VideoData = Google.Protobuf.ByteString.Empty;
                var message = new ValidateRecognitionRequest { ImageContent = lin16VideoData };

                try
                {
                    await stream.RequestStream.WriteAsync(message);
                }
                catch (RpcException ex)
                {
                    // Do something with the error. An error
                }

                // Wait for the response from the server
                try
                {
                    await stream.ResponseStream.MoveNext();

                    // Response contains information about the enrollment status.
                    LivenessRecognitionResponse response = stream.ResponseStream.Current;
                    isAlive = response.IsAlive;
                }
                catch (RpcException ex)
                {
                    // Do something with the error - generally this is due to the server closing the stream
                }
            }

            await stream.RequestStream.CompleteAsync();
        }
    }
}
