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

namespace Test
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
            response.Models.Add(new AudioModel { IsEnrollable = true, ModelType = ModelType.SoundEventFixed, Name = "model-name" });

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
        public async Task TestStreamEnrollment()
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
            var isLivenessEnabled = false;
            var userId = "1234";
            var modelName = "my-model";

            var audioConfig = new AudioConfig { AudioChannelCount = 1, Encoding = AudioConfig.Types.AudioEncoding.Linear16, LanguageCode = "en-US", SampleRateHertz = 16000 };
            var enrollmentStream = await audioService.StreamEnrollment(audioConfig, description, userId, modelName, isLivenessEnabled);
            Assert.IsNotNull(enrollmentStream, "enrollment stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (CreateEnrollmentRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreSame(configMessage.Config.Audio, audioConfig, "audio config shoud match what was passed in");
            Assert.AreEqual(configMessage.Config.Description, description, "description should match what was passed in");
            Assert.AreEqual(configMessage.Config.UserId, userId, "userId should match what was passed in");
            Assert.AreEqual(configMessage.Config.ModelName, modelName, "modelName should match what was passed in");
            Assert.AreEqual(configMessage.Config.IsLivenessEnabled, isLivenessEnabled, "isLivenessEnabled should match what was passed in");
        }


        [Test]
        public async Task TestStreamAuthentication()
        {
            var tokenManager = new MockTokenManager();
            var audioModelsClient = new Mock<AudioModels.AudioModelsClient>();
            var audioBiometricsClient = new Mock<AudioBiometrics.AudioBiometricsClient>();
            var audioEventsClient = new Mock<AudioEvents.AudioEventsClient>();
            var audioTranscriptionsClient = new Mock<AudioTranscriptions.AudioTranscriptionsClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<AuthenticateRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<AuthenticateResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            audioBiometricsClient.Setup(m => m.Authenticate(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var audioService = new MockAudioService(
                new Config("doesnt-matter", "doesnt-matter"),
                tokenManager,
                audioModelsClient.Object,
                audioBiometricsClient.Object,
                audioEventsClient.Object,
                audioTranscriptionsClient.Object
            );

            var enrollmentId = "enrollmentId";
            var isLivenessEnabled = true;
            var sensitivity = ThresholdSensitivity.Low;
            var security = AuthenticateConfig.Types.ThresholdSecurity.Low;

            var audioConfig = new AudioConfig { AudioChannelCount = 1, Encoding = AudioConfig.Types.AudioEncoding.Linear16, LanguageCode = "en-US", SampleRateHertz = 16000 };
            var enrollmentStream = await audioService.StreamAuthentication(audioConfig, enrollmentId, isLivenessEnabled, sensitivity, security);
            Assert.IsNotNull(enrollmentStream, "authentication stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (AuthenticateRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreSame(configMessage.Config.Audio, audioConfig, "audio config shoud match what was passed in");
            Assert.AreEqual(configMessage.Config.EnrollmentId, enrollmentId, "enrollmentId should match what was passed in");
            Assert.IsEmpty(configMessage.Config.EnrollmentGroupId, "enrollmentGroupId should be empty");
            Assert.AreEqual(configMessage.Config.IsLivenessEnabled, isLivenessEnabled, "isLivenessEnabled should match what was passed in");
            Assert.AreEqual(configMessage.Config.Sensitivity, sensitivity, "sensitivity should match what was passed in");
            Assert.AreEqual(configMessage.Config.Security, security, "security should match what was passed in");
        }

        [Test]
        public async Task TestStreamGroupAuthentication()
        {
            var tokenManager = new MockTokenManager();
            var audioModelsClient = new Mock<AudioModels.AudioModelsClient>();
            var audioBiometricsClient = new Mock<AudioBiometrics.AudioBiometricsClient>();
            var audioEventsClient = new Mock<AudioEvents.AudioEventsClient>();
            var audioTranscriptionsClient = new Mock<AudioTranscriptions.AudioTranscriptionsClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<AuthenticateRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<AuthenticateResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            audioBiometricsClient.Setup(m => m.Authenticate(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var audioService = new MockAudioService(
                new Config("doesnt-matter", "doesnt-matter"),
                tokenManager,
                audioModelsClient.Object,
                audioBiometricsClient.Object,
                audioEventsClient.Object,
                audioTranscriptionsClient.Object
            );

            var enrollmentGroupId = "enrollmentId";
            var isLivenessEnabled = true;
            var sensitivity = ThresholdSensitivity.Highest;
            var security = AuthenticateConfig.Types.ThresholdSecurity.High;

            var audioConfig = new AudioConfig { AudioChannelCount = 1, Encoding = AudioConfig.Types.AudioEncoding.Linear16, LanguageCode = "en-US", SampleRateHertz = 16000 };
            var enrollmentStream = await audioService.StreamGroupAuthentication(audioConfig, enrollmentGroupId, isLivenessEnabled, sensitivity, security);
            Assert.IsNotNull(enrollmentStream, "authentication stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (AuthenticateRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreSame(configMessage.Config.Audio, audioConfig, "audio config shoud match what was passed in");
            Assert.IsEmpty(configMessage.Config.EnrollmentId, "enrollentId should be empty");
            Assert.AreEqual(configMessage.Config.EnrollmentGroupId, enrollmentGroupId, "enrollmentGroupId should match what was passed in");
            Assert.AreEqual(configMessage.Config.IsLivenessEnabled, isLivenessEnabled, "isLivenessEnabled should match what was passed in");
            Assert.AreEqual(configMessage.Config.Sensitivity, sensitivity, "sensitivity should match what was passed in");
            Assert.AreEqual(configMessage.Config.Security, security, "security should match what was passed in");
        }

        [Test]
        public async Task TestStreamEvent()
        {
            var tokenManager = new MockTokenManager();
            var audioModelsClient = new Mock<AudioModels.AudioModelsClient>();
            var audioBiometricsClient = new Mock<AudioBiometrics.AudioBiometricsClient>();
            var audioEventsClient = new Mock<AudioEvents.AudioEventsClient>();
            var audioTranscriptionsClient = new Mock<AudioTranscriptions.AudioTranscriptionsClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<ValidateEventRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<ValidateEventResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            audioEventsClient.Setup(m => m.ValidateEvent(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var audioService = new MockAudioService(
                new Config("doesnt-matter", "doesnt-matter"),
                tokenManager,
                audioModelsClient.Object,
                audioBiometricsClient.Object,
                audioEventsClient.Object,
                audioTranscriptionsClient.Object
            );

            var userId = "userId";
            var modelName = "my-model";
            var sensitivity = ThresholdSensitivity.Highest;

            var audioConfig = new AudioConfig { AudioChannelCount = 1, Encoding = AudioConfig.Types.AudioEncoding.Linear16, LanguageCode = "en-US", SampleRateHertz = 16000 };
            var enrollmentStream = await audioService.StreamEvent(audioConfig, userId, modelName, sensitivity);
            Assert.IsNotNull(enrollmentStream, "event stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (ValidateEventRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreSame(configMessage.Config.Audio, audioConfig, "audio config shoud match what was passed in");
            Assert.AreEqual(configMessage.Config.UserId, userId, "userId should match what was passed in");
            Assert.AreEqual(configMessage.Config.ModelName, modelName, "modelName should match what was passed in");
            Assert.AreEqual(configMessage.Config.Sensitivity, sensitivity, "sensitivity should match what was passed in");
        }

        [Test]
        public async Task TestStreamTranscription()
        {
            var tokenManager = new MockTokenManager();
            var audioModelsClient = new Mock<AudioModels.AudioModelsClient>();
            var audioBiometricsClient = new Mock<AudioBiometrics.AudioBiometricsClient>();
            var audioEventsClient = new Mock<AudioEvents.AudioEventsClient>();
            var audioTranscriptionsClient = new Mock<AudioTranscriptions.AudioTranscriptionsClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<TranscribeRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<TranscribeResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            audioTranscriptionsClient.Setup(m => m.Transcribe(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var audioService = new MockAudioService(
                new Config("doesnt-matter", "doesnt-matter"),
                tokenManager,
                audioModelsClient.Object,
                audioBiometricsClient.Object,
                audioEventsClient.Object,
                audioTranscriptionsClient.Object
            );

            var userId = "userId";
            var modelName = "my-model";

            var audioConfig = new AudioConfig { AudioChannelCount = 1, Encoding = AudioConfig.Types.AudioEncoding.Linear16, LanguageCode = "en-US", SampleRateHertz = 16000 };
            var enrollmentStream = await audioService.StreamTranscription(audioConfig, userId, modelName);
            Assert.IsNotNull(enrollmentStream, "transcription stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (TranscribeRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreSame(configMessage.Config.Audio, audioConfig, "audio config shoud match what was passed in");
            Assert.AreEqual(configMessage.Config.UserId, userId, "userId should match what was passed in");
            Assert.AreEqual(configMessage.Config.ModelName, modelName, "modelName should match what was passed in");
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
