using Microservice.SharedLibrary.CURDHelper;
using Microservice.SharedLibrary.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace PropertyApi.Infrastructure.Data
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly string _connectionString;

        public DatabaseFactory(string connectionString)
        {
            _connectionString = connectionString;
            QueryBuilder = new DefaultQueryBuilder();
        }

        public IDbConnection DbConnection => new SqlConnection(_connectionString);

        public QueryBuilder QueryBuilder { get; }

        public void Dispose()
        {
            DbConnection?.Dispose();
        }
    }

    public class DefaultQueryBuilder : QueryBuilder
    {
        // Optionally override GetTableName or other methods here
    }
}
