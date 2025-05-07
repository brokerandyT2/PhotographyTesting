using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels
{
    public class WeatherViewModel : WeatherDTO, IWeatherViewModel
    {
        public WeatherViewModel():base() { }
    }
}
