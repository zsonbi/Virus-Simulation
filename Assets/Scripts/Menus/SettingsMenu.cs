using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider RealTimeToSimulationTimeSlider;

    public void SettingsClick()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void SpeedSlider()
    {
        Settings.RealTimeToSimulationTime = RealTimeToSimulationTimeSlider.value;
    }
}