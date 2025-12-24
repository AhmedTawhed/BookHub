using BookHub.Api.Extensions;
using BookHub.Api.Middlewares;
using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookHubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookHubDbContext")));

builder.Services.AddBookHubIdentity(builder.Configuration);

builder.Services.AddBookHubServices();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddOpenApiDocumentation();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();