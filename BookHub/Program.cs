using BookHub.Extensions;
using BookHub.Infrastructure.Data;
using BookHub.Infrastructure.Data.Seeding;
using BookHub.Middlewares;
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    await IdentitySeed.Seed(scope.ServiceProvider);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();