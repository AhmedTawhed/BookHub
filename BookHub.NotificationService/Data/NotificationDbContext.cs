using BookHub.NotificationService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookHub.NotificationService.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();
}
