using UnityEngine;

public class House : Building
{
    public bool Occupied { get; private set; }

    public void Start()
    {
        Capacity = int.MaxValue;
        CurrentSize = 0;
        BuildingType = BuildingType.House;
    }

    public void Occupy()
    {
        this.Occupied = true;
    }
}