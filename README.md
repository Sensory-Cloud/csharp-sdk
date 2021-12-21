# C Sharp SDK

Sensory Cloud C# .Net SDK is build upon .Net Standard 2.0 in order to maximize [compatibility](https://dotnet.microsoft.com/platform/dotnet-standard#versions).

## General Information

Before getting started, you must spin up a Sensory Cloud inference server or have Sensory spin one up for you. You must also have the following pieces of information:
* Your inference server URL
* Your Sensory Tenant ID (UUID)
* Your configured secret key used to register OAuth clients

## Checking Server Health

It's important to check the health of your Sensory Inference server. You can do so via the following:

```csharp
// Tenant ID granted by Sensory Inc.
string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
string deviceId = "a-hardware-identifier-unique-to-your-device";

// Configuration specific to your tenant
Config config = new Config("your-inference-server.com", sensoryTenantId, deviceId).Connect();

HealthService healthService = new HealthService(config);

ServerHealthResponse serverHealth = healthService.GetHealth();
```

## Secure Credential Store

ISecureCredential is an interface that store and serves your OAuth credentials (clientId and clientSecret).
ISecureCredential must be implemented by you and the credentials should be persisted in a secure manner, such as in an encrypted database.
OAuth credentials should be generated one time per unique machine.

A crude example of ISecureCredential is as follows:

```csharp
public class SecureCredentialStoreExample : ISecureCredentialStore
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public string GetClientId()
    {
        return ClientId;
    }

    public string GetClientSecret()
    {
        return ClientSecret;
    }
}
```

## Registering OAuth Credentials

OAuth credentials should be registered once per unique machine. Registration is very simple, and provided as part of the SDK.

The below example shows how to create an OAuthService and register a client for the first time.

```csharp
// Tenant ID granted by Sensory Inc.
string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
string deviceId = "a-hardware-identifier-unique-to-your-device";

// Configuration specific to your tenant
Config config = new Config("your-inference-server.com", sensoryTenantId, deviceId).Connect();

ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
OauthService oauthService = new OauthService(config, credentialStore);

// Generate cryptographically random credentials
OauthClient oAuthClient = oauthService.GenerateCredentials();

// Register credentials with Sensory Cloud
var friendlyDeviceName = "Server 1";

// OAuth registration can take one of two paths, the insecure path that uses a shared secret between this device and your instance of Sensory Cloud
// or asymmetric public / private keypair registration.

// Path 1 --------
// Insecure authorization credential as configured on your instance of Sensory Cloud
var insecureSharedSecret = "password";
oauthService.Register(friendlyDeviceName, insecureSharedSecret);

// Path 2 --------
// Secure Public / private keypair registration using Portable.BouncyCastle and ScottBrady.IdentityModel

// Keypair generation happens out of band and long before a registration occurs. The below example shows how to generate and sign an enrollment JWT as a comprehensive example.
var keyPairGenerator = new Ed25519KeyPairGenerator();
keyPairGenerator.Init(new Ed25519KeyGenerationParameters(new SecureRandom()));
var keyPair = keyPairGenerator.GenerateKeyPair();

var privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;

// Public key is persisted in Sensory Cloud via the Management interface
var publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

var handler = new JsonWebTokenHandler();

var token = handler.CreateToken(new SecurityTokenDescriptor
{
    Issuer = "sensory-sdk",
    Audience = "sensory-cloud",
    Subject = new ClaimsIdentity(new[] { new Claim("clientId", oAuthClient.ClientId) }),

    // using JOSE algorithm "EdDSA"
    SigningCredentials = new SigningCredentials(
    new EdDsaSecurityKey(privateKey), ExtendedSecurityAlgorithms.EdDsa)
});

oauthService.Register(friendlyDeviceName, token);
```

## Creating a TokenManager

The TokenManger class handles requesting OAuth tokens when necessary.

```csharp
// Tenant ID granted by Sensory Inc.
string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
string deviceId = "a-hardware-identifier-unique-to-your-device";

// Configuration specific to your tenant
Config config = new Config("your-inference-server.com", sensoryTenantId, deviceId).Connect();

ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
IOAuthService oAuthService = new OAuthService(config, credentialStore);
ITokenManager tokenManager = new TokenManager(OAuthService);

// Obtain OAuth token from Sensory Cloud
string token = tokenManager.GetToken();

return tokenManager;

```

## Creating an AudioService

AudioService provides methods to stream audio to Sensory Cloud. It is recommended to only have 1 instance of AudioService
instantiated per Config. In most circumstances you will only ever have one Config, unless your app communicates with
multiple Sensory Cloud servers.

