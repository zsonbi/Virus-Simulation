using UnityEngine;

/// <summary>
/// Some kind of building
/// </summary>
public abstract class Building : MonoBehaviour
{
    /// <summary>
    /// The maximum size of the building
    /// </summary>
    public int Capacity { get; protected set; }

    /// <summary>
    /// The current size of the building
    /// </summary>
    public int CurrentSize { get; protected set; }

    /// <summary>
    /// The type of the building
    /// </summary>
    public BuildingType BuildingType { get; protected set; }

    /// <summary>
    /// The enterance to the building
    /// </summary>
    public Vector2 NearestRoad { get; private set; }

    //----------------------------------------------------------------------
    /// <summary>
    /// Does it has free capacity
    /// </summary>
    /// <returns>true-has, false-doesn't has</returns>
    public bool HasFreeSpace()
    {
        return CurrentSize < Capacity;
    }

    //-----------------------------------------------------------------------
    /// <summary>
    /// Set the enterance
    /// </summary>
    /// <param name="roadLocation">The location of the enterance</param>
    public void SetNearestRoadLocation(Vector2 roadLocation)
    {
        this.NearestRoad = roadLocation;
    }
}