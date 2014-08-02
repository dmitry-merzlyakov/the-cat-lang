using System;
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

namespace TheCat.Infrastructure
{
    public sealed class Locator
    {
        public static T Get<T>(CompositeParams parms = null)
        {
            return Locator.Get<T>(String.Empty);
        }

        public static T Get<T>(string key, CompositeParams parms = null)
        {
            return Lazy<Locator>.Value.GetService<T>(key, parms);
        }

        public static void AddService<T>(Func<CompositeParams, T> factory, bool isTransient = false)
        {
            Locator.AddService<T>(String.Empty, factory, isTransient);
        }

        public static void AddService<T>(string key, Func<CompositeParams, T> factory, bool isTransient = false)
        {
            Lazy<Locator>.Value.AddServiceDescriptor<T>(key, factory, isTransient);
        }

        private T GetService<T>(string key, CompositeParams parms)
        {
            object obj;

            int objHash = GetObjHash(typeof(T), key, parms);
            if (!Objects.TryGetValue(objHash, out obj))
            {
                IServiceDescriptor descriptor = GetServiceDescriptor(typeof(T), key);
                obj = descriptor.Create(parms);
                if (!descriptor.IsTransient)
                    Objects[objHash] = obj;
            }

            return (T)obj;
        }

        private IServiceDescriptor GetServiceDescriptor(Type type, string key)
        {
            IServiceDescriptor serviceDescriptor;
            int descHash = GetObjHash(type, key, null);
            if (!Descriptors.TryGetValue(descHash, out serviceDescriptor))
                throw new ArgumentException(String.Format("Service '{0}:{1}' is not registered", type, key));
            return serviceDescriptor;
        }

        private void AddServiceDescriptor<T>(string key, Func<CompositeParams, T> factory, bool isTransient)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            int descHash = GetObjHash(typeof(T), key, null);
            if (Descriptors.ContainsKey(descHash))
                throw new ArgumentException(String.Format("Service '{0}:{1}' is already registered", typeof(T), key));
            Descriptors[descHash] = new ServiceDescriptor<T>(factory, isTransient);
        }

        private int GetObjHash(Type type, string key, CompositeParams parms)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            int hash = type.GetHashCode();
            if (!String.IsNullOrWhiteSpace(key))
            {
                hash ^= key.GetHashCode();
                if (parms != null)
                    hash ^= parms.GetHashCode();
            }

            return hash;
        }

        private readonly IDictionary<int, IServiceDescriptor> Descriptors = new Dictionary<int, IServiceDescriptor>();
        private readonly IDictionary<int, object> Objects = new Dictionary<int, object>();

        private interface IServiceDescriptor
        {
            object Create(CompositeParams parms);
            bool IsTransient { get; }
        }

        private class ServiceDescriptor<T> : IServiceDescriptor
        {
            public ServiceDescriptor(Func<CompositeParams, T> factory, bool isTransient)
            {
                Factory = factory;
                IsTransient = isTransient;
            }

            public bool IsTransient { get; private set; }

            public object Create(CompositeParams parms)
            {
                return Factory(parms);
            }

            private readonly Func<CompositeParams, T> Factory;
        }
    }
}
