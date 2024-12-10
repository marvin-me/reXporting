namespace Model.UserDatabase;

/// <summary>
/// Represents model for a user database table.
/// </summary>
public interface ITable
{
    /// <summary>
    /// Gets the name of the table.
    /// </summary>
    public string Name { get; }
}