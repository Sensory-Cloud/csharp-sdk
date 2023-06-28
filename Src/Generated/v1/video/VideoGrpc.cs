// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: v1/video/video.proto
// </auto-generated>
// Original file comments:
// sensory.api.video
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Sensory.Api.V1.Video {
  /// <summary>
  /// Handles the retrieval and management of video models
  /// </summary>
  public static partial class VideoModels
  {
    static readonly string __ServiceName = "sensory.api.v1.video.VideoModels";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Video.GetModelsRequest> __Marshaller_sensory_api_v1_video_GetModelsRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Video.GetModelsRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Video.GetModelsResponse> __Marshaller_sensory_api_v1_video_GetModelsResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Video.GetModelsResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Video.GetModelsRequest, global::Sensory.Api.V1.Video.GetModelsResponse> __Method_GetModels = new grpc::Method<global::Sensory.Api.V1.Video.GetModelsRequest, global::Sensory.Api.V1.Video.GetModelsResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetModels",
        __Marshaller_sensory_api_v1_video_GetModelsRequest,
        __Marshaller_sensory_api_v1_video_GetModelsResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sensory.Api.V1.Video.VideoReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for VideoModels</summary>
    public partial class VideoModelsClient : grpc::ClientBase<VideoModelsClient>
    {
      /// <summary>Creates a new client for VideoModels</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public VideoModelsClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for VideoModels that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public VideoModelsClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected VideoModelsClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected VideoModelsClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Get available models for enrollment and authentication
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.V1.Video.GetModelsResponse GetModels(global::Sensory.Api.V1.Video.GetModelsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetModels(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Get available models for enrollment and authentication
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.V1.Video.GetModelsResponse GetModels(global::Sensory.Api.V1.Video.GetModelsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetModels, null, options, request);
      }
      /// <summary>
      /// Get available models for enrollment and authentication
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.V1.Video.GetModelsResponse> GetModelsAsync(global::Sensory.Api.V1.Video.GetModelsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetModelsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Get available models for enrollment and authentication
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.V1.Video.GetModelsResponse> GetModelsAsync(global::Sensory.Api.V1.Video.GetModelsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetModels, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override VideoModelsClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new VideoModelsClient(configuration);
      }
    }

  }
  /// <summary>
  /// Handles all video-related biometrics
  /// </summary>
  public static partial class VideoBiometrics
  {
    static readonly string __ServiceName = "sensory.api.v1.video.VideoBiometrics";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Video.CreateEnrollmentRequest> __Marshaller_sensory_api_v1_video_CreateEnrollmentRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Video.CreateEnrollmentRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Video.CreateEnrollmentResponse> __Marshaller_sensory_api_v1_video_CreateEnrollmentResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Video.CreateEnrollmentResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Video.AuthenticateRequest> __Marshaller_sensory_api_v1_video_AuthenticateRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Video.AuthenticateRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Video.AuthenticateResponse> __Marshaller_sensory_api_v1_video_AuthenticateResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Video.AuthenticateResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Video.CreateEnrollmentRequest, global::Sensory.Api.V1.Video.CreateEnrollmentResponse> __Method_CreateEnrollment = new grpc::Method<global::Sensory.Api.V1.Video.CreateEnrollmentRequest, global::Sensory.Api.V1.Video.CreateEnrollmentResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "CreateEnrollment",
        __Marshaller_sensory_api_v1_video_CreateEnrollmentRequest,
        __Marshaller_sensory_api_v1_video_CreateEnrollmentResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Video.AuthenticateRequest, global::Sensory.Api.V1.Video.AuthenticateResponse> __Method_Authenticate = new grpc::Method<global::Sensory.Api.V1.Video.AuthenticateRequest, global::Sensory.Api.V1.Video.AuthenticateResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "Authenticate",
        __Marshaller_sensory_api_v1_video_AuthenticateRequest,
        __Marshaller_sensory_api_v1_video_AuthenticateResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sensory.Api.V1.Video.VideoReflection.Descriptor.Services[1]; }
    }

    /// <summary>Client for VideoBiometrics</summary>
    public partial class VideoBiometricsClient : grpc::ClientBase<VideoBiometricsClient>
    {
      /// <summary>Creates a new client for VideoBiometrics</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public VideoBiometricsClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for VideoBiometrics that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public VideoBiometricsClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected VideoBiometricsClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected VideoBiometricsClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Enrolls a user with a stream of video. Streams a CreateEnrollmentResponse
      /// as the video is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Video.CreateEnrollmentRequest, global::Sensory.Api.V1.Video.CreateEnrollmentResponse> CreateEnrollment(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateEnrollment(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Enrolls a user with a stream of video. Streams a CreateEnrollmentResponse
      /// as the video is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Video.CreateEnrollmentRequest, global::Sensory.Api.V1.Video.CreateEnrollmentResponse> CreateEnrollment(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_CreateEnrollment, null, options);
      }
      /// <summary>
      /// Authenticates a user with a stream of video against an existing enrollment.
      /// Streams an AuthenticateResponse as the video is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Video.AuthenticateRequest, global::Sensory.Api.V1.Video.AuthenticateResponse> Authenticate(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Authenticate(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Authenticates a user with a stream of video against an existing enrollment.
      /// Streams an AuthenticateResponse as the video is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Video.AuthenticateRequest, global::Sensory.Api.V1.Video.AuthenticateResponse> Authenticate(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_Authenticate, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override VideoBiometricsClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new VideoBiometricsClient(configuration);
      }
    }

  }
  /// <summary>
  /// Handles all video recognition endpoints
  /// </summary>
  public static partial class VideoRecognition
  {
    static readonly string __ServiceName = "sensory.api.v1.video.VideoRecognition";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Video.ValidateRecognitionRequest> __Marshaller_sensory_api_v1_video_ValidateRecognitionRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Video.ValidateRecognitionRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Video.LivenessRecognitionResponse> __Marshaller_sensory_api_v1_video_LivenessRecognitionResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Video.LivenessRecognitionResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Video.ValidateRecognitionRequest, global::Sensory.Api.V1.Video.LivenessRecognitionResponse> __Method_ValidateLiveness = new grpc::Method<global::Sensory.Api.V1.Video.ValidateRecognitionRequest, global::Sensory.Api.V1.Video.LivenessRecognitionResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "ValidateLiveness",
        __Marshaller_sensory_api_v1_video_ValidateRecognitionRequest,
        __Marshaller_sensory_api_v1_video_LivenessRecognitionResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sensory.Api.V1.Video.VideoReflection.Descriptor.Services[2]; }
    }

    /// <summary>Client for VideoRecognition</summary>
    public partial class VideoRecognitionClient : grpc::ClientBase<VideoRecognitionClient>
    {
      /// <summary>Creates a new client for VideoRecognition</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public VideoRecognitionClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for VideoRecognition that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public VideoRecognitionClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected VideoRecognitionClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected VideoRecognitionClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Validates the liveness of a single image or stream of images.
      /// Streams a ValidateRecognitionResponse as the images are processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Video.ValidateRecognitionRequest, global::Sensory.Api.V1.Video.LivenessRecognitionResponse> ValidateLiveness(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ValidateLiveness(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Validates the liveness of a single image or stream of images.
      /// Streams a ValidateRecognitionResponse as the images are processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Video.ValidateRecognitionRequest, global::Sensory.Api.V1.Video.LivenessRecognitionResponse> ValidateLiveness(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_ValidateLiveness, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override VideoRecognitionClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new VideoRecognitionClient(configuration);
      }
    }

  }
}
#endregion
