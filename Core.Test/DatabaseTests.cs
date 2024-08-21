using System.Data;
using System.Data.SqlClient;
using Core.UserDatabase;
using Moq;

namespace Core.Test;

public class DatabaseTests
{
    private readonly Mock<ISqlDataContext> _dataContextMock = new Mock<ISqlDataContext>();

    public DatabaseTests()
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
    public void GetbaseName_ShouldReturnDatabaseName()
    {
        var database = new Database(new SqlServerDataProvider(_dataContextMock.Object));

        var result = database.Name;

        // Assert
        Assert.Equal("TestDB", result);
    }

    [Fact]
    public void GetVersion_ShouldReturnDatabaseVersion()
    {
        var database = new Database(new SqlServerDataProvider(_dataContextMock.Object));

        var result = database.Version;

        // Assert
        Assert.Equal("123", result);
    }

    [Fact]
    public void Reload_ShouldReturnNewDatabaseName()
    {
        var alternativeMock = new Mock<ISqlDataContext>();

        var dataReaderName = new Mock<IDataReader>();
        var dataReaderVersion = new Mock<IDataReader>();

        dataReaderName.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderName.Setup(m => m[0]).Returns("TestDB");

        dataReaderVersion.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderVersion.Setup(m => m[0]).Returns("123");

        alternativeMock.Setup(m =>
                m.ExecuteReader("SELECT TOP(1) DB_NAME();",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderName.Object);
        alternativeMock.Setup(m =>
                m.ExecuteReader("SELECT TOP(1) SERVERPROPERTY('productversion');",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderVersion.Object);

        var database = new Database(new SqlServerDataProvider(alternativeMock.Object));

        var resultBeforeReload = database.Name;

        dataReaderName.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderName.Setup(m => m[0]).Returns("NeueDB");

        dataReaderVersion.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderVersion.Setup(m => m[0]).Returns("123");


        database.ReloadData();

        var resultAfterReload = database.Name;

        // Assert
        Assert.Equal("TestDB", resultBeforeReload);
        Assert.Equal("NeueDB", resultAfterReload);
    }
}