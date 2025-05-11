using Microservice.SharedLibrary.CURDHelper;
using System.Data;

namespace Microservice.SharedLibrary.Interface
{
    public interface IDatabaseFactory : IDisposable
    {
        IDbConnection DbConnection { get; }
        QueryBuilder QueryBuilder { get; }
    }
}
