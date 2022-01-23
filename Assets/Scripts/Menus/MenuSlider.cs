using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the setting's slider updates
/// </summary>
public class MenuSlider : MonoBehaviour
{
    [Header("Where should the value be displayed")]
    public Text ValueLabel;

    //Runs before the first frame update
    private void Start()
    {
        UpdateLabel();
    }

    //------------------------------------------------------
    /// <summary>
    /// Update the value label
    /// </summary>
    public void UpdateLabel()
    {
        ValueLabel.text = this.gameObject.GetComponent<Slider>().value.ToString();
    }
}