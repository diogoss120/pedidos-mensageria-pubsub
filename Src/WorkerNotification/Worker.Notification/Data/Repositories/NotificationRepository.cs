using MongoDB.Driver;
using WorkerNotification.Data.Entities;
using WorkerNotification.Data.Repositories.Interfaces;

namespace WorkerNotification.Data.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly IMongoCollection<Notification> _notificationsCollection;

    public NotificationRepository(IMongoDatabase database)
    {
        _notificationsCollection = database.GetCollection<Notification>("Notifications");
    }

    public async Task CreateAsync(Notification notification)
    {
        await _notificationsCollection.InsertOneAsync(notification);
    }
}
