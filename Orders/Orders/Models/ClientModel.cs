using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Orders.Tables;

namespace Orders.Models
{
    public class ClientModel : INotifyPropertyChanged
    {
        List<Client> clientslist;
        public List<Client> ClientsList
        {
            get { return clientslist; }
            set
            {
                if (clientslist != value)
                {
                    clientslist = value;
                    OnPropertyChanged();
                }
            }
        }

        //Client selectedclient;
        //public Client SelectedClient
        //{
        //    get { return selectedclient; }
        //    set
        //    {
        //        if (selectedclient != value)
        //        {
        //            selectedclient = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

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
