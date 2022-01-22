/// <summary>
/// The current action state if it is dividable by 2 it is a moving action
/// </summary>
public enum ActionState : byte
{
    RelaxingAtHome = 1,
    GoingHome = 2,
    Waiting = 3,
    GoingToWork = 4,
    Working = 5,
    GoingShopping = 6,
    Shopping = 7,
    GoingHomeFromShopping = 8,
    InQuarantine = 9,
}