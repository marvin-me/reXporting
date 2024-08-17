namespace Core.UserDatabase;

public interface IDataProvider
{
    public String LoadDatabaseName();
    public String LoadDatabaseVersion();
}