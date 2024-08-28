namespace Core.UserDatabase;

/// <summary>
/// Provides methods for loading database-related information.
/// </summary>
public interface IDataProvider
{
    /// <summary>
    /// Loads the name of the database.
    /// </summary>
    /// <returns>The name of the database as a string.</returns>
    public String LoadDatabaseName();

    /// <summary>
    /// Loads the version of the database.
    /// </summary>
    /// <returns>The version of the database as a string.</returns>
    public String LoadDatabaseVersion();
}