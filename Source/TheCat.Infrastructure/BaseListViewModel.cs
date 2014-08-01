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
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TheCat.Infrastructure
{
    public abstract class BaseListViewModel<T> : BaseViewModel
    {
        public ObservableCollection<T> Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
                OnPropertyChanged("Items");
            }
        }

        public T SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        protected void RefreshItems()
        {
            T previouslySelected = SelectedItem;

            Items.Clear();
            foreach (T item in GetItemsFromRepository())
                Items.Add(item);

            if (Items.Contains(previouslySelected))
                SelectedItem = previouslySelected;
        }

        protected abstract IEnumerable<T> GetItemsFromRepository();

        private ObservableCollection<T> _Items = new ObservableCollection<T>();
        private T _SelectedItem;
    }
}
