namespace YMouseButtonControl.DataAccess.Queries;

public class SettingQueries : BaseQueries
{
    public override string Add() =>
        """
            INSERT INTO Settings
            (Id, Name, BoolValue, StringValue, IntValue)
            VALUES (@Id, @Name, @BoolValue, @StringValue, @IntValue);
            """;

    public override string Update() =>
        """
            UPDATE Settings
            SET Name = @Name,
                BoolValue = @BoolValue,
                StringValue = @StringValue,
                IntValue = @IntValue
            WHERE Id = @Id;
            """;
}
