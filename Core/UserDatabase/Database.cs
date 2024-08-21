namespace Core.UserDatabase;

public class Database
{
    public string Name { get; private set; }
    public string Version { get; private set; }

    private readonly IDataProvider _dataProvider;

    public Database(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        LoadData();
    }

    public void ReloadData()
    {
        LoadData();
    }

    private void LoadData()
    {
        Name = _dataProvider.LoadDatabaseName();
        Version = _dataProvider.LoadDatabaseVersion();
    }
    
}