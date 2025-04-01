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
    [Table("TipType")]
    public class TipTypeDTO : ITipTypeDTO, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _id;
        private string _name;
        private string _i8n = "en-US";




        [PrimaryKey, AutoIncrement]
        public int Id
        { get { return _id; } set { _id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id))); } }
        public string Name
        { get { return _name; } set { _name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); } }
        public string I8n
        { get { return _i8n; } set { _i8n = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(I8n)); } }
    }
}
