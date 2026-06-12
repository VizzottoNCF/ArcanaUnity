using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    private GameController _gameController;
    [SerializeField] private GameObject pauseMenu_UI;

    private void Start() => _gameController = GetComponent<GameController>();

    private void Update()
    {
        if (_gameController.IsDead) { return; }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) { rf_Resume(); }
            else { rf_Pause(); } }
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
