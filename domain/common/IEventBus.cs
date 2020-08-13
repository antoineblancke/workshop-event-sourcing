using System.Collections.Generic;

namespace domain.common
{
    public interface IEventBus
    {
        void Register(IEventListener eventListener);

        void Push(List<Event> events);

        void Clear();
    }
}