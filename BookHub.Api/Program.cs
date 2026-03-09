using BookHub.Api.Extensions;
using BookHub.Api.Middlewares;
using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<BookHubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookHubDbContext")));

builder.Services.AddBookHubIdentity(builder.Configuration);
builder.Services.AddBookHubServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocumentation();
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("BookHubPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("BookHubPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();