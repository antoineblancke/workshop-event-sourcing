using System.Collections.Generic;

namespace Domain.Common
{
    public interface IEventBus
    {
        void Register(IEventListener eventListener);

        void Push(List<Event> events);

        void Clear();
    }
}
