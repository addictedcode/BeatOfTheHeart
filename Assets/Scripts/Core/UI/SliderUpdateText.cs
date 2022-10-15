using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderUpdateText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void OnSliderValueChange(float value)
    {
        text.text = value.ToString();
    }
}
