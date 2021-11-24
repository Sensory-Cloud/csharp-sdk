using System.Threading.Tasks;
using Grpc.Core;
using Sensory.Api.V1.Audio;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using SensoryCloud.Src.TokenManager;

namespace Test.Examples
{
    public static class AudioServiceExample
    {
        public static AudioService GetAudioService()
        {
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";

            // Configuration specific to your tenant
            Config config = new Config("https://your-inference-server.com", sensoryTenantId);

            ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
            IOauthService oAuthService = new OauthService(config, credentialStore);
            ITokenManager tokenManager = new TokenManager(oAuthService);

            return new AudioService(config, tokenManager);
        }

        public static void GetAudioModels()
        {
            AudioService audioService = GetAudioService();
            GetModelsResponse audioModels = audioService.GetModels();
        }

        public static async void EnrollAudio()
        {
            AudioService audioService = GetAudioService();
            AudioConfig audioConfig = new AudioConfig
            {
                Encoding = AudioConfig.Types.AudioEncoding.Linear16,
                AudioChannelCount = 1,
                SampleRateHertz = 16000,
                LanguageCode = "en-US",
            };

            // Set basic enrollment information
            string enrollmentDescription = "My Enrollment";
            string userId = "72f286b8-173f-436a-8869-6f7887789ee9";
            string modelName = "wakeword-16kHz-open_sesame.ubm";
            bool isLivenessEnabled = false;

            // Stream is of type AsyncDuplexStreamingCall<CreateEnrollmentRequest, CreateEnrollmentResponse>
            var stream = await audioService.StreamEnrollment(
                audioConfig,
                enrollmentDescription,
                userId,
                modelName,
                isLivenessEnabled
                );

            // EnrollmentId if enrollment is successful
            string enrollmentId = null;

            // Start background task to receive messages from the cloud
            var readTask = Task.Run(async () =>
            {
                try
                {
                    while (await stream.ResponseStream.MoveNext())
                    {
                        // Response contains information about the enrollment status.
                        // For enrollments with liveness, there are two additional fields that are populated.
                        // * ModelPrompt - indicaites what the user should say in order to proceed with the enrollment.
                        // * SectionPercentComplete - indicates the percentage of the current ModelPrompt that has been spoken.
                        CreateEnrollmentResponse response = stream.ResponseStream.Current;
                        enrollmentId = response.EnrollmentId;
                    }
                }
                catch (RpcException ex)
                {
                    // Do something with the error - generally this is due to the server closing the stream
                }

            });

            // Start blocking while streaming up audio data. If you have the entire audio file already you can ignore the while loop.
            while (enrollmentId == null)
            {
                // Populate with real audio data. This example creates an empty bytestring.
                var lin16AudioData = Google.Protobuf.ByteString.Empty;
                var message = new CreateEnrollmentRequest { AudioContent = lin16AudioData };

                try
                {
                    await stream.RequestStream.WriteAsync(message);
                } catch (RpcException ex)
                {
                    // Do something with the error. An error
                }
            }

            await stream.RequestStream.CompleteAsync();
            await readTask;
        }

        public static async void AuthenticateAudio()
        {
            AudioService audioService = GetAudioService();
            AudioConfig audioConfig = new AudioConfig
            {
                Encoding = AudioConfig.Types.AudioEncoding.Linear16,
                AudioChannelCount = 1,
                SampleRateHertz = 16000,
                LanguageCode = "en-US",
            };

            // Set basic enrollment information
            string enrollmentId = "72f286b8-173f-436a-8869-6f7887789ee9";
            bool isLivenessEnabled = false;

            // Stream is of type AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse>
            // You can also authenticate against an enrolment group by using the StreamGroupAuthentication method.
            var stream = await audioService.StreamAuthentication(
                audioConfig,
                enrollmentId,
                isLivenessEnabled
                );

            // Indicates if the authentication is successful
            bool authenticateSuccess = false;

            // Start background task to receive messages from the cloud
            var readTask = Task.Run(async () =>
            {
                try
                {
                    while (await stream.ResponseStream.MoveNext())
                    {
                        // Response contains information about the authentication status.
                        // For enrollments with liveness, there are two additional fields that are populated.
                        // * ModelPrompt - indicaites what the user should say in order to proceed with the authentication.
                        // * SectionPercentComplete - indicates the percentage of the current ModelPrompt that has been spoken.
                        AuthenticateResponse response = stream.ResponseStream.Current;
                        authenticateSuccess = response.Success;
                    }
                }
                catch (RpcException ex)
                {
                    // Do something with the error - generally this is due to the server closing the stream
                }

            });

            // Start blocking while streaming up audio data. If you have the entire audio file already you can ignore the while loop.
            while (!authenticateSuccess)
            {
                // Populate with real audio data. This example creates an empty bytestring.
                var lin16AudioData = Google.Protobuf.ByteString.Empty;
                var message = new AuthenticateRequest { AudioContent = lin16AudioData };

                try
                {
                    await stream.RequestStream.WriteAsync(message);
                }
                catch (RpcException ex)
                {
                    // Do something with the error
                }
            }

            await stream.RequestStream.CompleteAsync();
            await readTask;
        }

