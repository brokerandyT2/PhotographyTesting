using Locations.Core.Shared.DTO.Interfaces;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace Locations.Core.Shared.DTO
{
    [Table("Settings")]
    public class SettingDTO : ISettingDTO, INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public SettingDTO() { }
        private int _id;
        private string _name;
        private string _value;
        private DateTime _timestamp;

        [AutoIncrement, PrimaryKey]
        public int Id { get { return _id; } set { _id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id))); } }

        public string Name { get { return _name; } set { _name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); } }
        public string Value { get { return _value; } set { _value = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value))); } }
        public DateTime Timestamp { get { return _timestamp; } set { _timestamp = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Timestamp))); } }
    }
}
