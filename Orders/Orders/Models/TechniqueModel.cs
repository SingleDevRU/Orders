using Orders.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Orders.Models
{
    public class TechniqueModel: INotifyPropertyChanged
    {
        List<Technique> techniquelist;
        public List<Technique> TechniqueList
        {
            get { return techniquelist; }
            set
            {
                if (techniquelist != value)
                {
                    techniquelist = value;
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
