using BookHub.Contracts;
using BookHub.NotificationService.Data;
using BookHub.NotificationService.Entities;
using MassTransit;
using System.Text.Json;

namespace BookHub.NotificationService.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly NotificationDbContext _db;
    private readonly ILogger<UserRegisteredConsumer> _logger;

    public UserRegisteredConsumer(NotificationDbContext db, ILogger<UserRegisteredConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var evt = context.Message;
        _logger.LogInformation("New user registered: {UserName} ({Email})", evt.UserName, evt.Email);

        _db.NotificationLogs.Add(new NotificationLog
        {
            EventType = nameof(UserRegisteredEvent),
            Payload = JsonSerializer.Serialize(evt),
            ReceivedAt = DateTime.UtcNow,
            Status = "Processed"
        });

        await _db.SaveChangesAsync();
    }
}
