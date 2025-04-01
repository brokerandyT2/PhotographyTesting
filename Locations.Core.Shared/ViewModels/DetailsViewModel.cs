using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels
{
    public class DetailsViewModel : ViewModelBase, IDetailsView
    {
        public override event PropertyChangedEventHandler? PropertyChanged;

        private LocationViewModel _locationViewModel;
        private WeatherViewModel _weatherViewModel;

        public LocationViewModel LocationViewModel
        {
            get { return _locationViewModel; }
            set
            {
                _locationViewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LocationViewModel)));
            }
        }
        public WeatherViewModel WeatherViewModel
        {
            get { return _weatherViewModel; }
            set
            {
                _weatherViewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WeatherViewModel)));
            }
        }
    }
}
