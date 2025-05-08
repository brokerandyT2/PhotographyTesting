using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Locations.Core.Shared.DTO
{
    public partial class DailyWeatherWithDetailsDTO : DTOBase, INotifyPropertyChanged
    {
        [ObservableProperty]
        private WeatherDetailsDTO _details;

        [ObservableProperty]
        private string _forecast;

        [ObservableProperty]
        private string _sunRise;

        [ObservableProperty]
        private string _sunSet;

        [ObservableProperty]
        private string _icon;

        [ObservableProperty]
        private double _high_temp;

        [ObservableProperty]
        private double _low_temp;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}