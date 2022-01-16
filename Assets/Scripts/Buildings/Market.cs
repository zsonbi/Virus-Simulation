/// <summary>
/// A market
/// </summary>
public class Market : Building
{
    /// <summary>
    /// Enter the market
    /// </summary>
    public void EnterMarket()
    {
        CurrentSize++;
    }

    //-----------------------------------------------------
    /// <summary>
    /// Leave the market
    /// </summary>
    public void LeaveMarket()
    {
        CurrentSize--;
    }

    //------------------------------------------------------------
    //Runs when the script is loaded
    private void Awake()
    {
        Capacity = 50;
        CurrentSize = 0;
        BuildingType = BuildingType.Market;
    }
}