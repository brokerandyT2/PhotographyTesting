using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace Locations.Core.Shared.ViewModels
{
    /// <summary>
    /// This class is only here for SQLite to generate the table
    /// </summary>
    public class Log
    {
        private int _id;

        [PrimaryKey, AutoIncrement]
        public int ID { get => _id; set => value = _id; }
        public string Timestamp;
        public string Level;
        public string Message;
    }
}
