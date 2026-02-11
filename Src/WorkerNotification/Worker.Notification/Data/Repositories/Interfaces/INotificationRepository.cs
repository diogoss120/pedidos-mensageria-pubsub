using WorkerNotification.Data.Entities;

namespace WorkerNotification.Data.Repositories.Interfaces;

public interface INotificationRepository
{
    Task CreateAsync(Notification notification);
}
