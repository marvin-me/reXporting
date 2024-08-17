using System.Data;
using System.Data.SqlClient;

namespace Core.UserDatabase;

public class SqlServerDataProvider(ISqlDataContext dataContext) : IDataProvider
{
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

    public string LoadDatabaseName()
    {
        return LoadScalar("SELECT TOP(1) DB_NAME();").ToString() ?? string.Empty;
    }

    public string LoadDatabaseVersion()
    {
        return LoadScalar("SELECT TOP(1) SERVERPROPERTY('productversion');").ToString() ?? string.Empty;
    }
}