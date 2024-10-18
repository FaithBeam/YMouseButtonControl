namespace YMouseButtonControl.DataAccess.Queries;

public abstract class BaseQueries
{
    public abstract string Add();

    public virtual string GetByName(string tblName) =>
        $"""
            SELECT * FROM {tblName}
            WHERE Name = @Name
            LIMIT 1;
            """;

    public virtual string GetAll(string tblName) => $"SELECT * FROM {tblName};";

    public virtual string GetById(string tblName) => $"SELECT * FROM {tblName} WHERE Id = @Id;";

    public abstract string Update();

    public virtual string DeleteById(string tblName) => $"DELETE FROM {tblName} WHERE Id = @Id;";
}
