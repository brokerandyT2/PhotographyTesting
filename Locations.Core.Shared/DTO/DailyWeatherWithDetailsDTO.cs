using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.DTO
{
    public class DailyWeatherWithDetailsDTO : DTOBase, INotifyPropertyChanged
    {
        private WeatherDetailsDTO _details;
        public WeatherDetailsDTO Details { get => _details; set => _details = value; }
        public string Forecast { get => _forecast; set => _forecast = value; }
        public string SunRise { get => _sunRise; set => _sunRise = value; }
        public string SunSet { get => _sunSet; set => _sunSet = value; }
        public string Icon { get => _icon; set => _icon = value; }
        public double High_temp { get => _high_temp; set => _high_temp = value; }
        public double Low_temp { get => _low_temp; set => _low_temp = value; }

        private string _forecast;
        private string _sunRise;
        private string _sunSet;
        private string _icon;
        private double _high_temp;
        private double _low_temp;


        public event PropertyChangedEventHandler? PropertyChanged;

        
    }
}
