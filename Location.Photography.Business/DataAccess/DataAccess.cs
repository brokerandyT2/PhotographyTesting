using Location.Photography.Business.DataAccess.Interfaces;
using Microsoft.Maui;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Business.DataAccess
{
    public class DataAccess : IDataAccess
    {
        public ISQLiteConnection database { get { return new SQLiteConnection(Constants.FullDatabasePath, Constants.Flags); }}

        ISQLiteConnection IBaseData.database { get => database; set => throw new NotImplementedException(); }

        public SQLiteConnection DatabaseConnection => throw new NotImplementedException();

        public DataAccess() { }
    }
}
