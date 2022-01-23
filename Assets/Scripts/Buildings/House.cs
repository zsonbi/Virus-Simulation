/// <summary>
/// A single house
/// </summary>
public class House : Building
{
    /// <summary>
    /// Is it occupied yet
    /// </summary>
    public bool Occupied { get; private set; }

    //-----------------------------------------------------------
    //Runs before the first frame
    public void Awake()
    {
        Capacity = int.MaxValue;
        CurrentSize = 0;
        BuildingType = BuildingType.House;
    }

    //------------------------------------------------------------------
    /// <summary>
    /// Occupies the house
    /// </summary>
    public void Occupy()
    {
        this.Occupied = true;
    }
}