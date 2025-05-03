using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.ViewModels;
namespace  Locations.Core.Data.Queries
{
    public class TipQuery<T> : QueryBase<T> where T : TipViewModel, new()
    {
        public TipQuery(IAlertService alertSer, Locations.Core.Shared.Customizations.Logging.Interfaces.ILoggerService loggerServic) : base(alertSer, loggerServic)
        {
        }

        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<TipViewModel>().Where(x => x.ID == id).FirstOrDefaultAsync().Result,typeof(T));
        }

        public override T GetItemByString<T>(string title)
        {
            return (T)Convert.ChangeType(dataB.Table<TipViewModel>().Where(x => x.Title == title).FirstOrDefaultAsync().Result, typeof(T));
        }

        public string GetSettingByString<T1>(string name)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
            return (IList<T>)dataB.Table<TipViewModel>().ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
