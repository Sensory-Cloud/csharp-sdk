// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: oauth/oauth.proto
// </auto-generated>
// Original file comments:
// sensory.api.oauth
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Sensory.Api.Oauth {
  /// <summary>
  /// Service for OAuth function
  /// </summary>
  public static partial class OauthService
  {
    static readonly string __ServiceName = "sensory.api.oauth.OauthService";

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
    static readonly grpc::Marshaller<global::Sensory.Api.Oauth.TokenRequest> __Marshaller_sensory_api_oauth_TokenRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.Oauth.TokenRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.Common.TokenResponse> __Marshaller_sensory_api_common_TokenResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.Common.TokenResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.Oauth.SignTokenRequest> __Marshaller_sensory_api_oauth_SignTokenRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.Oauth.SignTokenRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.Oauth.WhoAmIRequest> __Marshaller_sensory_api_oauth_WhoAmIRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.Oauth.WhoAmIRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.Oauth.WhoAmIResponse> __Marshaller_sensory_api_oauth_WhoAmIResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.Oauth.WhoAmIResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.Oauth.PublicKeyRequest> __Marshaller_sensory_api_oauth_PublicKeyRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.Oauth.PublicKeyRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sensory.Api.Oauth.PublicKeyResponse> __Marshaller_sensory_api_oauth_PublicKeyResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sensory.Api.Oauth.PublicKeyResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.Oauth.TokenRequest, global::Sensory.Api.Common.TokenResponse> __Method_GetToken = new grpc::Method<global::Sensory.Api.Oauth.TokenRequest, global::Sensory.Api.Common.TokenResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetToken",
        __Marshaller_sensory_api_oauth_TokenRequest,
        __Marshaller_sensory_api_common_TokenResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.Oauth.SignTokenRequest, global::Sensory.Api.Common.TokenResponse> __Method_SignToken = new grpc::Method<global::Sensory.Api.Oauth.SignTokenRequest, global::Sensory.Api.Common.TokenResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SignToken",
        __Marshaller_sensory_api_oauth_SignTokenRequest,
        __Marshaller_sensory_api_common_TokenResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.Oauth.WhoAmIRequest, global::Sensory.Api.Oauth.WhoAmIResponse> __Method_GetWhoAmI = new grpc::Method<global::Sensory.Api.Oauth.WhoAmIRequest, global::Sensory.Api.Oauth.WhoAmIResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetWhoAmI",
        __Marshaller_sensory_api_oauth_WhoAmIRequest,
        __Marshaller_sensory_api_oauth_WhoAmIResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sensory.Api.Oauth.PublicKeyRequest, global::Sensory.Api.Oauth.PublicKeyResponse> __Method_GetPublicKey = new grpc::Method<global::Sensory.Api.Oauth.PublicKeyRequest, global::Sensory.Api.Oauth.PublicKeyResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetPublicKey",
        __Marshaller_sensory_api_oauth_PublicKeyRequest,
        __Marshaller_sensory_api_oauth_PublicKeyResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sensory.Api.Oauth.OauthReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for OauthService</summary>
    public partial class OauthServiceClient : grpc::ClientBase<OauthServiceClient>
    {
      /// <summary>Creates a new client for OauthService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public OauthServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for OauthService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public OauthServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected OauthServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected OauthServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Obtain an OAuth token for the given credentials
      /// Endpoint does not require an authorization token
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.Common.TokenResponse GetToken(global::Sensory.Api.Oauth.TokenRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetToken(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Obtain an OAuth token for the given credentials
      /// Endpoint does not require an authorization token
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.Common.TokenResponse GetToken(global::Sensory.Api.Oauth.TokenRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetToken, null, options, request);
      }
      /// <summary>
      /// Obtain an OAuth token for the given credentials
      /// Endpoint does not require an authorization token
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.Common.TokenResponse> GetTokenAsync(global::Sensory.Api.Oauth.TokenRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetTokenAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Obtain an OAuth token for the given credentials
      /// Endpoint does not require an authorization token
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.Common.TokenResponse> GetTokenAsync(global::Sensory.Api.Oauth.TokenRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetToken, null, options, request);
      }
      /// <summary>
      /// Sign and return an OAuth token. The passed authorization token must have the SignToken authority.
      /// Therefore, Devices are not allowed to make this request.
      /// Sign does not validate credentials, and therefore should be used in specific circumstances where credentials are not required.
      /// One common usecase for the Sign request is an Io server issuing a user-scoped token after a successful authentication.
      /// Only a limited subset of of scopes may be requested from the SignToken request.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.Common.TokenResponse SignToken(global::Sensory.Api.Oauth.SignTokenRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SignToken(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Sign and return an OAuth token. The passed authorization token must have the SignToken authority.
      /// Therefore, Devices are not allowed to make this request.
      /// Sign does not validate credentials, and therefore should be used in specific circumstances where credentials are not required.
      /// One common usecase for the Sign request is an Io server issuing a user-scoped token after a successful authentication.
      /// Only a limited subset of of scopes may be requested from the SignToken request.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.Common.TokenResponse SignToken(global::Sensory.Api.Oauth.SignTokenRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SignToken, null, options, request);
      }
      /// <summary>
      /// Sign and return an OAuth token. The passed authorization token must have the SignToken authority.
      /// Therefore, Devices are not allowed to make this request.
      /// Sign does not validate credentials, and therefore should be used in specific circumstances where credentials are not required.
      /// One common usecase for the Sign request is an Io server issuing a user-scoped token after a successful authentication.
      /// Only a limited subset of of scopes may be requested from the SignToken request.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.Common.TokenResponse> SignTokenAsync(global::Sensory.Api.Oauth.SignTokenRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SignTokenAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Sign and return an OAuth token. The passed authorization token must have the SignToken authority.
      /// Therefore, Devices are not allowed to make this request.
      /// Sign does not validate credentials, and therefore should be used in specific circumstances where credentials are not required.
      /// One common usecase for the Sign request is an Io server issuing a user-scoped token after a successful authentication.
      /// Only a limited subset of of scopes may be requested from the SignToken request.
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.Common.TokenResponse> SignTokenAsync(global::Sensory.Api.Oauth.SignTokenRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SignToken, null, options, request);
      }
      /// <summary>
      /// Obtain information about oneself based on the passed OAuth token
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.Oauth.WhoAmIResponse GetWhoAmI(global::Sensory.Api.Oauth.WhoAmIRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetWhoAmI(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Obtain information about oneself based on the passed OAuth token
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.Oauth.WhoAmIResponse GetWhoAmI(global::Sensory.Api.Oauth.WhoAmIRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetWhoAmI, null, options, request);
      }
      /// <summary>
      /// Obtain information about oneself based on the passed OAuth token
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.Oauth.WhoAmIResponse> GetWhoAmIAsync(global::Sensory.Api.Oauth.WhoAmIRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetWhoAmIAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Obtain information about oneself based on the passed OAuth token
      /// Authorization metadata is required {"authorization": "Bearer &lt;TOKEN>"}
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.Oauth.WhoAmIResponse> GetWhoAmIAsync(global::Sensory.Api.Oauth.WhoAmIRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetWhoAmI, null, options, request);
      }
      /// <summary>
      /// Retrieve a public key by ID. Public keys retrieved by this endpoint can be used
      /// to validate tokens signed by the Sensory cloud.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.Oauth.PublicKeyResponse GetPublicKey(global::Sensory.Api.Oauth.PublicKeyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetPublicKey(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Retrieve a public key by ID. Public keys retrieved by this endpoint can be used
      /// to validate tokens signed by the Sensory cloud.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sensory.Api.Oauth.PublicKeyResponse GetPublicKey(global::Sensory.Api.Oauth.PublicKeyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetPublicKey, null, options, request);
      }
      /// <summary>
      /// Retrieve a public key by ID. Public keys retrieved by this endpoint can be used
      /// to validate tokens signed by the Sensory cloud.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.Oauth.PublicKeyResponse> GetPublicKeyAsync(global::Sensory.Api.Oauth.PublicKeyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetPublicKeyAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Retrieve a public key by ID. Public keys retrieved by this endpoint can be used
      /// to validate tokens signed by the Sensory cloud.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sensory.Api.Oauth.PublicKeyResponse> GetPublicKeyAsync(global::Sensory.Api.Oauth.PublicKeyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetPublicKey, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override OauthServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new OauthServiceClient(configuration);
      }
    }

  }
}
#endregion