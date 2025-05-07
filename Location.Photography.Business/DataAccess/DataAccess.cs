using Location.Photography.Business.DataAccess.Interfaces;

using SQLite;

namespace Location.Photography.Business.DataAccess
{
    public class DataAccess : IDataAccess
    {
        public ISQLiteConnection database;
        ISQLiteConnection IBaseData.database { get => database; set => throw new NotImplementedException(); }

        public SQLiteConnection DatabaseConnection => throw new NotImplementedException();

        
        public DataAccess() { }

    }
}
