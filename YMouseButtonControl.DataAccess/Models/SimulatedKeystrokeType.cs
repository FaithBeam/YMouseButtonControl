namespace YMouseButtonControl.DataAccess.Models;

public enum SimulatedKeystrokeType
{
    AsMousePressedAndReleasedActionType,
    DuringMouseActionType,
    InAnotherThreadPressedActionType,
    InAnotherThreadReleasedActionType,
    MouseButtonPressedActionType,
    MouseButtonReleasedActionType,
    RepeatedlyWhileButtonDownActionType,
    StickyHoldActionType,
    StickyRepeatActionType,
}
