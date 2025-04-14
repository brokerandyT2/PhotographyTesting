using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Shared.ViewModels
{
    public abstract class ViewModelBase : ObservableObject, INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
        }

        //abstract event PropertyChangedEventHandler? PropertyChanged;
        public abstract event PropertyChangedEventHandler? PropertyChanged;
    }
}
