using System.Data;
using System.Data.SqlClient;

namespace Core.UserDatabase;

public class SqlServerDataContext(string connectionString) : ISqlDataContext
{
    private SqlConnection _connection = null!;

    private bool _disposed = true;

    public void Open()
    {
        _connection = CreateConnection();
        _disposed = false;
    }

    public IDataReader ExecuteReader(string cmdText, ICollection<SqlParameter> parameters)
    {
        if (_disposed) Open();
        SqlCommand command = new SqlCommand(cmdText, _connection);
        foreach (SqlParameter parameter in parameters)
            command.Parameters.Add(parameter);
        _connection.Open();
        var reader = command.ExecuteReader();
        return reader;
    }

    private SqlConnection CreateConnection()
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        return new SqlConnection(connectionString);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
            _disposed = true;
        }
    }
}