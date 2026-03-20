using BookHub.Contracts;
using BookHub.NotificationService.Data;
using BookHub.NotificationService.Entities;
using MassTransit;
using System.Text.Json;

namespace BookHub.NotificationService.Consumers;

public class BookAddedConsumer : IConsumer<BookAddedEvent>
{
    private readonly NotificationDbContext _db;
    private readonly ILogger<BookAddedConsumer> _logger;

    public BookAddedConsumer(NotificationDbContext db, ILogger<BookAddedConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BookAddedEvent> context)
    {
        var evt = context.Message;
        _logger.LogInformation("New book added: '{Title}' by {Author}", evt.Title, evt.Author);

        _db.NotificationLogs.Add(new NotificationLog
        {
            EventType = nameof(BookAddedEvent),
            Payload = JsonSerializer.Serialize(evt),
            ReceivedAt = DateTime.UtcNow,
            Status = "Processed"
        });

        await _db.SaveChangesAsync();
    }
}
