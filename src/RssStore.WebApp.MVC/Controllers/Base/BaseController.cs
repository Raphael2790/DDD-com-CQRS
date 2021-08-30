using MediatR;
using Microsoft.AspNetCore.Mvc;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RssStore.WebApp.MVC.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediatorHandler;
        public BaseController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
        }

        protected bool ValidOperation() => !_notifications.HasNotifications();

        protected void NotifyError(string code, string message) => _mediatorHandler.PublishNotification(new DomainNotification(code, message));

        protected IEnumerable<string> GetErrorsMessages() => _notifications.GetNotifications().Select(x => x.Value).ToList();
        public Guid ClientId { get; set; } = Guid.Parse("59876ad0-b3fa-4979-8ca8-ac9a76705547");
    }
}
