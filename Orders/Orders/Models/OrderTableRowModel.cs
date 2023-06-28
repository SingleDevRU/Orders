using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Orders.Tables;

namespace Orders.Models
{
    public class OrderTableRowModel : INotifyPropertyChanged
    {
        List<OrderTableRow> ordertablerowlist;
        public List<OrderTableRow> OrderTableRowList
        {
            get { return ordertablerowlist; }
            set
            {
                if (ordertablerowlist != value)
                {
                    ordertablerowlist = value;
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
