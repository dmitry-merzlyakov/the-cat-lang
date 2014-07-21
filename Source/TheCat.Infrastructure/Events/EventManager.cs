using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TheCat.Infrastructure.Events
{
    public sealed class EventManager
    {
        public void PublishEvent(IEvent evnt)
        {
            if (evnt == null)
                throw new ArgumentNullException("evnt");

            Subscription subscription;
            if (!Subscriptions.TryGetValue(evnt.GetType(), out subscription))
                throw new ArgumentException(String.Format("Event type '{0}' is not registered", evnt.GetType()));

            subscription.Subscribers.ForEach(s => s.Invoke(evnt));
        }

        public void RegisterSubscription<T>(Action<T> evntHandler) where T : IEvent
        {
            if (evntHandler == null)
                throw new ArgumentNullException("evntHandler");

            Subscription subscription;
            if (!Subscriptions.TryGetValue(typeof(T), out subscription))
            {
                subscription = new Subscription();
                Subscriptions[typeof(T)] = subscription;
            }

            subscription.Subscribers.Add(new Subscriber<T>(evntHandler));
        }

        public void UnregisterSubscription<T>(Action<T> evntHandler) where T : IEvent
        {
            if (evntHandler == null)
                throw new ArgumentNullException("evntHandler");

            Subscription subscription;
            if (Subscriptions.TryGetValue(typeof(T), out subscription))
            {
                ISubscriber subscriber = subscription.Subscribers.FirstOrDefault(s => s.HasHandler(evntHandler));
                if (subscriber != null)
                    subscription.Subscribers.Remove(subscriber);

                if (!subscription.Subscribers.Any())
                    Subscriptions.Remove(typeof(T));
            }
        }

        private readonly IDictionary<Type, Subscription> Subscriptions = new Dictionary<Type, Subscription>();

        private interface ISubscriber
        {
            void Invoke(IEvent evnt);
            bool HasHandler(object evntHandler);
        }

        private class Subscriber<T> : ISubscriber where T : IEvent
        {
            public Subscriber(Action<T> action)
            {
                if (action == null)
                    throw new ArgumentNullException("action");

                //Action = new WeakReference(action);
                Action = action;
            }

            //public WeakReference Action { get; private set; }
            public Action<T> Action { get; private set; }

            public void Invoke(IEvent evnt)
            {
                /*
                if (Action.IsAlive)
                {
                    Action<T> target = (Action<T>)Action.Target;
                    target((T)evnt);
                }
                 */
                Action((T)evnt);
            }

            public bool HasHandler(object evntHandler)
            {
                return Object.Equals(Action, evntHandler);
            }
        }

        private class Subscription
        {
            public Subscription()
            {
                Subscribers = new List<ISubscriber>();
            }

            public List<ISubscriber> Subscribers { get; private set; }
        }
    }
}
