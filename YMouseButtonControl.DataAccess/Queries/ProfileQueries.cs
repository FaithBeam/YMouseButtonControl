namespace YMouseButtonControl.DataAccess.Queries;

public class ProfileQueries : BaseQueries
{
    public override string Add() =>
        """
            INSERT INTO Profiles
            (Id, IsDefault, Checked, DisplayPriority, Name, Description, WindowCaption, Process, WindowClass, ParentClass, MatchType)
            VALUES (@Id, @IsDefault, @Checked, @DisplayPriority, @Name, @Description, @WindowCaption, @Process, @WindowClass, @ParentClass, @MatchType);
            SELECT SEQ from sqlite_sequence WHERE name='Profiles';
            """;

    public override string Update() =>
        """
            UPDATE Profiles
            SET IsDefault = @IsDefault,
                Checked = @Checked,
                DisplayPriority = @DisplayPriority,
                Name = @Name,
                Description = @Description,
                WindowCaption = @WindowCaption,
                Process = @Process,
                WindowClass = @WindowClass,
                ParentClass = @ParentClass,
                MatchType = @MatchType;
            """;
}
