using System.Net.Mime;
using System.Text;
using DrKnuffelBackEnd.Repositories.Progress;
using DrKnuffelBackEnd.Repositories.Step;
using DrKnuffelBackEnd.Repositories.UserData;
using DrKnuffelBackEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register MVC controllers for handling HTTP requests.
builder.Services.AddControllers();

// Retrieve the SQL connection string from configuration.
var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

// Register OpenAPI/Swagger for API documentation and testing.
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dr Knuffel's backend",
        Version = "v1"
    });
});
builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);
builder.Services.AddAuthorization();
// Register ASP.NET Core Identity with Dapper stores for user authentication and management.
// Configures password and user requirements.
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 10;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
    })
    .AddRoles<IdentityRole>()
    .AddDapperStores(options => { options.ConnectionString = sqlConnectionString; });

// Register IHttpContextAccessor for accessing HTTP context in services (e.g., to get current user info).
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

builder.Services.AddTransient<IStepRepo, SQLStepRepo>(o => new SQLStepRepo(sqlConnectionString!));
builder.Services.AddTransient<IExtraUserData, SQLExtraUserData>(o => new SQLExtraUserData(sqlConnectionString!));
builder.Services.AddTransient<IProgressRepo, SQLProgressRepo>(o => new SQLProgressRepo(sqlConnectionString!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DrKnuffelBackend API v1");
        options.RoutePrefix = "swagger"; // Access at /swagger
        options.CacheLifetime = TimeSpan.Zero; // Disable caching for development

        // Inject a warning in the Swagger UI if the SQL connection string is missing
        if (!sqlConnectionStringFound)
            options.HeadContent = "<h1 align=\"center\">❌ SqlConnectionString not found ❌</h1>";
    });
    //Was getting bored of the error page default in dev environment
    app.MapGet("/", context =>
    {
        var currentHealthMessage = @$"<!doctype html>
<html>
    <head><title>miniHTML</title></head>
    <body>
        <h1>Dev mode</h1>
        <p>The time on the server is {DateTime.Now:O}</p>
<a href='/swagger/index.html'>Swagger docu</a>
    </body>
</html>";
        context.Response.ContentType = MediaTypeNames.Text.Html;
        context.Response.ContentLength = Encoding.UTF8.GetByteCount(currentHealthMessage);
        return context.Response.WriteAsync(currentHealthMessage);
    });
}

app.UseHttpsRedirection();

// Enable authorization middleware.
app.UseAuthorization();

// Register Identity endpoints for account management (register, login, etc.) under /account.
// 👇 uncomment the following line to enable Identity API endpoints to use authentication/authorization
app.MapGroup("/account").MapIdentityApi<IdentityUser>().WithTags("Account");

app.MapControllers().RequireAuthorization();


app.Run();

public partial class Program { }