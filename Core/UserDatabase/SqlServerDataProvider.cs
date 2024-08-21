using System.Data;
using System.Data.SqlClient;

namespace Core.UserDatabase;

/// <summary>
/// Represents a data provider for SQL Server database operations.
/// </summary>
/// <param name="dataContext">The SQL data context for executing queries.</param>
public class SqlServerDataProvider(ISqlDataContext dataContext) : IDataProvider
{
    /// <summary>
    /// Retrieves the name of the current database.
    /// </summary>
    /// <returns>The name of the current database.</returns>
    public string LoadDatabaseName()
    {
        return LoadScalar("SELECT TOP(1) DB_NAME();").ToString() ?? string.Empty;
    }
    
    /// <summary>
    /// Retrieves the version of the current SQL Server instance.
    /// </summary>
    /// <returns>The version of the current SQL Server instance.</returns>
    public string LoadDatabaseVersion()
    {
        return LoadScalar("SELECT TOP(1) SERVERPROPERTY('productversion');").ToString() ?? string.Empty;
    }
    
    private object LoadScalar(string cmdText)
    {
        using var context = dataContext;
        using var reader = context.ExecuteReader(cmdText, new List<SqlParameter>());
        object result;
        if (reader.Read())
            result = reader[0];
        else
        {
            throw new DataException($"{cmdText} returned no result");
        }
        
        return result;
    }
}