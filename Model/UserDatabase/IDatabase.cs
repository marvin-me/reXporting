namespace Model.UserDatabase;

/// <summary>
/// Represents model for a user database.
/// </summary>
public interface IDatabase
{
    /// <summary>
    /// Gets the name of the database.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the version of the database.
    /// </summary>
    public string? Version { get; }
    
    /// <summary>
    /// Gets the tables of the database.
    /// </summary>
    public IEnumerable<ITable>? Tables { get; }
}