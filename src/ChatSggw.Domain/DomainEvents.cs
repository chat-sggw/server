using System;
using System.Collections.Generic;
using Neat.CQRSLite.Contract.Events;

namespace ChatSggw.Domain
{
    //http://udidahan.com/2009/06/14/domain-events-salvation/
    public static class DomainEvents
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static List<Delegate> actions;

        public static IEventBus EventBus { get; set; } //as before

        //Registers a callback for the given domain event
        public static void Register<T>(Action<T> callback) where T : IEvent
        {
            if (actions == null)
                actions = new List<Delegate>();

            actions.Add(callback);
        }

        //Clears callbacks passed to Register on the current thread
        public static void ClearCallbacks()
        {
            actions = null;
        }

        //Raises the given domain event
        public static void Raise<T>(T args) where T : IEvent
        {
            EventBus?.Send(args);

            if (actions != null)
                foreach (var action in actions)
                {
                    (action as Action<T>)?.Invoke(args);
                }
        }
    }
}