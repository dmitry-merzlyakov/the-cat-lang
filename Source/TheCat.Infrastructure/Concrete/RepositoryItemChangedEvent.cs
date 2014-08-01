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
using TheCat.Infrastructure.Events;

namespace TheCat.Infrastructure.Concrete
{
    public class RepositoryItemChangedEvent<T> : BaseEvent<T>
    {
        public RepositoryItemChangedEvent(T item, RepositoryItemChangeType modificationType)
            : base(item)
        {
            ModificationType = modificationType;
        }

        public RepositoryItemChangeType ModificationType { get; private set; }
    }

    public enum RepositoryItemChangeType
    {
        Created,
        Updated,
        Deleted
    }
}
