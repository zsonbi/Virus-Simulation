using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the status informations
/// Also updates the labels if necessary
/// </summary>
public class StatusHandler : MonoBehaviour
{
    [Header("The Label which will display the day count")]
    public Text DayCountText;

    [Header("The Label which will display the lockdown state")]
    public Text LockDownText;

    [Header("The Label which will display the people count")]
    public Text PeopleCountText;

    [Header("The Label which will display the underage people count")]
    public Text UnderagePeopleCountText;

    [Header("The Label which will display the adult people count")]
    public Text AdultPeopleCountText;

    [Header("The Label which will display the infected count")]
    public Text InfectedCountText;

    [Header("The Label which will display the underage infected count")]
    public Text UnderageInfectedText;

    [Header("The Label which will display the adult infected count")]
    public Text AdultInfectedText;

    private int dayCounter = 0;
    private int initialPeopleCount = 0; //The starting number of people
    private int initialAdultPeopleCount = 0; //The starting number of adults
    private int adultPeopleCount = 0; //The current number of adult people
    private int initialUnderagePeopleCount = 0; //The starting number of underage people
    private int underagePeopleCount = 0; //The current number of underage people
    private int infectedCount = 0; //The current infected count
    private int underAgeInfectedCount = 0; //The current number of underage infected
    private int adultInfectedCount = 0; //The current number of adult infected
    private int currPeople = 0; //The current people

    //--------------------------------------------------------------------------
    //Runs before first frame update
    private void Start()
    {
        initialPeopleCount = adultPeopleCount + underagePeopleCount;
        initialAdultPeopleCount = adultPeopleCount;
        initialUnderagePeopleCount = underagePeopleCount;
        UpdatePeopleCount();
    }

    //----------------------------------------------------------
    /// <summary>
    /// Increases the number of people in the specified age group
    /// </summary>
    /// <param name="ageGrp">Which group to increase</param>
    public void IncreasePeople(AgeGroup ageGrp)
    {
        switch (ageGrp)
        {
            case AgeGroup.Underage:
                underagePeopleCount++;
                break;

            case AgeGroup.Adult:
                adultPeopleCount++;
                break;

            default:
                break;
        }
        currPeople++;
    }

    //----------------------------------------------------------
    /// <summary>
    /// Decreases the number of people in the specified age group
    /// </summary>
    /// <param name="ageGrp">Which group to increase</param>
    public void DecreasePeople(AgeGroup ageGrp)
    {
        switch (ageGrp)
        {
            case AgeGroup.Underage:
                underagePeopleCount--;
                break;

            case AgeGroup.Adult:
                adultPeopleCount--;
                break;

            default:
                break;
        }
        currPeople--;
        //Update the texts
        UpdatePeopleCount();
    }

    //----------------------------------------------------------
    /// <summary>
    /// Hides or Shows the canvas of the statuses
    /// </summary>
    public void StatusButtonClick()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    //-----------------------------------------------------
    /// <summary>
    /// Updates the day time
    /// </summary>
    /// <param name="dayCount"></param>
    public void UpdateDayTime(int dayCount)
    {
        DayCountText.text = $"Day: {dayCount}";
        this.dayCounter = dayCount;
    }

    //------------------------------------------------------
    // Updates the people count text
    private void UpdatePeopleCount()
    {
        PeopleCountText.text = $"People: {currPeople}/{initialPeopleCount}";
        UpdateInfectedCount();
        UpdateAdultPeopleCount();
        UpdateUnderAgePeopleCount();
    }

    //------------------------------------------------------
    // Updates underage people count text
    private void UpdateUnderAgePeopleCount()
    {
        this.UnderagePeopleCountText.text = $"Underage people: {underagePeopleCount}/{initialUnderagePeopleCount}";
    }

    //------------------------------------------------------
    // Updates adult people count text
    private void UpdateAdultPeopleCount()
    {
        this.AdultPeopleCountText.text = $"Adult people: {adultPeopleCount}/{initialAdultPeopleCount}";
    }

    //------------------------------------------------------
    //Updates infected count text
    private void UpdateInfectedCount()
    {
        InfectedCountText.text = $"Infected people: {infectedCount}/{currPeople}";
        UpdateUnderAgeInfectedCount();
        UpdateAdultInfectedCount();
    }

    //------------------------------------------------------
    //Updates underage infected count text
    private void UpdateUnderAgeInfectedCount()
    {
        this.UnderageInfectedText.text = $"Underage infected: {underAgeInfectedCount}/{underagePeopleCount}";
    }

    //------------------------------------------------------
    //Updates adult infected count text
    private void UpdateAdultInfectedCount()
    {
        this.AdultInfectedText.text = $"Adult infected: {adultInfectedCount}/{adultPeopleCount}";
    }

    //------------------------------------------------------
    /// <summary>
    /// Increase infected count in the specified age group
    /// </summary>
    /// <param name="ageGrp">Which group to increase</param>
    public void IncreaseInfectedCount(AgeGroup ageGrp)
    {
        this.infectedCount++;

        switch (ageGrp)
        {
            case AgeGroup.Underage:
                underAgeInfectedCount++;
                break;

            case AgeGroup.Adult:
                adultInfectedCount++;
                break;

            default:
                break;
        }
        UpdateInfectedCount();
    }

    /// <summary>
    /// Decrease infected count in the specified age group
    /// </summary>
    /// <param name="ageGrp">Which group to decrease</param>
    public void DecreaseInfectedCount(AgeGroup ageGrp)
    {
        this.infectedCount--;

        switch (ageGrp)
        {
            case AgeGroup.Underage:
                underAgeInfectedCount--;
                break;

            case AgeGroup.Adult:
                adultInfectedCount--;
                break;

            default:
                break;
        }
        UpdateInfectedCount();
    }

    //------------------------------------------------------------------------
    /// <summary>
    /// Updates the lockdown text
    /// </summary>
    public void UpdateLockDownState()
    {
        LockDownText.text = $"Lockdown {(Settings.Lockdown ? "on" : "off")}";
    }

    //------------------------------------------------------------------------
    /// <summary>
    /// Writes out every status varriable in a string separated by a ';'
    /// </summary>
    /// <returns>The string</returns>
    public override string ToString()
    {
        return $"{dayCounter}" +
            $";{currPeople}" +
            $";{underagePeopleCount}" +
            $";{adultPeopleCount}" +
            $";{infectedCount}" +
            $";{underAgeInfectedCount}" +
            $";{adultInfectedCount}" +
            $"{(Settings.Lockdown ? "on" : "off")}";
    }
}