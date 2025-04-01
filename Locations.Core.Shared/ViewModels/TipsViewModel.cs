using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core .Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels
{
    public class TipsViewModel : ITipsViewmodel, INotifyPropertyChanged
    {
        public TipsViewModel() { }
        private List<TipTypeViewModel> _tipTypes;
        private TipViewModel _tip;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<TipTypeViewModel> Tips { get => _tipTypes; set { _tipTypes = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tips))); } }
        public TipViewModel Tip
        {
            get => _tip;
            set
            {
                _tip = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tip)));
            }
        }
    }
}
