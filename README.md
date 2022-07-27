# C Sharp SDK

Sensory Cloud C# .Net SDK is build upon .Net Standard 2.0 in order to maximize [compatibility](https://dotnet.microsoft.com/platform/dotnet-standard#versions).

## General Information

Before getting started, you must spin up a Sensory Cloud inference server or have Sensory spin one up for you. You must also have the following pieces of information:
* Your inference server URL
* Your Sensory Tenant ID (UUID)
* Your configured secret key used to register OAuth clients

## Checking Server Health

It's important to check the health of your Sensory Inference server. You can do so via the following:
[HealthServiceExample.cs](Test/Examples/HealthServiceExample.cs)

## Secure Credential Store

ISecureCredential is an interface that store and serves your OAuth credentials (clientId and clientSecret).
ISecureCredential must be implemented by you and the credentials should be persisted in a secure manner, such as in an encrypted database.
OAuth credentials should be generated one time per unique machine.

A crude example of ISecureCredential can be found in [SecureCredentialStoreExample.cs](Test/Examples/AudioServiceExample.cs)

## Initializing The SDK

Initialization is very simple. Configuration can be provided via a struct or .ini file.

An example can be found in [InitializationExample.cs](Test/Examples/InitializationExample.cs)

If you are using the more secure token-based enrollment, follow the JWT signing example found in [OauthSeviceExample.cs](Test/Examples/AudioServiceExample.cs)

## Renewing Credentials

If you are evaluting Sensory Cloud as part of a Proof of Concept then likely the token granted to you will expire.
Should the secret expire, you can request a new from from Sensory. You will need to renew your credential via the OAuth service.
You can always renew your secret before it has expired to ensure you have 0 downtime.

```
oauthService.RenewDeviceCredential(newToken);
```

## Creating a TokenManager

The TokenManger class handles requesting OAuth tokens when necessary.

An example can be found in [TokenManagerExample.cs](Test/Examples/TokenManagerExample.cs)

## Creating an AudioService

AudioService provides methods to stream audio to Sensory Cloud. It is recommended to only have 1 instance of AudioService
instantiated per Config. In most circumstances you will only ever have one Config, unless your app communicates with
multiple Sensory Cloud servers.

An example can be found in [AudioServiceExaple.cs GetAudioService()](Test/Examples/AudioServiceExaple.cs)

### Obtaining Audio Models

Certain audio models are available to your application depending on the models that are configured for your instance of Sensory Cloud.
In order to determine which audio models are accessible to you, you can execute the below code.

An example can be found in [AudioServiceExaple.cs GetAudioModels()](Test/Examples/AudioServiceExaple.cs)

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

An example can be found in [AudioServiceExaple.cs EnrollAudio()](Test/Examples/AudioServiceExaple.cs)

### Authenticating with Audio

Authenticating with audio is similar to enrollment, except now you have an enrollmentId to pass into the function.

An example can be found in [AudioServiceExaple.cs AuthenticateAudio()](Test/Examples/AudioServiceExaple.cs)

### Audio events

Audio events are used to recognize specific words, phrases, or sounds.

The below example waits for a single event to be recognized and ends the stream.
[AudioServiceExaple.cs AudioEvent()](Test/Examples/AudioServiceExaple.cs)

The below example outlines how to stream a wav file in chunks to Sensory Cloud and process all events before closing the stream.
[AudioSoundRecognizerExample.cs](Test/Examples/AudioSoundRecognizerExample.cs)
```

## CreateEnrolledEvent

You can enroll your own event into the Sensory cloud system. The process is similar to biometric enrollment where you must
play a sound or speak a particular phrase 4 or more times. This is usefull for recognizing sounds that are not offered by Sensory Cloud.

An example can be found in [AudioServiceExaple.cs CreateEnrolledEvent()](Test/Examples/CreateEnrolledEvent.cs)

### ValidateEnrolledEvent

Once you've created an enroled event, you can listen for that event (or groups of events) by calling
the ValidateEnrolledEvent function.

An example can be found in [AudioServiceExaple.cs ValidateEnrolledEvent()](Test/Examples/ValidateEnrolledEvent.cs)

### Transcription

Transcription is used to convert audio into text.

An example can be found in [AudioServiceExaple.cs AudioTranscribe()](Test/Examples/AudioTranscribe.cs)

## Creating a VideoService

VideoService provides methods to stream images to Sensory Cloud. It is recommended to only have 1 instance of VideoService
instantiated per Config. In most circumstances you will only ever have one Config, unless your app communicates with
multiple Sensory Cloud servers.

An example can be found in [VideoServiceExample.cs GetVideoService()](Test/Examples/VideoServiceExample.cs)

### Obtaining Video Models

Certain video models are available to your application depending on the models that are configured for your instance of Sensory Cloud.
In order to determine which video models are accessible to you, you can execute the below code.

An example can be found in [VideoServiceExample.cs GetVideoModels()](Test/Examples/VideoServiceExample.cs)

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

An example can be found in [VideoServiceExample.cs EnrollVideo()](Test/Examples/VideoServiceExample.cs)

### Authenticating with Video

Authenticating with video is similar to enrollment, except now you have an enrollmentId to pass into the function.

An example can be found in [VideoServiceExample.cs AuthenticateVideo()](Test/Examples/VideoServiceExample.cs)

### Video Liveness

Video Liveness allows one to send images to Sensory Cloud in order to determine if the subject is a live individual rather than a spoof, such as a paper mask or picture.

An example can be found in [VideoServiceExample.cs VideoLiveness()](Test/Examples/VideoServiceExample.cs)

## Creating a ManagementService

The ManagementService is used to manage typical CRUD operations with Sensory Cloud, such as deleting enrollments or creating enrollment groups.
For more information on the specific functions of the ManagementService, please refer to the ManagementService.cs file located in the Services folder.

An example can be found in [ManagementServiceExample.cs](Test/Examples/ManagementServiceExample.cs)
