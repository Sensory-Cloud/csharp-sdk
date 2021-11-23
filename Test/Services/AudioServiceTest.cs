using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Testing;
using Moq;
using NUnit.Framework;
using Sensory.Api.Common;
using Sensory.Api.V1.Audio;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using SensoryCloud.Src.TokenManager;

namespace Test.Services
{
    [TestFixture]
    public class AudioServiceTest
    {

        [Test]
        public void TestGetModels()
        {

            var tokenManager = new MockTokenManager();
            var audioModelsClient = new Mock<AudioModels.AudioModelsClient>();
            var audioBiometricsClient = new Mock<AudioBiometrics.AudioBiometricsClient>();
            var audioEventsClient = new Mock<AudioEvents.AudioEventsClient>();
            var audioTranscriptionsClient = new Mock<AudioTranscriptions.AudioTranscriptionsClient>();

            var response = new GetModelsResponse();
            response.Models.Add(new AudioModel { IsEnrollable = true, ModelType = ModelType.FaceBiometric, Name = "model-name" });

            audioModelsClient.Setup(client => client.GetModels(It.IsAny<GetModelsRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            var audioService = new MockAudioService(
                new Config("doesnt-matter", "doesnt-matter"),
                tokenManager,
                audioModelsClient.Object,
                audioBiometricsClient.Object,
                audioEventsClient.Object,
                audioTranscriptionsClient.Object
            );

            var modelsResponse = audioService.GetModels();
            Assert.AreSame(modelsResponse, response);
        }

        [Test]
        public async void TestStreamEnrollment()
        {
            var tokenManager = new MockTokenManager();
            var audioModelsClient = new Mock<AudioModels.AudioModelsClient>();
            var audioBiometricsClient = new Mock<AudioBiometrics.AudioBiometricsClient>();
            var audioEventsClient = new Mock<AudioEvents.AudioEventsClient>();
            var audioTranscriptionsClient = new Mock<AudioTranscriptions.AudioTranscriptionsClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<CreateEnrollmentRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<CreateEnrollmentResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            audioBiometricsClient.Setup(m => m.CreateEnrollment(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var audioService = new MockAudioService(
                new Config("doesnt-matter", "doesnt-matter"),
                tokenManager,
                audioModelsClient.Object,
                audioBiometricsClient.Object,
                audioEventsClient.Object,
                audioTranscriptionsClient.Object
            );

            var description = "my description";
            var userId = "1234";
            var modelName = "my-model";

            var audioConfig = new AudioConfig { AudioChannelCount = 1, Encoding = AudioConfig.Types.AudioEncoding.Linear16, LanguageCode = "en-US", SampleRateHertz = 16000 };
            var modelsResponse = await audioService.StreamEnrollment(audioConfig, description, userId, modelName, false);

            //TODO: Stopped here. Trying to mock audio.
        }
    }

    public class MockAudioService : AudioService
    {
        public MockAudioService(
            Config config,
            ITokenManager tokenManager,
            AudioModels.AudioModelsClient audioModelsClient,
            AudioBiometrics.AudioBiometricsClient audioBiometricsClient,
            AudioEvents.AudioEventsClient audioEventsClient,
            AudioTranscriptions.AudioTranscriptionsClient audioTranscriptionsClient) :
            base(config, tokenManager, audioModelsClient, audioBiometricsClient, audioEventsClient, audioTranscriptionsClient)
        {

        }
    }

}
