using Locations.Core.Shared.DTO.Interfaces;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.ComponentModel;
using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Locations.Core.Shared.DTO
{
    [Table("Settings")]
    public partial class SettingDTO : DTOBase, ISettingDTO, INotifyPropertyChanged
    {
        [ObservableProperty]
        [property: PrimaryKey, AutoIncrement]
        private int _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _value;

        [ObservableProperty]
        private DateTime _timestamp;

        public event PropertyChangedEventHandler? PropertyChanged;

        public SettingDTO()
        {
            Name = string.Empty;
            Value = string.Empty;
            Timestamp = DateTime.Now;
        }
    }
}