using MediatR;
using Microsoft.AspNetCore.Mvc;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using System.Threading.Tasks;

namespace RssStore.WebApp.MVC.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly DomainNotificationHandler _notifications;

        public SummaryViewComponent(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await Task.FromResult(_notifications.GetNotifications());

            foreach (var notication in notifications)
                ViewData.ModelState.AddModelError(string.Empty, notication.Value);

            return View();
        }
    }
}
