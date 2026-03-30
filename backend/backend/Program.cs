var builder = WebApplication.CreateBuilder(args); 
var app = builder.Build(); 

// See https://aka.ms/new-console-template for more information
app.MapGet("/", () -> "Hello World");

app.Run(); 
