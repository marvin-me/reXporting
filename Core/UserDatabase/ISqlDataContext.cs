using System.Data;
using System.Data.SqlClient;

namespace Core.UserDatabase;

/// <summary>
/// Represents a data context for executing SQL commands.
/// </summary>
public interface ISqlDataContext : IDisposable
{
    /// <summary>
    /// Executes a SQL command and returns a data reader.
    /// </summary>
    /// <param name="cmdText">The SQL command text.</param>
    /// <param name="parameters">The collection of SQL parameters.</param>
    /// <returns>An <see cref="IDataReader"/> object that contains the results of the command.</returns>
    IDataReader ExecuteReader(string cmdText, ICollection<SqlParameter> parameters);
}