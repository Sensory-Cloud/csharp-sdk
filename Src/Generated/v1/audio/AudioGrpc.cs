// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: v1/audio/audio.proto
// </auto-generated>
// Original file comments:
// sensory.api.audio
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Sensory.Api.V1.Audio {
  /// <summary>
  /// Handles the retrieval and management of audio models
  /// </summary>
  public static partial class AudioModels
  {
    static readonly string __ServiceName = "sensory.api.v1.audio.AudioModels";

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
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.GetModelsRequest> __Marshaller_sensory_api_v1_audio_GetModelsRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.GetModelsRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.GetModelsResponse> __Marshaller_sensory_api_v1_audio_GetModelsResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.GetModelsResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Audio.GetModelsRequest, global::Sensory.Api.V1.Audio.GetModelsResponse> __Method_GetModels = new grpc::Method<global::Sensory.Api.V1.Audio.GetModelsRequest, global::Sensory.Api.V1.Audio.GetModelsResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetModels",
        __Marshaller_sensory_api_v1_audio_GetModelsRequest,
        __Marshaller_sensory_api_v1_audio_GetModelsResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sensory.Api.V1.Audio.AudioReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for AudioModels</summary>
    public partial class AudioModelsClient : grpc::ClientBase<AudioModelsClient>
    {
      /// <summary>Creates a new client for AudioModels</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AudioModelsClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for AudioModels that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AudioModelsClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AudioModelsClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AudioModelsClient(ClientBaseConfiguration configuration) : base(configuration)
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
      public virtual global::Sensory.Api.V1.Audio.GetModelsResponse GetModels(global::Sensory.Api.V1.Audio.GetModelsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
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
      public virtual global::Sensory.Api.V1.Audio.GetModelsResponse GetModels(global::Sensory.Api.V1.Audio.GetModelsRequest request, grpc::CallOptions options)
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
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.V1.Audio.GetModelsResponse> GetModelsAsync(global::Sensory.Api.V1.Audio.GetModelsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
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
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.V1.Audio.GetModelsResponse> GetModelsAsync(global::Sensory.Api.V1.Audio.GetModelsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetModels, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override AudioModelsClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AudioModelsClient(configuration);
      }
    }

  }
  /// <summary>
  /// Handles all audio-related biometrics
  /// </summary>
  public static partial class AudioBiometrics
  {
    static readonly string __ServiceName = "sensory.api.v1.audio.AudioBiometrics";

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
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.CreateEnrollmentRequest> __Marshaller_sensory_api_v1_audio_CreateEnrollmentRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.CreateEnrollmentRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.CreateEnrollmentResponse> __Marshaller_sensory_api_v1_audio_CreateEnrollmentResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.CreateEnrollmentResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.AuthenticateRequest> __Marshaller_sensory_api_v1_audio_AuthenticateRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.AuthenticateRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.AuthenticateResponse> __Marshaller_sensory_api_v1_audio_AuthenticateResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.AuthenticateResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Audio.CreateEnrollmentRequest, global::Sensory.Api.V1.Audio.CreateEnrollmentResponse> __Method_CreateEnrollment = new grpc::Method<global::Sensory.Api.V1.Audio.CreateEnrollmentRequest, global::Sensory.Api.V1.Audio.CreateEnrollmentResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "CreateEnrollment",
        __Marshaller_sensory_api_v1_audio_CreateEnrollmentRequest,
        __Marshaller_sensory_api_v1_audio_CreateEnrollmentResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Audio.AuthenticateRequest, global::Sensory.Api.V1.Audio.AuthenticateResponse> __Method_Authenticate = new grpc::Method<global::Sensory.Api.V1.Audio.AuthenticateRequest, global::Sensory.Api.V1.Audio.AuthenticateResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "Authenticate",
        __Marshaller_sensory_api_v1_audio_AuthenticateRequest,
        __Marshaller_sensory_api_v1_audio_AuthenticateResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sensory.Api.V1.Audio.AudioReflection.Descriptor.Services[1]; }
    }

    /// <summary>Client for AudioBiometrics</summary>
    public partial class AudioBiometricsClient : grpc::ClientBase<AudioBiometricsClient>
    {
      /// <summary>Creates a new client for AudioBiometrics</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AudioBiometricsClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for AudioBiometrics that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AudioBiometricsClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AudioBiometricsClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AudioBiometricsClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Enrolls a user with a stream of audio. Streams a CreateEnrollmentResponse
      /// as the audio is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Audio.CreateEnrollmentRequest, global::Sensory.Api.V1.Audio.CreateEnrollmentResponse> CreateEnrollment(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateEnrollment(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Enrolls a user with a stream of audio. Streams a CreateEnrollmentResponse
      /// as the audio is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Audio.CreateEnrollmentRequest, global::Sensory.Api.V1.Audio.CreateEnrollmentResponse> CreateEnrollment(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_CreateEnrollment, null, options);
      }
      /// <summary>
      /// Authenticates a user with a stream of audio against an existing enrollment.
      /// Streams an AuthenticateResponse as the audio is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Audio.AuthenticateRequest, global::Sensory.Api.V1.Audio.AuthenticateResponse> Authenticate(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Authenticate(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Authenticates a user with a stream of audio against an existing enrollment.
      /// Streams an AuthenticateResponse as the audio is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Audio.AuthenticateRequest, global::Sensory.Api.V1.Audio.AuthenticateResponse> Authenticate(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_Authenticate, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override AudioBiometricsClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AudioBiometricsClient(configuration);
      }
    }

  }
  /// <summary>
  /// Handles all audio event processing
  /// </summary>
  public static partial class AudioEvents
  {
    static readonly string __ServiceName = "sensory.api.v1.audio.AudioEvents";

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
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.ValidateEventRequest> __Marshaller_sensory_api_v1_audio_ValidateEventRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.ValidateEventRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.ValidateEventResponse> __Marshaller_sensory_api_v1_audio_ValidateEventResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.ValidateEventResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Audio.ValidateEventRequest, global::Sensory.Api.V1.Audio.ValidateEventResponse> __Method_ValidateEvent = new grpc::Method<global::Sensory.Api.V1.Audio.ValidateEventRequest, global::Sensory.Api.V1.Audio.ValidateEventResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "ValidateEvent",
        __Marshaller_sensory_api_v1_audio_ValidateEventRequest,
        __Marshaller_sensory_api_v1_audio_ValidateEventResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sensory.Api.V1.Audio.AudioReflection.Descriptor.Services[2]; }
    }

    /// <summary>Client for AudioEvents</summary>
    public partial class AudioEventsClient : grpc::ClientBase<AudioEventsClient>
    {
      /// <summary>Creates a new client for AudioEvents</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AudioEventsClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for AudioEvents that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AudioEventsClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AudioEventsClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AudioEventsClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Validates a phrase or sound with a stream of audio.
      /// Streams a ValidateEventResponse as the audio is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Audio.ValidateEventRequest, global::Sensory.Api.V1.Audio.ValidateEventResponse> ValidateEvent(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ValidateEvent(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Validates a phrase or sound with a stream of audio.
      /// Streams a ValidateEventResponse as the audio is processed.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Audio.ValidateEventRequest, global::Sensory.Api.V1.Audio.ValidateEventResponse> ValidateEvent(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_ValidateEvent, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override AudioEventsClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AudioEventsClient(configuration);
      }
    }

  }
  /// <summary>
  /// Handles all audio transcriptions
  /// </summary>
  public static partial class AudioTranscriptions
  {
    static readonly string __ServiceName = "sensory.api.v1.audio.AudioTranscriptions";

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
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.TranscribeRequest> __Marshaller_sensory_api_v1_audio_TranscribeRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.TranscribeRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.V1.Audio.TranscribeResponse> __Marshaller_sensory_api_v1_audio_TranscribeResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.V1.Audio.TranscribeResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.V1.Audio.TranscribeRequest, global::Sensory.Api.V1.Audio.TranscribeResponse> __Method_Transcribe = new grpc::Method<global::Sensory.Api.V1.Audio.TranscribeRequest, global::Sensory.Api.V1.Audio.TranscribeResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "Transcribe",
        __Marshaller_sensory_api_v1_audio_TranscribeRequest,
        __Marshaller_sensory_api_v1_audio_TranscribeResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sensory.Api.V1.Audio.AudioReflection.Descriptor.Services[3]; }
    }

    /// <summary>Client for AudioTranscriptions</summary>
    public partial class AudioTranscriptionsClient : grpc::ClientBase<AudioTranscriptionsClient>
    {
      /// <summary>Creates a new client for AudioTranscriptions</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AudioTranscriptionsClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for AudioTranscriptions that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AudioTranscriptionsClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AudioTranscriptionsClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AudioTranscriptionsClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Transcribes voice into text.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Audio.TranscribeRequest, global::Sensory.Api.V1.Audio.TranscribeResponse> Transcribe(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Transcribe(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Transcribes voice into text.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Sensory.Api.V1.Audio.TranscribeRequest, global::Sensory.Api.V1.Audio.TranscribeResponse> Transcribe(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_Transcribe, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override AudioTranscriptionsClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AudioTranscriptionsClient(configuration);
      }
    }

  }
}
#endregion
