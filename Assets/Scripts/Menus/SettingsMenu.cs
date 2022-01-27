using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the settings menu
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    [Header("The slider which stores the speed value")]
    public Slider RealTimeToSimulationTimeSlider;

    [Header("The slider which stores the infection rate value")]
    public Slider InfectionRateSlider;

    //Runs before first frame update
    private void Start()
    {
        RealTimeToSimulationTimeSlider.value = Settings.RealTimeToSimulationTime;
        InfectionRateSlider.value = Settings.InfectionRateMultiplier;
    }

    //----------------------------------------------------
    /// <summary>
    /// Hides or shows the settings
    /// </summary>
    public void SettingsClick()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    //---------------------------------------------------------
    /// <summary>
    /// Adjusts the speed to the slider's value
    /// </summary>
    public void SpeedSlider()
    {
        Settings.RealTimeToSimulationTime = RealTimeToSimulationTimeSlider.value;
    }

    //---------------------------------------------------------
    //Adjust the infection rate multiplier to the slider's value
    public void InfectionRateSliderChanged()
    {
        Settings.InfectionRateMultiplier = InfectionRateSlider.value;
    }

    //--------------------------------------------------------
    /// <summary>
    /// Returns the user back to the main menu
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}