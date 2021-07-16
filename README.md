- [Configuration](#configuration)
  - [Options](#options)
    - [Overriding the default InvalidModelStateResponseFactory](#overriding-the-default-invalidmodelstateresponsefactory)

## Configuration

To register the IApiResponseBuilder in your services, add `Phlank.ApiModeling` to your usings and add the following to your `ConfigureServices(...)` method in `Startup.cs`:

```
services.ConfigureApiResponseBuilder();
```

This will make the IApiResponseBuilder injectable in your project.

### Options

#### Overriding the default InvalidModelStateResponseFactory

To replace the default InvalidModelStateResponseFactory used by ASP.NET Core, add the following option to your configuration.

```
services.ConfigureApiResponseBuilder(options => 
{
    options.UseApiResponseForModelStateErrors();
});
```

This will replace the usual invalid model state response to the ApiResponse.

The default response

```
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

will become 

```
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

This option will not completely reconfigure any `ApiBehaviorOptions` already configured in the `IServiceCollection`, but will instead override only the `InvalidModelStateResponseFactory`. Therefore, any usage of this option should be done after configuring the `ApiBehaviorOptions`. However, if no options have been configured, then a new `ApiBehaviorOptions` will be configured.