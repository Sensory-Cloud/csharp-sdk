using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Testing;
using Moq;
using NUnit.Framework;
using Sensory.Api.Common;
using Sensory.Api.V1.Video;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using SensoryCloud.Src.TokenManager;

namespace Test
{
    [TestFixture]
    public class VideoServiceTest
    {
        [Test]
        public void TestGetModels()
        {

            var tokenManager = new MockTokenManager();
            var videoModelsClient = new Mock<VideoModels.VideoModelsClient>();
            var videoBiometricsClient = new Mock<VideoBiometrics.VideoBiometricsClient>();
            var videoRecognitionsClient = new Mock<VideoRecognition.VideoRecognitionClient>();

            var response = new GetModelsResponse();
            response.Models.Add(new VideoModel { IsEnrollable = true, ModelType = ModelType.FaceBiometric, Name = "model-name" });

            videoModelsClient.Setup(client => client.GetModels(It.IsAny<GetModelsRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            var videoService = new MockVideoService(
                new Config(new SDKInitConfig { }),
                tokenManager,
                videoModelsClient.Object,
                videoBiometricsClient.Object,
                videoRecognitionsClient.Object
            );

            var modelsResponse = videoService.GetModels();
            Assert.AreSame(modelsResponse, response);
        }

        [Test]
        public async Task TestStreamEnrollment()
        {
            var tokenManager = new MockTokenManager();
            var videoModelsClient = new Mock<VideoModels.VideoModelsClient>();
            var videoBiometricsClient = new Mock<VideoBiometrics.VideoBiometricsClient>();
            var videoRecognitionsClient = new Mock<VideoRecognition.VideoRecognitionClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<CreateEnrollmentRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<CreateEnrollmentResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            videoBiometricsClient.Setup(m => m.CreateEnrollment(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var videoService = new MockVideoService(
                new Config(new SDKInitConfig { }),
                tokenManager,
                videoModelsClient.Object,
                videoBiometricsClient.Object,
                videoRecognitionsClient.Object
            );

            var description = "my description";
            var isLivenessEnabled = false;
            var numLivenessFrames = 5;
            var recognitionThreshold = RecognitionThreshold.Low;
            var userId = "1234";
            var deviceId = "1234";
            var modelName = "my-model";

            var enrollmentStream = await videoService.StreamEnrollment(description, userId, deviceId, modelName, isLivenessEnabled, recognitionThreshold, numLivenessFrames);
            Assert.IsNotNull(enrollmentStream, "enrollment stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (CreateEnrollmentRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreEqual(configMessage.Config.Description, description, "description should match what was passed in");
            Assert.AreEqual(configMessage.Config.UserId, userId, "userId should match what was passed in");
            Assert.AreEqual(configMessage.Config.ModelName, modelName, "modelName should match what was passed in");
            Assert.AreEqual(configMessage.Config.IsLivenessEnabled, isLivenessEnabled, "isLivenessEnabled should match what was passed in");
            Assert.AreEqual(configMessage.Config.LivenessThreshold, recognitionThreshold, "recognitionThreshold should match what was passed in");
            Assert.AreEqual(configMessage.Config.NumLivenessFramesRequired, numLivenessFrames, "numLivenessFrames should match what was passed in");
        }

        [Test]
        public async Task TestStreamAuthentication()
        {
            var tokenManager = new MockTokenManager();
            var videoModelsClient = new Mock<VideoModels.VideoModelsClient>();
            var videoBiometricsClient = new Mock<VideoBiometrics.VideoBiometricsClient>();
            var videoRecognitionsClient = new Mock<VideoRecognition.VideoRecognitionClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<AuthenticateRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<AuthenticateResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            videoBiometricsClient.Setup(m => m.Authenticate(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var videoService = new MockVideoService(
                new Config(new SDKInitConfig { }),
                tokenManager,
                videoModelsClient.Object,
                videoBiometricsClient.Object,
                videoRecognitionsClient.Object
            );

            var enrollmentId = "enrollmentId";
            var isLivenessEnabled = true;
            var threhsold = RecognitionThreshold.Low;

            var enrollmentStream = await videoService.StreamAuthentication(enrollmentId, isLivenessEnabled, threhsold);
            Assert.IsNotNull(enrollmentStream, "authentication stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (AuthenticateRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreEqual(configMessage.Config.EnrollmentId, enrollmentId, "enrollmentId should match what was passed in");
            Assert.AreEqual(configMessage.Config.IsLivenessEnabled, isLivenessEnabled, "isLivenessEnabled should match what was passed in");
            Assert.AreEqual(configMessage.Config.LivenessThreshold, threhsold, "threhsold should match what was passed in");
        }

        [Test]
        public async Task TestStreamGroupAuthentication()
        {
            var tokenManager = new MockTokenManager();
            var videoModelsClient = new Mock<VideoModels.VideoModelsClient>();
            var videoBiometricsClient = new Mock<VideoBiometrics.VideoBiometricsClient>();
            var videoRecognitionsClient = new Mock<VideoRecognition.VideoRecognitionClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<AuthenticateRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<AuthenticateResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            videoBiometricsClient.Setup(m => m.Authenticate(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var videoService = new MockVideoService(
                new Config(new SDKInitConfig { }),
                tokenManager,
                videoModelsClient.Object,
                videoBiometricsClient.Object,
                videoRecognitionsClient.Object
            );

            var enrollmentGroupId = "enrollmentId";
            var isLivenessEnabled = true;
            var threhsold = RecognitionThreshold.Low;

            var enrollmentStream = await videoService.StreamGroupAuthentication(enrollmentGroupId, isLivenessEnabled, threhsold);
            Assert.IsNotNull(enrollmentStream, "authentication stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (AuthenticateRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreEqual(configMessage.Config.EnrollmentGroupId, enrollmentGroupId, "enrollmentGroupId should match what was passed in");
            Assert.AreEqual(configMessage.Config.IsLivenessEnabled, isLivenessEnabled, "isLivenessEnabled should match what was passed in");
            Assert.AreEqual(configMessage.Config.LivenessThreshold, threhsold, "threhsold should match what was passed in");
        }

        [Test]
        public async Task TestStreamRecognition()
        {
            var tokenManager = new MockTokenManager();
            var videoModelsClient = new Mock<VideoModels.VideoModelsClient>();
            var videoBiometricsClient = new Mock<VideoBiometrics.VideoBiometricsClient>();
            var videoRecognitionsClient = new Mock<VideoRecognition.VideoRecognitionClient>();

            var mockRequestStream = new Mock<IClientStreamWriter<ValidateRecognitionRequest>>();
            var mockResponseStream = new Mock<IAsyncStreamReader<LivenessRecognitionResponse>>();

            var fakeCall = TestCalls.AsyncDuplexStreamingCall(mockRequestStream.Object, mockResponseStream.Object, Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });
            videoRecognitionsClient.Setup(m => m.ValidateLiveness(It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(fakeCall);

            var videoService = new MockVideoService(
                new Config(new SDKInitConfig { }),
                tokenManager,
                videoModelsClient.Object,
                videoBiometricsClient.Object,
                videoRecognitionsClient.Object
            );

            var userId = "userId";
            var modelName = "modelName";
            var threhsold = RecognitionThreshold.Highest;

            var enrollmentStream = await videoService.StreamLivenessRecognition(userId, modelName, threhsold);
            Assert.IsNotNull(enrollmentStream, "recognition stream should be returned");

            Assert.AreEqual(mockRequestStream.Invocations.Count, 1, "one call should have been made to pass config to the server");
            var configMessage = (ValidateRecognitionRequest)mockRequestStream.Invocations[0].Arguments[0];

            Assert.AreEqual(configMessage.Config.UserId, userId, "userId should match what was passed in");
            Assert.AreEqual(configMessage.Config.ModelName, modelName, "modelName should match what was passed in");
            Assert.AreEqual(configMessage.Config.Threshold, threhsold, "threhsold should match what was passed in");
        }
    }

    public class MockVideoService : VideoService
    {
        public MockVideoService(
            Config config,
            ITokenManager tokenManager,
            VideoModels.VideoModelsClient videoModelsClient,
            VideoBiometrics.VideoBiometricsClient videoBiometricsClient,
            VideoRecognition.VideoRecognitionClient videoRecognitionClient) :
            base(config, tokenManager, videoModelsClient, videoBiometricsClient, videoRecognitionClient)
        {

        }
    }
}
