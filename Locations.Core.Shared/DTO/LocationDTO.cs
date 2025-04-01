using Locations.Core.Shared.DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace Locations.Core.Shared.DTO
{
    [Table("Location")]
    public class LocationDTO : ILocationDTO, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _city;
        private string _state;
        private int _id;
        private double _lattitude;
        private double _longitude;
        private string _title = "";
        private string _description = "";
        private string _photo = "";
        private DateTime _timestamp = DateTime.Now;
        private bool _isDeleted = false;
        public string DateFormat { get; set; }
        public string TimestampFormatted { get => _timestamp.ToString(DateFormat); }
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(City)));
            }
        }
        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }
            [AutoIncrement, PrimaryKey]
        public int Id
        {   
            get { return _id; }
            set { _id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id))); }
        }
        public string Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Photo)));
            }
        }
        public double Lattitude
        {
            get { return _lattitude; }
            set
            {
                _lattitude = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lattitude)));
            }
        }

        public double Longitude
        {
            get { return _longitude; }
            set
            {
                _longitude = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude)));
            }
        }

        public string Title
        { get { return _title; } set { _title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title))); } }
        public string Description
        { get { return _description; } set { _description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description))); } }
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set
            {
                _timestamp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Timestamp)));
            }
        }
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDeleted))); }
        }
        public bool CanDelete
        {
            get => !(_longitude == 0.0 && _lattitude == 0.0);
        }
    }
}
