using System;

namespace Domain.Common
{
    public abstract class Event
    {
        public String AggregateId { get; }

        public DateTime EventCreationDate { get; }

        public Event(string bankAccountId) {
            this.AggregateId = bankAccountId;
            this.EventCreationDate = DateTime.Now;
        }

        public abstract void ApplyOn(IEventListener eventListener);

        protected bool Equals(Event other)
        {
            return AggregateId == other.AggregateId;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Event) obj);
        }

        public override int GetHashCode()
        {
            return AggregateId.GetHashCode();
        }
    }
}