        public static async void AudioEvent()
        {
            AudioService audioService = GetAudioService();
            AudioConfig audioConfig = new AudioConfig
            {
                Encoding = AudioConfig.Types.AudioEncoding.Linear16,
                AudioChannelCount = 1,
                SampleRateHertz = 16000,
                LanguageCode = "en-US",
            };

            // Set basic enrollment information
            string userId = "72f286b8-173f-436a-8869-6f7887789ee9";
            string modelName = "sound-16kHz-door_bell.trg";

            // Stream is of type AsyncDuplexStreamingCall<ValidateEventRequest, ValidateEventResponse>
            var stream = await audioService.StreamEvent(
                audioConfig,
                userId,
                modelName
                );

            // Indicates if the event is recognized
            bool eventRecognized = false;

            // Start background task to receive messages from the cloud
            var readTask = Task.Run(async () =>
            {
                try
                {
                    while (await stream.ResponseStream.MoveNext())
                    {
                        // Response contains information about the recognition.
                        ValidateEventResponse response = stream.ResponseStream.Current;
                        eventRecognized = response.Success;
                    }
                }
                catch (RpcException ex)
                {
                    // Do something with the error - generally this is due to the server closing the stream
                }

            });

            // Start blocking while streaming up audio data. If you have the entire audio file already you can ignore the while loop.
            // Block until one event is recognized. This logic is totally up to your application.
            while (!eventRecognized)
            {
                // Populate with real audio data. This example creates an empty bytestring.
                var lin16AudioData = Google.Protobuf.ByteString.Empty;
                var message = new ValidateEventRequest { AudioContent = lin16AudioData };

                try
                {
                    await stream.RequestStream.WriteAsync(message);
                }
                catch (RpcException ex)
                {
                    // Do something with the error
                }
            }

            await stream.RequestStream.CompleteAsync();
            await readTask;
        }

        public static async void AudioTranscribe()
        {
            AudioService audioService = GetAudioService();
            AudioConfig audioConfig = new AudioConfig
            {
                Encoding = AudioConfig.Types.AudioEncoding.Linear16,
                AudioChannelCount = 1,
                SampleRateHertz = 16000,
                LanguageCode = "en-US",
            };

            // Set basic enrollment information
            string userId = "72f286b8-173f-436a-8869-6f7887789ee9";
            string modelName = "sound-16kHz-door_bell.trg";

            // Stream is of type AsyncDuplexStreamingCall<TranscribeRequest, TranscribeResponse>
            var stream = await audioService.StreamTranscription(
                audioConfig,
                userId,
                modelName
                );

            // Start background task to receive messages from the cloud
            var readTask = Task.Run(async () =>
            {
                try
                {
                    while (await stream.ResponseStream.MoveNext())
                    {
                        // Response contains the current transcription.
                        TranscribeResponse response = stream.ResponseStream.Current;
                    }
                }
                catch (RpcException ex)
                {
                    // Do something with the error - generally this is due to the server closing the stream
                }

            });

            // Start blocking while streaming up audio data. If you have the entire audio file already you can ignore the while loop.
            // Block until you are done sending audio.
            while (true)
            {
                // Populate with real audio data. This example creates an empty bytestring.
                var lin16AudioData = Google.Protobuf.ByteString.Empty;
                var message = new TranscribeRequest { AudioContent = lin16AudioData };

                try
                {
                    await stream.RequestStream.WriteAsync(message);
                }
                catch (RpcException ex)
                {
                    // Do something with the error
                }
            }

            await stream.RequestStream.CompleteAsync();
            await readTask;
        }
    }
}
