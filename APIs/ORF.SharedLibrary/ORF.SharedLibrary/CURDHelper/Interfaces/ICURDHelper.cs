using System.Data;

namespace Microservice.SharedLibrary.CURDHelper.Interfaces
{
    public interface ICURDHelper
    {
        Task<T> GetAsync<T>(object id, IDbTransaction? transaction = null, int? commandTimeout = null);
        Task<IEnumerable<T>> GetListAsync<T>(object whereConditions, IDbTransaction? transaction = null, int? commandTimeout = null);
        Task<int> InsertAsync<T>(T entityToInsert, IDbTransaction? transaction = null, int? commandTimeout = null);
        Task<int> UpdateAsync<T>(T entityToUpdate, IDbTransaction? transaction = null, int? commandTimeout = null);
        Task<int> DeleteAsync<T>(object id, IDbTransaction? transaction = null, int? commandTimeout = null);
    }
}
