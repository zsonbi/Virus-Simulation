using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public int Capacity { get; protected set; }
    public int CurrentSize { get; protected set; }

    public BuildingType BuildingType { get; protected set; }

    public Vector2 NearestRoad { get; private set; }

    public bool HasFreeSpace()
    {
        return CurrentSize < Capacity;
    }

    public void SetNearestRoadLocation(Vector2 roadLocation)
    {
        this.NearestRoad = roadLocation;
    }
}