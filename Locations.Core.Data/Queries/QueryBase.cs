using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EncryptedSQLite;
using NormalSQLite;
using static Locations.Core.Shared.Customizations.Alerts.Implementation.AlertService;

namespace Locations.Core.Data.Queries
{
    public abstract class QueryBase<T> : Database, IDataAccessBase where T : new()
    {

        protected readonly IAlertService AlertService;
        protected readonly ILoggerService LoggerService;
        private string email;
        public bool IsError;
        public QueryBase(IAlertService alertService, Locations.Core.Shared.Customizations.Logging.Interfaces.ILoggerService loggerService)
        {
            email = string.Empty;
            AlertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
        }

        public QueryBase(IAlertService alertService, ILoggerService loggerService, string email) : this(alertService, loggerService)
        {
            this.email = email;
        }
        public abstract T GetItem<T>(int id);

        public abstract string GetValueByString<T>(string name);

        public abstract IList<T> GetItems<T>();
        public abstract T GetItemByString<T>(string name);
        public int DeleteItem<T>(T item) where T : new()
        {
            try
            {
                var x = dataB.DeleteAsync(item).Result;
                if (x != 0)
                {
                    IsError = true;
                    return 69;
                }
                else
                {
                    IsError = false;
                    return x;
                }
            }
            catch
            {
                return 420;
            }
        }


        public void DeleteItem<T>(int id)
        {
            try
            {
                var x = GetItem<T>(id);
                var y = dataB.DeleteAsync(x).Result;
            }
            catch (Exception ex)
            {
                IsError = true;
                LoggerService.LogError(ex.Message);
            }
        }
        public void SaveItem(T item)
        {
            try
            {
                var x = dataB.InsertOrReplaceAsync(item).Result;
            }
            catch (Exception ex)
            {
                IsError = true;
                LoggerService.LogError(ex.Message);

            }
        }
        public void Update(T item)
        {
            SaveItem(item);
        }
        /// <summary>
        /// Inserts the object and returns the entire object with the primary key populated.  This does not use the SQLite ASYNCRONOUS, but rather instantiates a SYNCRONOUS SQLlite connetion.  Only use when return ID is needed for things like FK relationships for insertion.
        /// </summary>
        /// <param name="item">Must be of type ViewModel</param>
        /// <returns></returns>
        public T SaveWithIDReturn(T item)
        {
            SQLite.SQLiteConnection conn;
            if (string.IsNullOrEmpty(email))
            {
                conn = DataUnEncrypted.GetSyncConnection();

            }
            else
            {
                conn = DataEncrypted.GetSyncConnection(this.email);
            }

            try
            {
                conn.Insert(item);
            }
            catch (Exception ex)
            {
                IsError = true;
                LoggerService.LogError(ex.Message);
                
            }
            return (T)Convert.ChangeType(item, typeof(T));
        }

    }
}
