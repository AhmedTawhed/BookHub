using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BookHubDbContext>();
            db.Database.Migrate();
        }
    }
}
