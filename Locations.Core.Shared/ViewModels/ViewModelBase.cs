using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels
{
    public abstract class ViewModelBase : IDTOBase
    {
        public abstract event PropertyChangedEventHandler? PropertyChanged;
    }
}
