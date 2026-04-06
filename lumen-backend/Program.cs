using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LumenAPI.Identity.Database;
using LumenAPI.Database;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using LumenAPI.Identity.Routing;

var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); 
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddOpenApiDocument(config => {
    config.DocumentName= "LumenAPI";
    config.Title = "LumenAPI v1"; 
    config.Version = "v1"; 
    }); 
builder.Services.AddEndpointsApiExplorer(); 
// Identity and Authentication
builder.Services.AddAuthentication()
    .AddCookie(IdentityConstants.ApplicationScheme);
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});
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
app.UseCors("AllowFrontend");
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

app.MapControllers();
app.MapCustomIdentityApi<UserAccount>(); // Map Identity API endpoints
app.Run(); 
