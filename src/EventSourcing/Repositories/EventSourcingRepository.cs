using EventSourcing.Interfaces;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using RssStore.Core.Data.EventSourcing;
using RssStore.Core.Data.EventSourcing.Interfaces;
using RssStore.Core.DomainObjects.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Repositories
{
    public class EventSourcingRepository : IEventSourcingRepository
    {
        private readonly IEventStoreService _eventStoreService;
        public EventSourcingRepository(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }
        public async Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId)
        {
            var events = await _eventStoreService.GetConnection().ReadStreamEventsBackwardAsync(aggregateId.ToString(), 0, 500, false);

            var eventList = new List<StoredEvent>();

            foreach (var resolvedEvent in events.Events)
            {
                var dataEncoded = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
                var jsonData = JsonConvert.DeserializeObject<BaseEvent>(dataEncoded);

                var @event = new StoredEvent(
                    resolvedEvent.Event.EventId,
                    resolvedEvent.Event.EventType,
                    jsonData.Timestamp,
                    dataEncoded);

                eventList.Add(@event);
            }

            return eventList.OrderBy(x => x.OcurrencyDate);
        }

        public async Task SaveEvent<TEvent>(TEvent @event) where TEvent : Event
        {
            await _eventStoreService.GetConnection().AppendToStreamAsync(@event.AggregateId.ToString(), ExpectedVersion.Any, FormatEvent(@event));
        }

        private static IEnumerable<EventData> FormatEvent<TEvent>(TEvent @event) where TEvent : Event
        {
            yield return new EventData(Guid.NewGuid(), @event.MessageType, true, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)), null);
        }

        internal class BaseEvent
        {
            public DateTime Timestamp { get; set; }
        }
    }
}
