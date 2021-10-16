# About

Responder is a project that I felt would be a benefit to those attempting to maintain a high quality of consistency and standardization in Web APIs on new projects with tight deadlines. I made it for myself and for my team at work during my off-time, and decided that it could likely benefit others. I've been doing my best to maintain the standards set forth in the RFC documents regarding Problem Details, HTTP status codes, etc. If you feel that there is room for improvement regarding standardization, please let me know by logging an issue.

# Purpose

In ASP.NET Core, API controllers tend to do far more than one thing. Multiple developers will write API endpoints in vastly different ways. This library makes use of the builder pattern to place guiderails onto the developer to make the cleaner options more obvious. In addition to this, the models are set up so that any controller action can return the same object, and the details are correctly discovered by the ApiExplorer (used by Swashbuckle for Swagger documentation). Additionally, any response from a controller method can be deserialized into the same type of object.

# Usage

## Configuration and Dependency Injection

The builder object and its interface are `Responder` and `IResponder` respectively. To inject them into your controllers, register them with the service collection in the application startup:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddScoped<IResponder, Responder>();
    ...
}
```

There are no explicit dependencies to construct the Responder object. It will gain configuration options in future versions. If you have ideas for configuration options, please create an issue in this repository.

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

public async Task<ResponderResult<LoginSuccessModel>> Login(string email, string password) 
{
    var user = databaseService.GetUser(email);
    if (user == null) _responder.AddProblem(UnsuccessfulSignIn);
    else
    {
        var signInResult = await _signInManager.PasswordSignInAsync(user, password);

        if (signInResult.IsLockedOut) _responder.AddProblem(LockedOut);
        else if (signInResult.IsNotAllowed) _responder.AddProblem(NotAllowed);
        else if (!signInResult.Succeeded) _responder.AddProblem(UnsuccessfulSignIn);
    }

    // No type parameter needed if no body content needs to be sent
    return _responder.Build(this);
}
```

## Consuming responses

Responder is not just for the server, but also for the client! Digestion of responses created using Responder is simple. If there may be content, deserialize the content into Response<T>, and if there won't be content, use Response.

```csharp
var client = new HttpClient();
var result = await client.GetAsync("https://someurl/withcontent");
Response<Model> response = JsonConvert.DeserializeObject<Response<Model>>(await result.Content.ReadAsStringAsync());
if (response.IsSuccessful)
{
    // Do stuff
}
else
{
    // Do other stuff
}
```

The `IsSuccessful` property is calculated simply by checking to see if the `Problem` property is null.

## The JSON itself

When using the Responder, response content will look considerably different depending on whether there was a problem. This is because, when serializing, problem responses must follow the appropriate standards laid out in RFC7807. However, this is not an issue when deserializing; the Response object will neatly fold all of those top-level fields into its `Problem` property. Responses may also contain extension data, but if there was a problem, all extension data will deserialize into the `Problem` property's `Extensions` rather than those belonging to the Response.
