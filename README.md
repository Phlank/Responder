- [About](#about)
- [Usage](#usage)
  - [Basic usage](#basic-usage)
      - [Response without content](#response-without-content)
      - [Response with content](#response-with-content)
  - [Warnings and errors](#warnings-and-errors)
- [Configuration](#configuration)
  - [Options](#options)
    - [Overriding the default InvalidModelStateResponseFactory](#overriding-the-default-invalidmodelstateresponsefactory)
      - [Default InvalidModelStateResponse](#default-invalidmodelstateresponse)
      - [New InvalidModelStateResponse](#new-invalidmodelstateresponse)

# About

This project is a way to return consistent objects from an ASP<area>.NET Core Web API for digestion in other systems. It is designed to be clean and make reading through controller methods much simpler.

# Usage

Add `Phlank.ApiModeling` to your usings and inject the `IApiResponseBuilder` into your class.

## Basic usage

There are two similar classes returned by the `IApiResponseBuilder`:

- The `ApiResponse` class has three properties: `Warnings`, `Errors`, and `Success`.
- The `ApiResponse<TContent>` class has four properties: `Warnings`, `Errors`, `Success`, and `Content`.


The `IApiResponseBuilder` can create either of these objects depending on whether content is specified in the `Build()` method.

To add warnings and errors to the response, the methods `WithError(ApiError error)`, `WithErrors(IEnumerable<ApiError> errors)`, `WithWarning(ApiWarning warning)`, and `WithWarnings(IEnumerable<ApiWarning> warnings)` are available. Calling any of these methods will concatenate onto the existing warnings or errors placed onto the `IApiResponseBuilder`. If no warnings or errors are added, the corresponding property will be `null` rather than an empty array. 

The `Success` property is true when the `Errors` property is null or empty.

#### Response without content

```C#
public async Task<ApiResponse> Login(LoginRequest request)
{
    var signInResult = await _signInManager.PasswordSignInAsync(
        request.UserName,
        request.Password, ...);

    if (signInResult.IsLockedOut) _responseBuilder.WithError(LockedOutError);
    else if (signInResult.IsNotAllowed) _responseBuilder.WithError(NotAllowedError);
    else if (!signInResult.Succeeded) _responseBuilder.WithError(UnsuccessfulSignInError);

    return _responseBuilder.Build();
}
```

#### Response with content

```C#
public ApiResponse<WeatherForecast> GetForecast(WeatherForecastRequest request)
{
    if (request.DaysAhead > 10) _responseBuilder.WithWarning(NoConfidenceForecastWarning);
    else if (request.DaysAhead > 7) _responseBuilder.WithWarning(LowConfidenceForecastWarning);

    var content = _weatherService.GetRandomWeatherForecast();
    return _responseBuilder.Build(content);
}
```

## Warnings and errors

Whenever an `ApiError` object is added to the builder, the resulting `Success` of the built `ApiResponse` object will always be false. Success is determined by a lack of errors. If messages need to be conveyed to the user while also indicating operational success, using an `ApiWarning` is recommended. The two classes are similar and share most of their properties.

- `ApiError` has three properties: `Fields`, `Code`, and `Message`.
- `ApiWarning` has four properties: `Fields`, `Code`, `Message`, and `Severity`.

`Fields` is an `IEnumerable<string>` and should list the fields that are causing the particular error or warning. If no fields can be specifically linked to the cause, leave this property null.

`Code` is a `string` which should be unique to the event regarding the error or warning. For example, if a user failed to login because of an invalid username or password, I would set `Code` differently than if their account was locked out.

`Message` is a `string` and is similar to `Code`, but it can be more descriptive. In general, `Message` should include some type of action that a user could take to remedy the cause of the error or warning.

`Severity` is an enumerable type with three possible values: Critical, High, and Low. Because errors imply failure to execute requested actions, they do not have severity (all errors are critical in nature). Warnings, however, aid users in understanding some action they may need to take to avoid errors in the future. Warnings could also be a message to a user about the state of the content given by the API.

> When ASP<area>.NET Core produces a ContentResult from your `ApiResponse`, Severity will be serialized as a `string` if you are using either `Newtonsoft.Json` or `System.Text.Json`. However, if you are using a different serializing library, it will be serialized as an `int`.
>
> - Severity.Critical = 1
> - Severity.High = 2
> - Severity.Low = 3

# Configuration

To register the IApiResponseBuilder in your services, add `Phlank.ApiModeling.Extensions` to your usings and add the following to your `ConfigureServices(...)` method in `Startup.cs`:

```C#
public void ConfigureServices(IServiceCollection services)
{
    services.ConfigureApiResponseBuilder();
}
```

This will make the IApiResponseBuilder injectable in your project.

## Options

### Overriding the default InvalidModelStateResponseFactory

To replace the default InvalidModelStateResponseFactory used by ASP<area>.NET Core, add the following option to your configuration.

```C#
services.ConfigureApiResponseBuilder(options => 
{
    options.UseApiResponseForModelStateErrors();
});
```

This will replace the default invalid model state response with the ApiResponse.

#### Default InvalidModelStateResponse

```Json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "|fb6d72a7-451d72a7d6708e4e.",
    "errors": {
        "TemperatureUnits": [
            "Acceptable values for TemperatureFormat are C, F, and K."
        ]
    }
}
```

#### New InvalidModelStateResponse

```Json
{
    "Success": false,
    "Errors": [
        {
            "Fields": [
                "TemperatureUnits"
            ],
            "Code": "InvalidField",
            "Message": "Acceptable values for TemperatureFormat are C, F, and K."
        }
    ],
    "Warnings": null
}
```

This option will not completely reconfigure any `ApiBehaviorOptions` already configured in the `IServiceCollection`, but will instead override only the `InvalidModelStateResponseFactory`. Therefore, any usage of this option should be done after configuring the `ApiBehaviorOptions`. If no options have been configured, a new `ApiBehaviorOptions` will be configured with the `IServiceCollection`.
