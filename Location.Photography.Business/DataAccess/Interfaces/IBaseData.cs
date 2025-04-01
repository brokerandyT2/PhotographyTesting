using Microsoft.Maui;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Photography.Shared;
namespace Location.Photography.Business.DataAccess.Interfaces
{
    public interface IBaseData
    {
        public ISQLiteConnection database { get; set; }
        public SQLiteConnection DatabaseConnection { get; }
    }
}
