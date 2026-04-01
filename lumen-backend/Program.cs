using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddDbContext<LearningResourceDb>(opt => opt.UseInMemoryDatabase("Lumen"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); 
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddOpenApiDocument(config => {
    config.DocumentName= "LumenAPI";
    config.Title = "LumenAPI v1"; 
    config.Version = "v1"; 
    }); 
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

//GET functions for the Learning Resources
app.MapGet("api/learningresources/{id}",async (int id, LearningResourceDb db) => 
    await db.LearningResources.FindAsync(id)
    is LearningResource lr   
                ? Results.Ok(lr)
                : Results.NotFound()); 
app.MapGet("api/learningresources",async (LearningResourceDb db)=> 
                  Results.Ok(await db.LearningResources.ToListAsync())); 
//POST function for creating LearningResources  
app.MapPost("api/learningresources", async (LearningResourceDb db, [FromBody] JsonObject json ) =>
    {
      var lr = JsonSerializer.Deserialize<LearningResource>(json.ToJsonString());
      if (lr is null)
      {
          return Results.BadRequest();
      }

      db.LearningResources.Add(lr); 
      await db.SaveChangesAsync(); 

      return Results.Created($"api/learningresources/{lr.Id}", lr); 

    });
app.MapPut("api/learningresources/{id}", async (LearningResourceDb db, [FromBody] JsonObject json, int id ) =>
    {
      var learningResource = await db.LearningResources.FindAsync(id); 
      
      if (learningResource is null ){

       return Results.NotFound(); 
       }

       var lr = JsonSerializer.Deserialize<LearningResource>(json.ToJsonString());
       if (lr is null)
       {
           return Results.BadRequest();
       }

       learningResource.Title = lr.Title; 
       learningResource.Description = lr.Description; 
      
       await db.SaveChangesAsync(); 
       return Results.NoContent(); 
    });
//DELETE to remove LearningResource from the database 
app.MapDelete("api/learningresources/{id}", async (LearningResourceDb db, int id ) =>
    {
      if (await db.LearningResources.FindAsync(id) is LearningResource lr)
      {
        db.LearningResources.Remove(lr); 
        await db.SaveChangesAsync(); 
        return Results.NoContent(); 
      }

      return Results.NotFound(); 
    }); 
app.Run(); 