```csharp
public static AudioService GetAudioService()
{
    // Tenant ID granted by Sensory Inc.
    string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
    string deviceId = "a-hardware-identifier-unique-to-your-device";

    // Configuration specific to your tenant
    Config config = new Config("your-inference-server.com", sensoryTenantId, deviceId).Connect();

    ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
    IOAuthService oAuthService = new OAuthService(config, credentialStore);
    ITokenManager tokenManager = new TokenManager(OAuthService);

    return new AudioService(config, tokenManager);
}

```

### Obtaining Audio Models

Certain audio models are available to your application depending on the models that are configured for your instance of Sensory Cloud.
In order to determine which audio models are accessible to you, you can execute the below code.

```csharp
AudioService audioService = GetAudioService();
GetModelsResponse audioModels = audioService.GetModels();
```

Audio models contain the following properties:
* Name - the unique name tied to this model. Used when calling any other audio function.
* IsEnrollable - indicates if the model can be enrolled into. Models that are enrollable can be used in the CreateEnrollment function.
* ModelType - indicates the class of model and it's general function.
* FixedPhrase - for speech-based models only. Indicates if a specific phrase must be said.
* Samplerate - indicates the audio samplerate required by this model. Generally, the number will be 16000.
* IsLivenessSupported - indicates if this model supports liveness for enrollment and authentication. Liveness provides an added layer of security by requring a users to speak random digits.

### Enrolling with Audio

In order to enroll with audio, you must first ensure you have an enrollable model enabled for your Sensory Cloud instance. This can be obtained via the GetModels() request.
Enrolling with audio uses a bi-directional streaming pattern to allow immediate feedback to the user during enrollment. It is important to save the enrollmentId
in order to perform authentication against it in the future.

```csharp
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
            // * ModelPrompt - indicates what the user should say in order to proceed with the enrollment.
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
while (String.IsNullOrEmpty(enrollmentId))
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
```

### Authenticating with Audio

Authenticating with audio is similar to enrollment, except now you have an enrollmentId to pass into the function.

```csharp
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
            // * ModelPrompt - indicates what the user should say in order to proceed with the authentication.
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
```

### Audio events

Audio events are used to recognize specific words, phrases, or sounds. The below example waits for a single
event to be recognized and ends the stream.

```csharp
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

            // response.ResultId contains the specific sound that was heard in the case of combined models.
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

```

The below example outlines how to stream a wav file in chunks to Sensory Cloud and process all events before closing the stream.

```csharp
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
// Set to userId in your system who is making the request
string userId = "unique-user-id";
string modelName = "sound-16kHz-combined.trg";

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
```

### Transcription

Transcription is used to convert audio into text.

```csharp
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
```

## Creating a VideoService

VideoService provides methods to stream images to Sensory Cloud. It is recommended to only have 1 instance of VideoService
instantiated per Config. In most circumstances you will only ever have one Config, unless your app communicates with
multiple Sensory Cloud servers.

```csharp
// Tenant ID granted by Sensory Inc.
string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
string deviceId = "a-hardware-identifier-unique-to-your-device";

// Configuration specific to your tenant
Config config = new Config("your-inference-server.com", sensoryTenantId, deviceId).Connect();

ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
IOAuthService oAuthService = new OAuthService(config, credentialStore);
ITokenManager tokenManager = new TokenManager(OAuthService);

return new VideoService(config, tokenManager);
```

### Obtaining Video Models

Certain video models are available to your application depending on the models that are configured for your instance of Sensory Cloud.
In order to determine which video models are accessible to you, you can execute the below code.

```csharp
VideoService videoService = GetVideoService();
GetModelsResponse videoModels = videoService.GetModels();
```

Audio models contain the following properties:
* Name - the unique name tied to this model. Used when calling any other video function.
* IsEnrollable - indicates if the model can be enrolled into. Models that are enrollable can be used in the CreateEnrollment function.
* ModelType - indicates the class of model and it's general function.
* FixedObject - for recognition-based models only. Indicates if this model is built to recognize a specific object.
* IsLivenessSupported - indicates if this model supports liveness for enrollment and authentication. Liveness provides an added layer of security.

### Enrolling with Video

In order to enroll with video, you must first ensure you have an enrollable model enabled for your Sensory Cloud instance. This can be obtained via the GetModels() request.
Enrolling with video uses a call and response streaming pattern to allow immediate feedback to the user during enrollment. It is important to save the enrollmentId
in order to perform authentication against it in the future.

