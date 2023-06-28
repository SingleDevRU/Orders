using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Orders.Tables;

namespace Orders.Models
{
    public class OrderModel : INotifyPropertyChanged
    {
        List<Order> orderlist;
        public List<Order> OrderList
        {
            get { return orderlist; }
            set
            {
                if (orderlist != value)
                {
                    orderlist = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
