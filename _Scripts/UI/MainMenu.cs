using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas settingsMenu;

    public void rf_PlayGame()
    {
        SceneManager.LoadScene("Level_0");
    }
    public void rf_QuitGame()
    {

    }
    public void rf_SettingsMenu()
    {
        settingsMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
    }
    public void rf_MainMenu()
    {
        settingsMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}
