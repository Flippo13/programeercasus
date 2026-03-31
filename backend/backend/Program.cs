using Microsoft.EntityFrameworkCore;  
var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddDbContext<LearningResourceDb>(opt => opt.UseInMemoryDatabase("Lumen"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); 
builder.Services.AddEndpointAPIExplorer(); 
buildder.Services.AddOpenAPIDocument(config => {
    config.DocumentName("LumenAPI");
    config.Title = "LumenAPI v1"; 
    config.Version = "v1"; 
    }); 
var app = builder.Build(); 
if(app.environment.IsDevelopment())
{
  app.useOpenApi(); 
  app.UseSwaggerUi(config =>
      {
          config.DocumentTitle = "LumenAPI"; 
          config.Path = "/swagger"; 
          config.DocumentPath = "/swagger/{documentName}/swagger.json"; 
          config.DocExpansion = "list"; 
      });
}


// See https://aka.ms/new-console-tempa te for more information
app.MapGet("/hello", () => "Hello World");

//GET functions for the Learning Resources
app.MapGet("/learningresources/{id}",async (int id, LearningResourceDB db) => 
    await db.LearningResources.FindAsync(id)
    is LearningResource Lr   
                ? Results.Ok(lr)
                : Results.NotFound()); 
app.MapGet("/learningresources",async (LearningResourceDb db)=> 
                  await db.LearningResources.ToListAsync()); 
//POST function for creating LearningResources  
app.MapPost("/learningresources", async (LearningResourcesDb db, LearningResource lr ) =>
    {
      db.LearningResources.Add(Lr); 
      await db.SaveChangesAsync(); 

      return Results.Created($"learningresources/{lr.id}", lr); 

    });
app.MapPut("/learningresources/{id}", async (LearningResourcesDb db, LearningResource lr, int id ) =>
    {
      var learningResource = await db.LearningResources.FindAsync(id); 
      
      if (learningResource is null ){

       return Results.NotFound(); 
       }

       learningResource.Title = lr.Title; 
       learningResource.Description = lr.Description; 
      
       await db.SaveChangesAsync(); 
       return Results.NoContent(); 
    });
//DELETE to remove LearningResource from the database 
app.MapDelete("learningresources/{id}", async (LearningResourcesDb db, int id ) =>
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
