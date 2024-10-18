namespace YMouseButtonControl.DataAccess.Queries;

public class ButtonMappingQueries : BaseQueries
{
    public string GetByProfileId() => $"SELECT * FROM ButtonMappings WHERE ProfileId = @Id;";

    public override string Add() =>
        """
            INSERT INTO ButtonMappings 
                (Keys, MouseButton, ProfileId, SimulatedKeystrokeType, Selected, BlockOriginalMouseInput, ButtonMappingType) 
            VALUES (@Keys, @MouseButton, @ProfileId, @SimulatedKeystrokeType, @Selected, @BlockOriginalMouseInput, @ButtonMappingType);
            """;

    public override string Update() =>
        """
            UPDATE ButtonMappings 
            SET Keys = @Keys,
                MouseButton = @MouseButton,
                ProfileId = @ProfileId,
                SimulatedKeystrokeType = @SimulatedKeystrokeType,
                Selected = @Selected,
                BlockOriginalMouseInput = @BlockOriginalMouseInput;
            """;
}
