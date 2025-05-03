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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }
    }
}
