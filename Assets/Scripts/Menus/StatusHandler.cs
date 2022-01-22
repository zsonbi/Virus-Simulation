using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusHandler : MonoBehaviour
{
    public World world;
    public Text DayCountText;
    public Text PeopleCountText;
    public Text InfectedCountText;

    private int initialPeopleCount = 0;
    private int infectedCount = 0;
    private int currPeople = 0;

    public void SetInitialPeopleCount(int count)
    {
        initialPeopleCount = count;
        UpdatePeopleCount(count);
        UpdateInfectedCount();
    }

    public void StatusButtonClick()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void UpdateDayTime(int dayCount)
    {
        DayCountText.text = $"day: {dayCount}";
    }

    public void UpdatePeopleCount(int count)
    {
        PeopleCountText.text = $"People: {count}/{initialPeopleCount}";
        currPeople = count;
        UpdateInfectedCount();
    }

    public void UpdateInfectedCount()
    {
        InfectedCountText.text = $"Infected people: {infectedCount}/{currPeople}";
    }

    public void IncreaseInfectedCount()
    {
        this.infectedCount++;
        UpdateInfectedCount();
    }

    public void DecreaseInfectedCount()
    {
        this.infectedCount--;
        UpdateInfectedCount();
    }
}