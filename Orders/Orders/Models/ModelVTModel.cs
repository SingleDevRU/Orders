using Orders.Core;
using Orders.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Orders.Models
{
    public class ModelVTModel: INotifyPropertyChanged
    {
        List<Model> modelslist;
        public List<Model> ModelsList
        {
            get { return modelslist; }
            set
            {
                if (modelslist != value)
                {
                    modelslist = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Grouping<string, Model>> GroupedModelList
        {
            get { return GetGroupedModelsList(); }

        }
        public ObservableCollection<Grouping<string, Model>> GetGroupedModelsList()
        {      
            var groups = modelslist.GroupBy(model => model.GroupName).Select(group => new Grouping<string,Model>(group.Key,group));
            return new ObservableCollection<Grouping<string, Model>>(groups);
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
