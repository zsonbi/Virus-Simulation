using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : Building
{
    public void EnterMarket()
    {
        CurrentSize++;
    }

    public void LeaveMarket()
    {
        CurrentSize--;
    }

    public void Start()
    {
        Capacity = 50;
        CurrentSize = 0;
        BuildingType = BuildingType.Market;
    }
}