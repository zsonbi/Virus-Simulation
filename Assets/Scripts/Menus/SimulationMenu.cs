using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationMenu : MonoBehaviour
{
    public World world;
    public Text DayText;

    public void UpdateDayText()
    {
        DayText.text = $"Day: {this.world.DayCounter}";
    }

    public void UpdateNumberOfPeople()
    {
    }
}