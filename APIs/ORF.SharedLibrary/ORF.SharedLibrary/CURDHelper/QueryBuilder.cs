using Dapper;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

namespace Microservice.SharedLibrary.CURDHelper
{
    public class QueryRequest
    {
        public string? QuerySql { get; set; }
        public object? Parameters { get; set; }
    }

    public abstract class QueryBuilder
    {
        private readonly ConcurrentDictionary<Type, string> _tableNames = new();

        protected virtual string GetTableName(Type type) => _tableNames.GetOrAdd(type, t => t.Name);

        public virtual QueryRequest Get<T>(object id)
        {
            return GetById<T>("Id", id);
        }

        public QueryRequest GetById<T>(string keyColumn, object id)
        {
            var table = GetTableName(typeof(T));
            var query = $"SELECT * FROM {table} WHERE {keyColumn} = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return new QueryRequest
            {
                QuerySql = query,
                Parameters = parameters
            };
        }

        public QueryRequest GetList<T>(object? whereConditions = null)
        {
            var table = GetTableName(typeof(T));
            var query = new StringBuilder($"SELECT * FROM {table}");
            var parameters = new DynamicParameters();

            if (whereConditions != null)
            {
                var props = whereConditions.GetType().GetProperties();
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", props.Select(p => $"{p.Name} = @{p.Name}")));
                foreach (var prop in props)
                    parameters.Add($"@{prop.Name}", prop.GetValue(whereConditions));
            }

            return new QueryRequest
            {
                QuerySql = query.ToString(),
                Parameters = parameters
            };
        }

        //public QueryRequest Insert<T>(T entity)
        //{
        //    var table = GetTableName(typeof(T));
        //    var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    var columns = string.Join(", ", props.Select(p => p.Name));
        //    var values = string.Join(", ", props.Select(p => $"@{p.Name}"));

        //    var parameters = new DynamicParameters();
        //    foreach (var prop in props)
        //        parameters.Add($"@{prop.Name}", prop.GetValue(entity));

        //    return new QueryRequest
        //    {
        //        QuerySql = $"INSERT INTO {table} ({columns}) VALUES ({values})",
        //        Parameters = parameters
        //    };
        //}
        public QueryRequest Insert<T>(T entity)
        {
            var table = GetTableName(typeof(T));
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Skip properties where the name is "Id" and the type is int (common pattern for identity key)
            var insertableProps = props
                .Where(p => !(p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) && p.PropertyType == typeof(int)))
                .ToList();

            var columns = string.Join(", ", insertableProps.Select(p => p.Name));
            var values = string.Join(", ", insertableProps.Select(p => $"@{p.Name}"));

            var parameters = new DynamicParameters();
            foreach (var prop in insertableProps)
                parameters.Add($"@{prop.Name}", prop.GetValue(entity));

            return new QueryRequest
            {
                QuerySql = $"INSERT INTO {table} ({columns}) VALUES ({values})",
                Parameters = parameters
            };
        }
        public QueryRequest Update<T>(T entity, string keyColumn = "Id")
        {
            var table = GetTableName(typeof(T));
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var keyProp = props.FirstOrDefault(p => p.Name == keyColumn)
                ?? throw new ArgumentException($"Key column '{keyColumn}' not found.");

            var setProps = props.Where(p => p.Name != keyColumn).ToList();
            var setClause = string.Join(", ", setProps.Select(p => $"{p.Name} = @{p.Name}"));

            var parameters = new DynamicParameters();
            foreach (var prop in props)
                parameters.Add($"@{prop.Name}", prop.GetValue(entity));

            parameters.Add("@Key", keyProp.GetValue(entity));

            return new QueryRequest
            {
                QuerySql = $"UPDATE {table} SET {setClause} WHERE {keyColumn} = @Key",
                Parameters = parameters
            };
        }

        public QueryRequest Delete<T>(object id, string keyColumn = "Id")
        {
            var table = GetTableName(typeof(T));
            var query = $"DELETE FROM {table} WHERE {keyColumn} = @Key";

            var parameters = new DynamicParameters();
            parameters.Add("@Key", id);

            return new QueryRequest
            {
                QuerySql = query,
                Parameters = parameters
            };
        }
    }
}
