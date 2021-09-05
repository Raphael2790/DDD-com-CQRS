using MediatR;
using Microsoft.AspNetCore.Mvc;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.Data.EventSourcing.Interfaces;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.WebApp.MVC.Controllers.Base;
using System;
using System.Threading.Tasks;

namespace RssStore.WebApp.MVC.Controllers
{
    public class EventsController : BaseController
    {
        private readonly IEventSourcingRepository _eventSourcingRepository;
        public EventsController(INotificationHandler<DomainNotification> notifications,
                                IMediatorHandler mediatorHandler, 
                                IEventSourcingRepository eventSourcingRepository) : base(notifications, mediatorHandler)
        {
            _eventSourcingRepository = eventSourcingRepository;
        }

        [HttpGet("eventos/{id:guid}")]
        public async Task<IActionResult> Index(Guid id)
        {
            var events = await _eventSourcingRepository.GetEvents(id);
            return View(events);
        }
    }
}
