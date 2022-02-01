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

    [Header("The input field where the daily vaccine production is given")]
    public InputField DailyVaccineText;

    [Header("The input field where the vaccines duration will be stored at")]
    public InputField VaccineDuration;

    //Runs before first frame update
    private void Start()
    {
        RealTimeToSimulationTimeSlider.value = Settings.RealTimeToSimulationTime;
        InfectionRateSlider.value = Settings.InfectionRateMultiplier;
        DailyVaccineText.text = Settings.DailyVaccineAmount.ToString();
        VaccineDuration.text = (Settings.VaccineImmunityTime / 86400f).ToString();
    }

    //----------------------------------------------------
    /// <summary>
    /// Hides or shows the settings
    /// </summary>
    public void SettingsClick()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    //-----------------------------------------------------------
    /// <summary>
    /// Sets the lockdown varriable to true or false
    /// </summary>
    public void LockDownClick()
    {
        Settings.Lockdown = !Settings.Lockdown;
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
    ///<summary>
    ///Adjust the infection rate multiplier to the slider's value
    ///</summary>
    public void InfectionRateSliderChanged()
    {
        Settings.InfectionRateMultiplier = InfectionRateSlider.value;
    }

    //-------------------------------------------------------------
    /// <summary>
    /// Adjust the number of daily vaccines
    /// </summary>
    public void DailyVaccinesTextChanged()
    {
        Settings.DailyVaccineAmount = System.Convert.ToInt32(DailyVaccineText.text);
    }

    //---------------------------------------------------------------
    /// <summary>
    /// The vaccine duration text changed
    /// </summary>
    public void VaccineDurationChanged()
    {
        Settings.VaccineImmunityTime = (float)(System.Convert.ToDouble(VaccineDuration.text.Replace('.', ',')) * 86400);
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