using System.Data;
using System.Data.SqlClient;
using Core.UserDatabase;
using Moq;

namespace Core.Test;

public class SqlServerDataProviderTests
{
    private readonly Mock<ISqlDataContext> _dataContextMock = new Mock<ISqlDataContext>();

    public SqlServerDataProviderTests()
    {
        var dataReaderName = new Mock<IDataReader>();
        var dataReaderVersion = new Mock<IDataReader>();

        dataReaderName.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderName.Setup(m => m[0]).Returns("TestDB");

        dataReaderVersion.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderVersion.Setup(m => m[0]).Returns("123");
        
        _dataContextMock.Setup(m =>
                m.ExecuteReader("SELECT TOP(1) DB_NAME();",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderName.Object);
        _dataContextMock.Setup(m =>
                m.ExecuteReader("SELECT TOP(1) SERVERPROPERTY('productversion');",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderVersion.Object);
    }

    [Fact]
    public void LoadDatabaseName_ShouldReturnDatabaseName()
    {
        var provider = new SqlServerDataProvider(_dataContextMock.Object);

        var result = provider.LoadDatabaseName();

        // Assert
        Assert.Equal("TestDB", result);
    }

    [Fact]
    public void LoadDatabaseVersion_ShouldReturnDatabaseVersion()
    {
        var provider = new SqlServerDataProvider(_dataContextMock.Object);

        var result = provider.LoadDatabaseVersion();

        // Assert
        Assert.Equal("123", result);
    }
}