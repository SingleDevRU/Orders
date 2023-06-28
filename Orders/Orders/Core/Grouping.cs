using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Orders.Core
{
    public class Grouping<K,T> : ObservableCollection<T>
    {
        public K Name { get; private set; }
        public Grouping(K name, IEnumerable<T> items)
        {
            Name = name;
            foreach (var item in items) 
            {
                Items.Add(item);
            }
        }
    }
}
