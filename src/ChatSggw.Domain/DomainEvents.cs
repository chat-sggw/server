using System;
using System.Collections.Generic;
using Neat.CQRSLite.Contract.Events;

namespace ChatSggw.Domain
{
    public static class DomainEvents
    {
        private static readonly List<Action<IEvent>> EventHandlers = new List<Action<IEvent>>();

        public static void Publish(IEvent @event)
        {
            EventHandlers.ForEach(handler => handler(@event));
        }

        public static SubscriptionToken RegisterEventHandler(Action<IEvent> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var token = new SubscriptionToken(handler);
            EventHandlers.Add(handler);
            return token;
        }

        public sealed class SubscriptionToken : IDisposable
        {
            private readonly Action<IEvent> _handler;

            public SubscriptionToken(Action<IEvent> handler)
            {
                _handler = handler;
            }

            public void Dispose()
            {
                EventHandlers.Remove(_handler);
                GC.SuppressFinalize(this);
            }
        }
    }
}