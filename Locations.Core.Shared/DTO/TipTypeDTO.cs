using Locations.Core.Shared.DTO.Interfaces;
using System;
using System.ComponentModel;
using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Locations.Core.Shared.DTO
{
    [Table("TipType")]
    public partial class TipTypeDTO : DTOBase, ITipTypeDTO, INotifyPropertyChanged
    {
        [ObservableProperty]
        [property: PrimaryKey, AutoIncrement]
        private int _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _i8n = "en-US";

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}