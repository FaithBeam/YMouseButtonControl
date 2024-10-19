namespace YMouseButtonControl.DataAccess.Queries;

public class ThemeQueries : BaseQueries
{
    public override string Add() =>
        """
            INSERT INTO Themes (Id, Name, Background, Highlight)
            VALUES (@Id, @Name, @Background, @Highlight);
            """;

    public override string Update() =>
        """
            UPDATE Themes
            SET Name = @Name,
                Background = @Background,
                Highlight = @Highlight
            Where Id = @Id,
            """;
}
