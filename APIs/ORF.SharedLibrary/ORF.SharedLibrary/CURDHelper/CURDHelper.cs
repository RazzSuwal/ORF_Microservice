using Dapper;
using Microservice.SharedLibrary.CURDHelper.Interfaces;
using Microservice.SharedLibrary.Interface;
using System.Data;

namespace Microservice.SharedLibrary.CURDHelper
{
    public class CURDHelper : ICURDHelper
    {
        private readonly IDatabaseFactory _factory;

        public CURDHelper(IDatabaseFactory factory)
        {
            _factory = factory;
        }

        public async Task<T> GetAsync<T>(object id, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            var sql = _factory.QueryBuilder.Get<T>(id);
            var result = await _factory.DbConnection.QueryAsync<T>(sql.QuerySql!, sql.Parameters, transaction, commandTimeout);
            return result.FirstOrDefault()!;
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(object whereConditions, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            var sql = _factory.QueryBuilder.GetList<T>(whereConditions);
            return await _factory.DbConnection.QueryAsync<T>(sql.QuerySql!, sql.Parameters, transaction, commandTimeout);
        }

        public async Task<int> InsertAsync<T>(T entityToInsert, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            var sql = _factory.QueryBuilder.Insert<T>(entityToInsert!);
            return await _factory.DbConnection.ExecuteAsync(sql.QuerySql!, sql.Parameters, transaction, commandTimeout);
        }

        public async Task<int> UpdateAsync<T>(T entityToUpdate, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            var sql = _factory.QueryBuilder.Update<T>(entityToUpdate);
            return await _factory.DbConnection.ExecuteAsync(sql.QuerySql!, sql.Parameters, transaction, commandTimeout);
        }

        public async Task<int> DeleteAsync<T>(object id, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            var sql = _factory.QueryBuilder.Delete<T>(id);
            return await _factory.DbConnection.ExecuteAsync(sql.QuerySql!, sql.Parameters, transaction, commandTimeout);
        }
    }
}
