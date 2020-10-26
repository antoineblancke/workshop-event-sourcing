using System;
using System.Collections.Generic;
using System.Linq;
using domain.common;

namespace infrastructure.infrastructure
{
    public class InMemoryEventStore : EventStore
    {
        private readonly Dictionary<string, List<Event>> store = new Dictionary<string, List<Event>>();
        
        public InMemoryEventStore(IEventBus eventBus) : base(eventBus)
        {
        }

        protected override List<Event> Save(int aggregateVersion, List<Event> events)
        {
            if (!events.Any())
            {
                return events;
            }

            var aggregateId = events.First().AggregateId;
            var currentEvents = store.GetValueOrDefault(aggregateId, new List<Event>());
            if (currentEvents.Count() != aggregateVersion)
            {
                throw new Exception($"Conflict when saving events for aggregate {aggregateId} : version {aggregateVersion} already exists");
            }

            currentEvents.AddRange(events);
            store[aggregateId] = currentEvents;
            DispatchToEventBus(events);
            return events;
        }

        protected override List<Event> Load(string bankAccountId, int fromAggregateVersion)
        {
            return store.GetValueOrDefault(bankAccountId, new List<Event>());
        }

        public override void Clear()
        {
            store.Clear();
        }
    }
}