using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] GameObject pauseMenu_UI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                rf_Resume();
            }
            else
            {
                rf_Pause();
            }
        }
    }

    private void rf_Resume()
    {
        pauseMenu_UI.SetActive(false);

        GameIsPaused = false;

        Time.timeScale = 1.0f;
    }

    private void rf_Pause()
    {
        pauseMenu_UI.SetActive(true);

        GameIsPaused = true;

        Time.timeScale = 0f;
    }

}
