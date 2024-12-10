using Model.UserDatabase;

namespace Core.UserDatabase;

/// <summary>
/// Represents a user database.
/// </summary>
public class Database : IDatabase
{
    public string? Name { get; private set; }
    public string? Version { get; private set; }
    public IEnumerable<ITable>? Tables { get; private set; }
    
    private readonly IDataProvider _dataProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="Database"/> class with the specified data provider.
    /// </summary>
    /// <param name="dataProvider">The data provider used to load database information.</param>
    public Database(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        LoadData();
    }

    /// <summary>
    /// Reloads the database information by querying the data provider.
    /// </summary>
    public void ReloadData()
    {
        LoadData();
    }

    private void LoadData()
    {
        Name = _dataProvider.LoadDatabaseName();
        Version = _dataProvider.LoadDatabaseVersion();
        Tables = _dataProvider.LoadDatabaseTables();
    }
}