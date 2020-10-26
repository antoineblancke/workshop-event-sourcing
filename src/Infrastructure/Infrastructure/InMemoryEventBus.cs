using System.Collections.Generic;

using Domain.Common;

namespace Infrastructure.Infrastructure
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly List<IEventListener> listeners = new List<IEventListener>();

        public void Register(IEventListener eventListener)
        {
            listeners.Add(eventListener);
        }

        public void Push(List<Event> events)
        {
            events.ForEach(evt => listeners.ForEach(evt.ApplyOn));
        }

        public void Clear()
        {
            listeners.Clear();
        }
    }
}
