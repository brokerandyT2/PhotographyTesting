using Locations.Core.Shared.DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations.Schema;

namespace Locations.Core.Shared.DTO
{
    [Table("Tips")]
    public class TipDTO : DTOBase, ITipDTO, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public TipDTO() { }

        private string _fstop;
        private string _shutterspeed;
        private string _iso;
        private int _tipTypeID;
        private int _id;
        private string _title;
        private string _i8n;
        private string _content;
        [PrimaryKey, AutoIncrement]
        public int ID
        { get => _id; set { _id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID))); } }
        public string Fstop
        { get => _fstop; set { _fstop = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Fstop))); } }
        public string Shutterspeed
        { get => _shutterspeed; set { _shutterspeed = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Shutterspeed))); } }
        public string ISO
        { get => _iso; set { _iso = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ISO))); } }
        public string Content
        {
            get => _content;
            set
            {
                _content = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Content)));
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("TipTypeDTO")]
        public int TipTypeID { get => _tipTypeID; set { _tipTypeID = value; } }
        public string Title
        { get => _title; set { _title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title))); } }
        public string I8n
        {
            get => _i8n; set { _i8n = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(I8n))); }
        }
    }
}
