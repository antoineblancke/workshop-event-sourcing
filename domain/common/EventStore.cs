using System.Collections.Generic;
using System.Linq;

namespace domain.common
{
    public abstract class EventStore : IEventStore
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
        
        public List<Event> Load(string bankAccountId) {
            return Load(bankAccountId, 1);
        }

        protected void DispatchToEventBus(List<Event> events) {
            eventBus.Push(events);
        }


        protected abstract List<Event> Save(int aggregateVersion, List<Event> events);

        protected abstract List<Event> Load(string bankAccountId, int fromAggregateVersion);

        public abstract void Clear();
    }

    public interface IEventStore
    {
        List<Event> Save(int aggregateVersion, params Event[] events);
        List<Event> Load(string bankAccountId);
    }
}