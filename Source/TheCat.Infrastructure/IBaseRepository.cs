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
using System.Linq;

namespace TheCat.Infrastructure
{
    public interface IBaseRepository<T,K>
    {
        T Get(K id);
        IQueryable<T> GetAll();
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
