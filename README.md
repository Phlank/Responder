- [About](#about)
- [Purpose](#purpose)
- [Usage](#usage)
  - [Dependency Injection](#dependency-injection)
  - [Configuration](#configuration)
  - [Returning data](#returning-data)
  - [Problems](#problems)
  - [Consuming responses](#consuming-responses)
  - [The JSON itself](#the-json-itself)

# About

Responder is a project that I felt would be a benefit to those attempting to maintain a high quality of consistency and standardization in Web APIs on new projects with tight deadlines. I made it for myself and for my team at work during my off-time, and decided that it could likely benefit others. I've been doing my best to maintain the standards set forth in the RFC documents regarding Problem Details, HTTP status codes, etc. If you feel that there is room for improvement regarding standardization, please let me know by logging an issue on Github.

# Purpose

In ASP.NET Core, API controllers tend to do far more than one thing. Multiple developers will write API endpoints in vastly different ways. This library makes use of the builder pattern to place guiderails onto the developer to make the cleaner options more obvious. In addition to this, the models are set up so that any controller action can return the same object, and the details are correctly discovered by the ApiExplorer (used by Swashbuckle for Swagger documentation). Additionally, any response from a controller method can be deserialized into the same type of object and treated equally.

# Usage

## Dependency Injection

The builder object and its interface are `Responder` and `IResponder` respectively. To inject them into your controllers, register them with the service collection in the application startup:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddScoped<IResponder, Responder>();
    ...
}
```

Once the IResponder is registered in the service collection, you will be able to inject it into your controllers.

```csharp
[ApiController]
public class MyController : Controller
{
    private readonly IResponder _responder;

    public MyController(IResponder responder)
    {
        _responder = responder;
    }
}
```

## Configuration

In Startup, register the options for the Responder using the IServiceCollection.Configure method.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddScoped<IResponder, Responder>();
    services.Configure<ResponderOptions>(o => {
        o.IncludeTraceIdOnErrors = true; // Will include the HttpContext's trace identifier in a Problem's extensions. True by default.
    });
    ...
}
```

## Returning data

In order to return data from a controller action, use the `AddContent` and `Build<T>` methods. Have your controller action return the ResponderResult type.

```csharp
public ResponderResult<AccountApiModel> GetAccount(int id) {
    var account = databaseService.GetSomeData(id);
    var model = mappingService.Map<AccountApiModel>(account);

    return _responder.AddContent(model).Build<AccountApiModel>(this);
}
```

The `Build<T>` method takes a ControllerBase instance as an argument. This is used for accessing the HttpContext of the controller. The `Build` method is similar, though the ResponderResult that is returned here does not contain data but still may contain extensions.

## Problems

Dealing with problems is very simple with Responder, and the builder pattern does an excellent job at making code smells more obvious and easier to deodorize. Use the `AddProblem` method to add a Problem item to the IResponder. You can add a Problem, ProblemDetail, Exception, or provide arguments to construct a Problem instance. Multiple problems can be added to the IResponder, and it will combine them when the result is built.

```csharp
private Problem UnsuccessfulSignIn = new Problem(...);
private Problem LockedOut = new Problem(...);
private Problem NotAllowed = new Problem(...);

public async ResponderResult Login(string email, string password)
{
    var user = databaseService.GetUser(email);
    if (user == null) _responder.AddProblem(UnsuccessfulSignIn);
    else
    {
        var signInResult = await _signInManager.PasswordSignIn(user, password);

        if (signInResult.IsLockedOut) _responder.AddProblem(LockedOut);
        else if (signInResult.IsNotAllowed) _responder.AddProblem(NotAllowed);
        else if (!signInResult.Succeeded) _responder.AddProblem(UnsuccessfulSignIn);
    }

    // No type parameter needed if no body content needs to be sent
    return _responder.Build(this);
}
```

## Consuming responses

Responder is not just for the server, but also for the client! Digestion of responses created using Responder is simple. If there may be content, deserialize the content into Response<T>, and if there won't be content, use Response. There are also custom JsonConverters for both System.Text.Json and Json.NET. Either provide them to the global serializer settings or provide them as variables into the deserialization call itself. For Newtonsoft, use `NewtonsoftResponseConverter` and `NewtonsoftResponseConverter<T>`. For System.Text.Json, use `SystemTextJsonResponseConverter` and `SystemTextJsonResponseConverter<T>`.

```csharp
var client = new HttpClient();
var result = await client.GetAsync("https://someurl/withcontent");
var content = await result.Content.ReadAsStringAsync();

Response<T> response;

// Newtonsoft, without settings
response = JsonConvert.DeserializeObject<Response<T>>(content, new NewtonsoftResponseConverter<T>());

// Newtonsoft, with settings
var settings = new JsonSerializerSettings();
settings.Converters.Add(new NewtonsoftResponseConverter<T>());
response = JsonConvert.DeserializeObject<Response<T>>(content, settings);

// System.Text.Json only correctly deserializes using settings.
var settings = new JsonSerializerOptions();
settings.Converters.Add(new SystemTextJsonResponseConverter<T>());
response = JsonSerializer.Deserialize<Response<T>>(content, settings);

if (response.IsSuccessful) 
{
    ...
}
else 
{
    ...
}
```

The `IsSuccessful` property is calculated simply by checking to see if the `Problem` property is null.

A note must be made about System.Text.Json; this library depends on version 5.0.0 because it is the first version to include default value handling. However, all versions of System.Text.Json currently supported are compatible with any version of .NET Core 3.1 and above. This will also include System.Text.Json 6.0.0, which this project will upgrade to when it is released.

## The JSON itself

When using the Responder, response content will look considerably different depending on whether there was a problem. This is because, when serializing, problem responses must follow the appropriate standards laid out in RFC7807. However, this is not an issue when deserializing; included converters will neatly fold all of those top-level fields into its `Problem` property. Responses may also contain extension data, but if there was a problem, all extension data will deserialize into the `Problem`'s `Extensions` property rather than those belonging to the Response.

For a given model,

```csharp
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureHigh { get; set; }
    public int TemperatureLow { get; set; }
    public string Summary { get; set; }
}
```

the following example Swagger response will be created by Swashbuckle.

```json
{
  "isSuccessful": true,
  "data": {
    "date": "2021-10-18T03:57:11.541Z",
    "temperatureHigh": 0,
    "temperatureLow": 0,
    "summary": "string"
  },
  "additionalProp1": "string",
  "additionalProp2": "string",
  "additionalProp3": "string"
}
```

If no data is provided by the server, then the `data` field will be ommitted.

The following JSON can be expected for Problems:

```json
{
  "status": 400,
  "title": "string",
  "detail": "string",
  "type": "string",
  "instance": "string",
  "additionalProp1": "string",
  "additionalProp2": "string",
  "additionalProp3": "string"
}
```