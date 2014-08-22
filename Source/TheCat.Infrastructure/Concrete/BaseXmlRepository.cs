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
using System.Linq;
using System.Collections.Generic;
using TheCat.Infrastructure.VirtualFileSystem;
using TheCat.Infrastructure.Events;

namespace TheCat.Infrastructure.Concrete
{
    public class BaseXmlRepository<T,K> : IBaseRepository<T,K>
    {
        public BaseXmlRepository(IExtendedFileSystemProvider provider, string fileName, Func<T,K> keyGetter)
        {
            Provider = provider;
            FileName = fileName;
            Reader = Lazy<BaseXmlReader<T>>.Value;
            Writer = Lazy<BaseXmlWriter<T>>.Value;
            KeyGetter = keyGetter;
        }

        public string FileName 
        {
            get { return _FileName; }
            private set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                _FileName = value;
            }
        }

        public IExtendedFileSystemProvider Provider
        {
            get { return _Provider; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _Provider = value;
            }
        }

        public Func<T, K> KeyGetter
        {
            get { return _KeyGetter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _KeyGetter = value;
            }
        }

        public virtual T Get(K id)
        {
            EnsureLoaded();
            return Items.FirstOrDefault(item => Object.Equals(KeyGetter(item), id));
        }

        public virtual IQueryable<T> GetAll()
        {
            EnsureLoaded();
            return Items.AsQueryable();
        }

        public void Create(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            EnsureLoaded();
            if (Get(KeyGetter(item)) != null)
                throw new ArgumentException(String.Format("Item with key '{0}' is already added", KeyGetter(item)));

            Items.Add(item);
            Save();
            SendNotification(item, RepositoryItemChangeType.Created);
        }

        public void Update(T item)
        {
            EnsureLoaded();

            int position;
            while ((position = Items.IndexOf(Get(KeyGetter(item)))) != -1)
                Items.RemoveAt(position);

            Items.Add(item);
            Save();
            SendNotification(item, RepositoryItemChangeType.Updated);
        }

        public void Delete(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            EnsureLoaded();
            Items.Remove(item);
            Save();
            SendNotification(item, RepositoryItemChangeType.Deleted);
        }

        private List<T> Items { get; set; }

        private void EnsureLoaded()
        {
            if (Items == null)
                Items = Reader.Read(Provider, FileName) ?? new List<T>();
        }

        private void Save()
        {
            Writer.Write(Provider, Items, FileName);
        }

        private void SendNotification(T item, RepositoryItemChangeType modificationType)
        {
            Locator.Get<EventManager>().PublishEvent(new RepositoryItemChangedEvent<T>(item, modificationType));
        }

        private readonly BaseXmlReader<T> Reader;
        private readonly BaseXmlWriter<T> Writer;
        private IExtendedFileSystemProvider _Provider;
        private string _FileName;
        private Func<T, K> _KeyGetter;
    }
}
