using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider RealTimeToSimulationTimeSlider;
    public Slider InfectionRateSlider;

    public void SettingsClick()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void SpeedSlider()
    {
        Settings.RealTimeToSimulationTime = RealTimeToSimulationTimeSlider.value;
    }

    public void InfectionRateSliderChanged()
    {
        Settings.InfectionRateMultiplier = InfectionRateSlider.value;
    }
}