using System.Data;
using System.Data.SqlClient;

namespace Core.UserDatabase;

public interface ISqlDataContext : IDisposable
{
    IDataReader ExecuteReader(string cmdText, ICollection<SqlParameter> parameters);
}