```csharp
VideoService videoService = GetVideoService();

// Set basic enrollment information
string enrollmentDescription = "My Enrollment";
string userId = "72f286b8-173f-436a-8869-6f7887789ee9";
string modelName = "wakeword-16kHz-open_sesame.ubm";
bool isLivenessEnabled = false;

// Stream is of type AsyncDuplexStreamingCall<CreateEnrollmentRequest, CreateEnrollmentResponse>
var stream = await videoService.StreamEnrollment(
    enrollmentDescription,
    userId,
    modelName,
    isLivenessEnabled
    );

// EnrollmentId if enrollment is successful
string enrollmentId = null;

// Start blocking while streaming up image data.
while (String.IsNullOrEmpty(enrollmentId))
{
    // Populate with real video data. This example creates an empty bytestring.
    var lin16VideoData = Google.Protobuf.ByteString.Empty;
    var message = new CreateEnrollmentRequest { ImageContent = lin16VideoData };

    try
    {
        await stream.RequestStream.WriteAsync(message);
    }
    catch (RpcException ex)
    {
        // Do something with the error. An error
    }

    // Wait for the response from the server
    try
    {
        await stream.ResponseStream.MoveNext();

        // Response contains information about the enrollment status.
        CreateEnrollmentResponse response = stream.ResponseStream.Current;
        enrollmentId = response.EnrollmentId;
    }
    catch (RpcException ex)
    {
        // Do something with the error - generally this is due to the server closing the stream
    }
}

await stream.RequestStream.CompleteAsync();
```

### Authenticating with Video

Authenticating with video is similar to enrollment, except now you have an enrollmentId to pass into the function.

```csharp
VideoService videoService = GetVideoService();

// Set basic enrollment information
string enrollmentId = "72f286b8-173f-436a-8869-6f7887789ee9";
bool isLivenessEnabled = false;

// Stream is of type AsyncDuplexStreamingCall<AuthenticateRequest, AuthenticateResponse>
var stream = await videoService.StreamAuthentication(
    enrollmentId,
    isLivenessEnabled
    );

// Indicates if the authentication is successful
bool authenticateSuccess = false;

// Start blocking while streaming up image data.
while (String.IsNullOrEmpty(enrollmentId))
{
    // Populate with real video data. This example creates an empty bytestring.
    var lin16VideoData = Google.Protobuf.ByteString.Empty;
    var message = new AuthenticateRequest { ImageContent = lin16VideoData };

    try
    {
        await stream.RequestStream.WriteAsync(message);
    }
    catch (RpcException ex)
    {
        // Do something with the error. An error
    }

    // Wait for the response from the server
    try
    {
        await stream.ResponseStream.MoveNext();

        // Response contains information about the enrollment status.
        AuthenticateResponse response = stream.ResponseStream.Current;
        authenticateSuccess = response.Success;
    }
    catch (RpcException ex)
    {
        // Do something with the error - generally this is due to the server closing the stream
    }
}

await stream.RequestStream.CompleteAsync();
```

### Video Liveness

Video Liveness allows one to send images to Sensory Cloud in order to determine if the subject is a live individual rather than a spoof, such as a paper mask or picture.

```csharp
VideoService videoService = GetVideoService();

// Set basic enrollment information
string userId = "72f286b8-173f-436a-8869-6f7887789ee9";
string modelName = "sound-16kHz-door_bell.trg";

// Stream is of type AsyncDuplexStreamingCall<ValidateRecognitionRequest, LivenessRecognitionResponse>
var stream = await videoService.StreamLivenessRecognition(
    userId,
    modelName
    );

// Indicates if the event is recognized
bool isAlive = false;

// Start blocking while streaming up image data.
// Loop until user is determined to be alive. Looping behavior is entirely up to your application logic. This is a simple example of one use case.
while (!isAlive)
{
    // Populate with real video data. This example creates an empty bytestring.
    var lin16VideoData = Google.Protobuf.ByteString.Empty;
    var message = new ValidateRecognitionRequest { ImageContent = lin16VideoData };

    try
    {
        await stream.RequestStream.WriteAsync(message);
    }
    catch (RpcException ex)
    {
        // Do something with the error. An error
    }

    // Wait for the response from the server
    try
    {
        await stream.ResponseStream.MoveNext();

        // Response contains information about the enrollment status.
        LivenessRecognitionResponse response = stream.ResponseStream.Current;
        isAlive = response.IsAlive;
    }
    catch (RpcException ex)
    {
        // Do something with the error - generally this is due to the server closing the stream
    }
}

await stream.RequestStream.CompleteAsync();
```

## Creating a ManagementService

The ManagementService is used to manage typical CRUD operations with Sensory Cloud, such as deleting enrollments or creating enrollment groups.
For more information on the specific functions of the ManagementService, please refer to the ManagementService.cs file located in the Services folder.

```csharp
// Tenant ID granted by Sensory Inc.
string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
string deviceId = "a-hardware-identifier-unique-to-your-device";

// Configuration specific to your tenant
Config config = new Config("your-inference-server.com", sensoryTenantId, deviceId).Connect();

ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
IOAuthService oAuthService = new OAuthService(config, credentialStore);
ITokenManager tokenManager = new TokenManager(OAuthService);

return new ManagementService(config, tokenManager);
```
