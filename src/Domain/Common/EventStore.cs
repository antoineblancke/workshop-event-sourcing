using System.Collections.Generic;
using System.Linq;

namespace Domain.Common
{
    public abstract class EventStore
    {
        private IEventBus eventBus;

        public EventStore(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public List<Event> Save(int aggregateVersion, params Event[] events)
        {
            return Save(aggregateVersion, events.ToList());
        }

        public List<Event> Load(string bankAccountId)
        {
            return Load(bankAccountId, 1);
        }

        public void DispatchToEventBus(List<Event> events)
        {
            eventBus.Push(events);
        }

        public abstract List<Event> Save(int aggregateVersion, List<Event> events);

        public abstract List<Event> Load(string bankAccountId, int fromAggregateVersion);

        public abstract void Clear();
    }
}
