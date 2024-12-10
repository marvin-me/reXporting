using Model.UserDatabase;

namespace Core.UserDatabase;

public class Table : ITable
{
    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Table"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the table.</param>
    public Table(string name)
    {
        Name = name;
    }

}