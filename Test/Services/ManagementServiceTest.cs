using NUnit.Framework;
using Moq;
using Sensory.Api.V1.Management;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using SensoryCloud.Src.TokenManager;
using System.Threading;
using Grpc.Core;

namespace Test.Services
{
    [TestFixture]
    public class ManagementServiceTest
    {
        [Test]
        public void TestGetEnrollments()
        {
            var enrollmentClient = new Mock<EnrollmentService.EnrollmentServiceClient>();
            var tokenManager = new MockTokenManager();
            var response = new GetEnrollmentsResponse();

            response.Enrollments.Add(new EnrollmentResponse { Description = "enrollment", DidEnrollWithLiveness = false });

            enrollmentClient.Setup(client => client.GetEnrollments(It.IsAny<GetEnrollmentsRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            var managementService = new MockManagementService(new Config("doesnt-matter", "doesnt-matter"), tokenManager, enrollmentClient.Object);

            var enrollmentResponse = managementService.GetEnrollments("foo");
            Assert.AreSame(enrollmentResponse, response);
        }

        [Test]
        public void TestGetEnrollmentGroups()
        {
            var enrollmentClient = new Mock<EnrollmentService.EnrollmentServiceClient>();
            var tokenManager = new MockTokenManager();
            var response = new GetEnrollmentGroupsResponse();

            response.EnrollmentGroups.Add(new EnrollmentGroupResponse { Description = "enrollment-group", ModelName = "my-model" });

            enrollmentClient.Setup(client => client.GetEnrollmentGroups(It.IsAny<GetEnrollmentsRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            var managementService = new MockManagementService(new Config("doesnt-matter", "doesnt-matter"), tokenManager, enrollmentClient.Object);

            var enrollmentResponse = managementService.GetEnrollmentGroups("foo");
            Assert.AreSame(enrollmentResponse, response);
        }

        [Test]
        public void TestCreateEnrollmentGroup()
        {
            var enrollmentClient = new Mock<EnrollmentService.EnrollmentServiceClient>();
            var tokenManager = new MockTokenManager();
            var response = new EnrollmentGroupResponse(){
                Description = "enrollment-group",
                ModelName = "my-model"
            };

            enrollmentClient.Setup(client => client.CreateEnrollmentGroup(It.IsAny<CreateEnrollmentGroupRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            var managementService = new MockManagementService(new Config("doesnt-matter", "doesnt-matter"), tokenManager, enrollmentClient.Object);

            var enrollmentResponse = managementService.CreateEnrollmentGroup("foo", "bar", "baz", "wuz", "wux", new string[] { "1", "2", "3" });
            Assert.AreSame(enrollmentResponse, response);
        }

        [Test]
        public void TestAppendEnrollmentGroup()
        {
            var enrollmentClient = new Mock<EnrollmentService.EnrollmentServiceClient>();
            var tokenManager = new MockTokenManager();
            var response = new EnrollmentGroupResponse()
            {
                Description = "enrollment-group",
                ModelName = "my-model"
            };

            enrollmentClient.Setup(client => client.AppendEnrollmentGroup(It.IsAny<AppendEnrollmentGroupRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            var managementService = new MockManagementService(new Config("doesnt-matter", "doesnt-matter"), tokenManager, enrollmentClient.Object);

            var enrollmentResponse = managementService.AppendEnrollmentGroup("foo", new string[] { "1", "2", "3" });
            Assert.AreSame(enrollmentResponse, response);
        }

        [Test]
        public void TestDeleteEnrollment()
        {
            var enrollmentClient = new Mock<EnrollmentService.EnrollmentServiceClient>();
            var tokenManager = new MockTokenManager();
            var response = new EnrollmentResponse()
            {
                Description = "enrollment",
                ModelName = "my-model"
            };

            enrollmentClient.Setup(client => client.DeleteEnrollment(It.IsAny<DeleteEnrollmentRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            var managementService = new MockManagementService(new Config("doesnt-matter", "doesnt-matter"), tokenManager, enrollmentClient.Object);

            var enrollmentResponse = managementService.DeleteEnrollment("foo");
            Assert.AreSame(enrollmentResponse, response);
        }

        [Test]
        public void TestDeleteEnrollmentGroup()
        {
            var enrollmentClient = new Mock<EnrollmentService.EnrollmentServiceClient>();
            var tokenManager = new MockTokenManager();
            var response = new EnrollmentGroupResponse()
            {
                Description = "enrollment",
                ModelName = "my-model"
            };

            enrollmentClient.Setup(client => client.DeleteEnrollmentGroup(It.IsAny<DeleteEnrollmentGroupRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            var managementService = new MockManagementService(new Config("doesnt-matter", "doesnt-matter"), tokenManager, enrollmentClient.Object);

            var enrollmentResponse = managementService.DeleteEnrollmentGroup("foo");
            Assert.AreSame(enrollmentResponse, response);
        }

    }

    public class MockManagementService: ManagementService
    {
        public MockManagementService(
            Config config, ITokenManager tokenManager, EnrollmentService.EnrollmentServiceClient enrollmentClient): base(config, tokenManager, enrollmentClient)
        {

        }
    }

    public class MockTokenManager : ITokenManager
    {
        public string Token { get; }

        public Metadata GetAuthorizationMetadata()
        {
            string token = this.GetToken();
            return new Metadata
            {
                {"Authorization", "Bearer " + token }
            };
        }

        public string GetToken()
        {
            return this.Token;
        }
    }
}
