using EncryptedSQLite;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using NormalSQLite;

namespace Locations.Core.Data.Queries
{
    public class TipTypesQuery<T> : QueryBase<T> where T : TipTypeViewModel, new()
    {
        private bool v;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        public TipTypesQuery() : base()
        {
            dataB = DataEncrypted.GetAsyncConnection(KEY);
        }
        /// <summary>
        /// Used for initial setup, have to use NON encrypted DB to start
        /// </summary>
        /// <param name="v"></param>
        public TipTypesQuery(bool v)
        {
           dataB = DataUnEncrypted.GetConnection();
        }

        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<TipTypeViewModel>().Where(x => x.Id == id), typeof(T));
        }
       
        public override T GetItemByString<T>(string name)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
            return (IList<T>)dataB.Table<TipTypeViewModel>().Where(x => x == x).ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
