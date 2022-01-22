using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour
{
    public Text ValueLabel;

    private void Start()
    {
        UpdateLabel();
    }

    public void UpdateLabel()
    {
        ValueLabel.text = this.gameObject.GetComponent<Slider>().value.ToString();
    }
}