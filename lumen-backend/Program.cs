using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LumenAPI.Identity.Database;
using LumenAPI.Database;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using LumenAPI.Identity.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Keycloak;


var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); 
builder.Services.AddEndpointsApiExplorer(); 
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // ✅ Allow frontend origin
                  .AllowAnyMethod()                   // ✅ Allow all HTTP methods
                  .AllowAnyHeader();                  // ✅ Allow all headers
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"{builder.Configuration["Keycloak:BaseUrl"]}/realms/{builder.Configuration["Keycloak:Realm"]}",

        ValidateAudience = true,
        ValidAudience = "account",

        ValidateIssuerSigningKey = true,
        ValidateLifetime = false,

        IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
        {
            var client = new HttpClient();
            var keyUri = $"{parameters.ValidIssuer}/protocol/openid-connect/certs";
            var response = client.GetAsync(keyUri).Result;
            var keys = new JsonWebKeySet(response.Content.ReadAsStringAsync().Result);

            return keys.GetSigningKeys();
        }
    };
    options.RequireHttpsMetadata = false; // Only for develop
    options.SaveToken = true;
});
builder.Services.AddHttpClient();
builder.Services.AddScoped<KeycloakAuthService>();
// Identity services
builder.Services.AddIdentityCore<UserAccount>()
    .AddEntityFrameworkStores<UserAccountDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.AddDbContext<UserAccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddDbContext<LearningResourceDb>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("LearningResourceConnection")));

builder.Services.AddControllers(); // Add controllers for API endpoints
var app = builder.Build(); 
if(app.Environment.IsDevelopment())
{
  app.UseOpenApi(); 
  app.UseSwaggerUi(config =>
      {
          config.DocumentTitle = "LumenAPI"; 
          config.Path = "/swagger"; 
          config.DocumentPath = "/swagger/{documentName}/swagger.json"; 
          config.DocExpansion = "list"; 
      });

}

// See https://aka.ms/new-console-tempa te for more information
app.MapGet("api/hello", () => Results.Json(new { message = "Hello World" }));

app.MapCustomIdentityApi<UserAccount>(); // Map Identity API endpoints
app.UseCors("AllowFrontend"); // ✅ Must be before app.UseAuthorization() and app.MapControllers()

app.UseAuthorization();
app.MapControllers();
app.Run();
app.Run(); 
