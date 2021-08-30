using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;
        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public virtual IEnumerable<DomainNotification> GetNotifications() => _notifications;

        public virtual bool HasNotifications() => GetNotifications().Any();

        public void Dispose() => _notifications = new List<DomainNotification>();
    }
}
