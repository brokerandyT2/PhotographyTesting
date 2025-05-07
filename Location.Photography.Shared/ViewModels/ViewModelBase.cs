
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.DTO;
namespace Location.Photography.Shared.ViewModels
{
    public abstract class ViewModelBase : DTOBase, INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
        }

        //abstract event PropertyChangedEventHandler? PropertyChanged;
        public abstract event PropertyChangedEventHandler? PropertyChanged;
    }
}
