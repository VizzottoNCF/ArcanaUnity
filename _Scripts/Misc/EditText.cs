using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditText : MonoBehaviour
{
    public TMP_Text text;
    public Slider slider;

    public void UpdateText()
    {
        float val = slider.value * 100;
        text.text = val.ToString("0");
    }
}
