using BookHub.Contracts;
using BookHub.NotificationService.Data;
using BookHub.NotificationService.Entities;
using MassTransit;
using System.Text.Json;

namespace BookHub.NotificationService.Consumers;

public class ReviewAddedConsumer : IConsumer<ReviewAddedEvent>
{
    private readonly NotificationDbContext _db;
    private readonly ILogger<ReviewAddedConsumer> _logger;

    public ReviewAddedConsumer(NotificationDbContext db, ILogger<ReviewAddedConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReviewAddedEvent> context)
    {
        var evt = context.Message;
        _logger.LogInformation("New review on BookId {BookId} by UserId {UserId} — Rating: {Rating}", evt.BookId, evt.UserId, evt.Rating);

        _db.NotificationLogs.Add(new NotificationLog
        {
            EventType = nameof(ReviewAddedEvent),
            Payload = JsonSerializer.Serialize(evt),
            ReceivedAt = DateTime.UtcNow,
            Status = "Processed"
        });

        await _db.SaveChangesAsync();
    }
}
