using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
            PlayerPrefs.SetFloat("Volume", 0.5f);
        else
            Load();
    }
    private void Update()
    {
        AudioListener.volume = volumeSlider.value;
    }
    
    public void Save() => PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    private void Load() => volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
}
