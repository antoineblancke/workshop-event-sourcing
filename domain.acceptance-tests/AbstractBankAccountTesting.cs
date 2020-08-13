using domain.common;
using infrastructure.infrastructure;

namespace domain.acceptance_tests
{
    public abstract class AbstractBankAccountTesting
    {
        protected IEventBus eventBus;

        protected EventStore eventStore;

        public AbstractBankAccountTesting()
        {
            eventBus = new InMemoryEventBus();
            eventStore = new InMemoryEventStore(eventBus);
        }
                
        public void Dispose() {
            eventStore.Clear();
            eventBus.Clear();
        }
    }
}