using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Core.UserDatabase;
using Moq;

namespace Test.Core;

public class DatabaseTests
{
    private readonly Mock<ISqlDataContext> _dataContextMock = new Mock<ISqlDataContext>();

    public DatabaseTests()
    {
        var dataReaderName = new Mock<IDataReader>();
        var dataReaderVersion = new Mock<IDataReader>();
        var dataReaderTables = new Mock<IDataReader>();

        dataReaderName.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderName.Setup(m => m[0]).Returns("TestDB");

        dataReaderVersion.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderVersion.Setup(m => m[0]).Returns("123");
        
        dataReaderTables.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderTables.Setup(m => m.GetString(0)).Returns("TestTable");

        _dataContextMock.Setup(m =>
                m.ExecuteReader("SELECT TOP(1) DB_NAME();",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderName.Object);
        _dataContextMock.Setup(m =>
                m.ExecuteReader("SELECT TOP(1) SERVERPROPERTY('productversion');",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderVersion.Object);
        _dataContextMock.Setup(m =>
                m.ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE';",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderTables.Object);
    }

    [Fact]
    public void GetName_ShouldReturnDatabaseName()
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
    public void GetTable_ShouldReturnDatabaseTables()
    {
        var database = new Database(new SqlServerDataProvider(_dataContextMock.Object));

        var result = database.Tables;
        
        // Assert
        Assert.Equal("TestTable", result.First().Name);
    }

    [Fact]
    public void Reload_ShouldReturnNewDatabaseName()
    {
        var alternativeMock = new Mock<ISqlDataContext>();

        var dataReaderName = new Mock<IDataReader>();
        var dataReaderVersion = new Mock<IDataReader>();
        var dataReaderTables = new Mock<IDataReader>();

        dataReaderName.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderName.Setup(m => m[0]).Returns("TestDB");

        dataReaderVersion.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderVersion.Setup(m => m[0]).Returns("123");
        
        dataReaderTables.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderTables.Setup(m => m.GetString(0)).Returns("TestTable");

        alternativeMock.Setup(m =>
                m.ExecuteReader("SELECT TOP(1) DB_NAME();",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderName.Object);
        alternativeMock.Setup(m =>
                m.ExecuteReader("SELECT TOP(1) SERVERPROPERTY('productversion');",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderVersion.Object);
        alternativeMock.Setup(m =>
                m.ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE';",
                    It.IsAny<ICollection<SqlParameter>>()))
            .Returns(dataReaderTables.Object);

        var database = new Database(new SqlServerDataProvider(alternativeMock.Object));

        var resultBeforeReload = database.Name;

        dataReaderName.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderName.Setup(m => m[0]).Returns("NeueDB");

        dataReaderVersion.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderVersion.Setup(m => m[0]).Returns("123");
        
        dataReaderTables.SetupSequence(m => m.Read()).Returns(true).Returns(false);
        dataReaderTables.Setup(m => m[0]).Returns("TestTable");

        database.ReloadData();

        var resultAfterReload = database.Name;

        // Assert
        Assert.Equal("TestDB", resultBeforeReload);
        Assert.Equal("NeueDB", resultAfterReload);
    }
}