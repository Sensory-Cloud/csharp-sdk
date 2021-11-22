using System;
using SensoryCloud.Src.TokenManager;
using Sensory.Api.V1.Management;
using Grpc.Core;

namespace SensoryCloud.Src.Services
{
    /// <summary>
    /// Service to handle all typical CRUD functions
    /// </summary>
    public class ManagementService
    {
        private readonly Config Config;
        private readonly EnrollmentService.EnrollmentServiceClient EnrollmentClient;
        private readonly ITokenManager TokenManager;

        public ManagementService(Config config, ITokenManager tokenManager)
        {
            this.Config = config;
            this.TokenManager = tokenManager;
            this.EnrollmentClient = new EnrollmentService.EnrollmentServiceClient(config.GetChannel());
        }

        /// <summary>
        /// Obtains all of the active enrollments given the userId
        /// </summary>
        /// <param name="userId">the unique userId</param>
        /// <returns>a list of user enrollments</returns>
        public GetEnrollmentsResponse GetEnrollments(string userId)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            GetEnrollmentsRequest request = new GetEnrollmentsRequest { UserId = userId };

            return this.EnrollmentClient.GetEnrollments(request, metadata);
        }

        /// <summary>
        /// Obtains all of the active enrollment groups registered by this userId
        /// </summary>
        /// <param name="userId">the unique userId</param>
        /// <returns>a list of enrollment groups</returns>
        public GetEnrollmentGroupsResponse GetEnrollmentGroups(string userId)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            GetEnrollmentsRequest request = new GetEnrollmentsRequest { UserId = userId };

            return this.EnrollmentClient.GetEnrollmentGroups(request, metadata);
        }

        /// <summary>
        /// Register a new enrollment group. Enrollment groups can contain up to 10 enrollments, and they enable multiple users to be recognized with the same request.
        /// </summary>
        /// <param name="userId">the unique userId who will act as the owner of this enrollment group</param>
        /// <param name="groupId">the ID to be associated with this group</param>
        /// <param name="groupName">the friendly name of this group</param>
        /// <param name="description">a brief description of this group</param>
        /// <param name="modelName">the exact name of the model tied to this enrollment group. This model name can be retrieved from the getModels() call.</param>
        /// <param name="enrollmentIds">The enrollmentIds to be associated with this group. Max 10.</param>
        /// <returns>a summary of the enrollment group</returns>
        public EnrollmentGroupResponse CreateEnrollmentGroup(string userId, string groupId, string groupName, string description, string modelName, string[] enrollmentIds)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            CreateEnrollmentGroupRequest request = new CreateEnrollmentGroupRequest
            {
                UserId = userId,
                Id = groupId,
                Name = groupName,
                Description = description,
                ModelName = modelName
            };

            request.EnrollmentIds.Add(enrollmentIds);

            return this.EnrollmentClient.CreateEnrollmentGroup(request, metadata);
        }

        /// <summary>
        /// Add a new enrollment to an enrollment group.
        /// </summary>
        /// <param name="groupId">the ID associated with this group</param>
        /// <param name="enrollmentIds">the enrollmentIds to be associated with this group. Max 10.</param>
        /// <returns>a summary of the enrollment group</returns>
        public EnrollmentGroupResponse AppendEnrollmentGroup(string groupId, string[] enrollmentIds)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            AppendEnrollmentGroupRequest request = new AppendEnrollmentGroupRequest
            {
                GroupId = groupId
            };

            request.EnrollmentIds.Add(enrollmentIds);
            return this.EnrollmentClient.AppendEnrollmentGroup(request, metadata);
        }

        /// <summary>
        /// Removes an enrollment from the system
        /// </summary>
        /// <param name="id">the unique enrollmentId</param>
        /// <returns>a summary of the removed enrollment</returns>
        public EnrollmentResponse DeleteEnrollment(string id)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            DeleteEnrollmentRequest request = new DeleteEnrollmentRequest { Id = id };
            return this.EnrollmentClient.DeleteEnrollment(request, metadata);
        }

        /// <summary>
        /// Removes an enrollment group from the system
        /// </summary>
        /// <param name="groupId">the unique enrollmentGroupId</param>
        /// <returns>a summary of the removed enrollment group</returns>
        public EnrollmentGroupResponse DeleteEnrollmentGroup(string groupId)
        {
            Metadata metadata = this.TokenManager.GetAuthorizationMetadata();
            DeleteEnrollmentGroupRequest request = new DeleteEnrollmentGroupRequest { Id = groupId };
            return this.EnrollmentClient.DeleteEnrollmentGroup(request, metadata);
        }
    }
}
