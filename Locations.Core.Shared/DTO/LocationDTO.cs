using Locations.Core.Shared.DTO.Interfaces;
using System;
using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Locations.Core.Shared.DTO
{
    [Table("Location")]
    public partial class LocationDTO : DTOBase, ILocationDTO
    {
        [ObservableProperty]
        [property: PrimaryKey, AutoIncrement]
        private int _id;

        [ObservableProperty]
        private string _city = string.Empty;

        [ObservableProperty]
        private string _state = string.Empty;

        [ObservableProperty]
        private double _lattitude;

        [ObservableProperty]
        private double _longitude;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private string _photo = string.Empty;

        [ObservableProperty]
        private DateTime _timestamp = DateTime.Now;

        [ObservableProperty]
        private bool _isDeleted = false;

        public string DateFormat { get; set; } = string.Empty;

        public string TimestampFormatted => Timestamp.ToString(DateFormat);

        public bool CanDelete => !(Longitude == 0.0 && Lattitude == 0.0);
        partial void OnLongitudeChanged(double oldValue, double newValue);
        partial void OnLattitudeChanged(double oldValue, double newValue);
        partial void OnLattitudeChanged(double oldValue, double newValue) =>
            OnPropertyChanged(nameof(CanDelete));

        partial void OnLongitudeChanged(double oldValue, double newValue) =>
            OnPropertyChanged(nameof(CanDelete));
    }
}
