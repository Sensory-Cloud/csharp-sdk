using System;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Sensory.Api.Common;
using Sensory.Api.V1.Audio;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using SensoryCloud.Src.TokenManager;

using NUnit.Framework;

using System.Collections.Generic;

namespace Test.Examples
{
    [TestFixture]
    public static class SensorsCallExample
    {
        // First Run:
        // Pass 0 args on your very first run to register an OAuth client
        // Ensure you update `enrollCredential` with the Sensory-provided credential before you run.
        // In your first run, look for a console output `Successfully registered OAuth client with clientId: {client.ClientId} and secret {client.ClientSecret}`
        // Save the clientId and secret for subsequent runs.
        //
        // Any Other Run Run:
        // Once you have OAuth credentials, you must pass three args to this function
        // arg0 - filePath to the 16000 wav file
        // arg1 - clientId obtained from step 1
        // arg2 - clientSecret obtained from step 2
        [Test]
        public static void TestMain()
        {
            // Should be replaced with string[] args 1, 2 and 3
            var filePath = "/Users/bryanmcgrane/Downloads/Sneeze.wav";
            var clientId = "a2b3221c-f441-4685-9dd7-7746f43dd725";
            var clientSecret = "yCq41TIEiisSkan1";

            // First run
            //TestMainAsync(new string[] {}).GetAwaiter().GetResult();

            // Any run thereafter
            TestMainAsync(new string[] { filePath, clientId, clientSecret }).GetAwaiter().GetResult();
        }

        public static async Task TestMainAsync(string[] args)
        {
            Console.WriteLine($"Starting SensorsCallExample");
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "8fb58cb8-c0aa-472c-800f-e14c0e3e8528";
            string deviceId = "globally-unique-hardware-identifier";

            // Configuration specific to your tenant
            Config config = new Config("sensorscall.cloud.sensory.com", sensoryTenantId, deviceId).Connect();

            // Get Server Health
            HealthService healthService = new HealthService(config);
            ServerHealthResponse serverHealth = healthService.GetHealth();
            Console.WriteLine($"Server health determined: ${serverHealth}");

            // Create a secure crendetial manager and OAuth service
            SecureCredentialStoreExample credentialStore = new SecureCredentialStoreExample();
            IOauthService oAuthService = new OauthService(config, credentialStore);

            // The very first time you reach out to the cloud, you must register your machine
            if (args.Length == 0)
            {
                OauthClient client = oAuthService.GenerateCredentials();
                credentialStore.ClientId = client.ClientId;
                credentialStore.ClientSecret = client.ClientSecret;

                string deviceName = "A friendly device name";
                // TODO: set this value
                string enrollCredential = "REPLACE_ME_WITH_CRENDENTIAL_FROM_SENSORY";

                // This call should only be made once per unique device ID
                oAuthService.Register(deviceName, enrollCredential);
                Console.WriteLine($"Successfully registered OAuth client with clientId: {client.ClientId} and secret {client.ClientSecret}");
                return;
            }
            else if (args.Length == 3)
            {
                credentialStore.ClientId = args[1];
                credentialStore.ClientSecret = args[2];
            }
            else
            {
                throw new ArgumentException($"An invalid number of arguments were passed. Expected 1 or 3, recieved {args.Length}");
            }

            var whoAmI = oAuthService.GetWhoAmI();
            Console.WriteLine($"OAuth Token successfully obtained for device: {whoAmI}");

            ITokenManager tokenManager = new TokenManager(oAuthService);

            // Configure audio service
            AudioService audioService = new AudioService(config, tokenManager);
            AudioConfig audioConfig = new AudioConfig
            {
                Encoding = AudioConfig.Types.AudioEncoding.Linear16,
                AudioChannelCount = 1,
                SampleRateHertz = 16000,
                LanguageCode = "en-US",
            };

            // Print out available models
            var models = audioService.GetModels();
            Console.WriteLine($"Found {models.Models.Count} model(s): {models.Models}");

            // Set basic enrollment information
            // TODO: set to userId in your system who is making the request
            string userId = "sensorscall";
            string modelName = "sound-16kHz-combined-sensorscall.trg";

            // Stream is of type AsyncDuplexStreamingCall<ValidateEventRequest, ValidateEventResponse>
            var stream = await audioService.StreamEvent(audioConfig, userId, modelName);

            var results = new List<ValidateEventResponse>();

            // Start background task to receive messages from the cloud
            var readTask = Task.Run(async () =>
            {
                try
                {
                    while (await stream.ResponseStream.MoveNext())
                    {
                        // Response contains information about the recognition.
                        ValidateEventResponse response = stream.ResponseStream.Current;
                        //eventRecognized = response.Success;
                        if (response.Success)
                        {
                            Console.WriteLine($"Event recognized {response}");
                            results.Add(response);
                        }
                    }
                }
                catch (RpcException ex)
                {
                    // Do something with the error - generally this is due to the server closing the stream
                    Console.WriteLine($"A server error occurred: {ex}");
                }
            });

            // load wav file and cut out wav file header
            byte[] wavFile = File.ReadAllBytes(args[0])[44..];
            var bytesPerSample = 2;
            var chunkTimeSeconds = 0.100; // 100ms
            var bytesPerChunk = Convert.ToInt32(bytesPerSample * audioConfig.SampleRateHertz * chunkTimeSeconds);

            Console.WriteLine($"Uploading {args[0]} ({wavFile.Length} bytes) for recognition");

            // Start blocking while streaming up audio data in 100ms chunks.
            for (int index = 0; index < wavFile.Length; index += bytesPerChunk)
            {
                int length = bytesPerChunk;
                if (index + length > wavFile.Length - 1)
                {
                    length = wavFile.Length - index - 1;
                }

                // Create audio message
                // Could be more efficient than using a copy
                var audioChunk = ByteString.CopyFrom(new ArraySegment<byte>(wavFile, index, length));
                var message = new ValidateEventRequest { AudioContent = audioChunk };

                try
                {
                    await stream.RequestStream.WriteAsync(message);
                }
                catch (RpcException ex)
                {
                    Console.WriteLine($"A client error occurred: {ex}");
                }
            }

            await stream.RequestStream.CompleteAsync();
            await readTask;


            Console.WriteLine($"Sounds recognized:");
            results.ForEach((result) =>
            {
                Console.WriteLine($"{result}");
            });
        }
    }
}
