// RepositoryTestBase.cs - New file
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Base;
using Locations.Core.Data.Queries.Interfaces;
using Moq;
using SQLite;
using System;
using System.Threading.Tasks;

namespace Locations.Core.Business.Tests.Base
{
    /// <summary>
    /// Base class for repository tests with SQLite in-memory database
    /// </summary>
    [TestClass]
    public abstract class RepositoryTestBase<TEntity, TRepository> : BaseServiceTests
        where TEntity : class, new()
        where TRepository : RepositoryBase<TEntity>
    {
        // The repository being tested
        protected TRepository Repository;

        [TestInitialize]
        public override async void Setup()
        {
            base.Setup();

            // Initialize the in-memory database
            await InitializeInMemoryDatabase();

            // Create the repository with the in-memory database
            Repository = CreateRepository();
        }

        /// <summary>
        /// Create the repository instance - must be implemented by derived classes
        /// </summary>
        protected abstract TRepository CreateRepository();

        /// <summary>
        /// Add a test entity to the database
        /// </summary>
        protected async Task<TEntity> AddTestEntityAsync(TEntity entity)
        {
            await InMemoryDb.InsertAsync(entity);
            return entity;
        }

        /// <summary>
        /// Get an entity from the database by ID
        /// </summary>
        protected async Task<TEntity> GetEntityByIdAsync(int id, string primaryKeyName = "Id")
        {
            var query = $"SELECT * FROM {typeof(TEntity).Name} WHERE {primaryKeyName} = ?";
            var result = await InMemoryDb.QueryAsync<TEntity>(query, id);
            return result.Count > 0 ? result[0] : null;
        }
    }
}