using Locations.Core.Shared.DTO.Interfaces;
using System;

using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locations.Core.Shared.DTO
{
    [SQLite.Table("Tips")]
    public partial class TipDTO : DTOBase, ITipDTO
    {
        [ObservableProperty]
        [property: PrimaryKey, AutoIncrement]
        private int _id;

        [ObservableProperty]
        private string _fstop = string.Empty;

        [ObservableProperty]
        private string _shutterspeed = string.Empty;

        [ObservableProperty]
        private string _iso = string.Empty;

        [ObservableProperty]
        [property: ForeignKey("TipTypeDTO")]
        private int _tipTypeID;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _i8n = string.Empty;

        [ObservableProperty]
        private string _content = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public TipDTO() { }
    }
}