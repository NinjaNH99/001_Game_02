using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private const string FACEBOOK_URL = "https://www.facebook.com";

    public GameObject canvas2;
    public GameObject boostSpeedButton;
    public GameObject pauseMenu;
    public GameObject statusBar;
    public GameObject tutorialBar;

    private float time;

    private void Awake()
    {
        time = 1f;
        canvas2.SetActive(false);
        pauseMenu.SetActive(false);
        statusBar.SetActive(true);
        tutorialBar.SetActive(true);
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnRateClick()
    {
        Application.OpenURL(FACEBOOK_URL);
    }

    public void OnTutorialClick()
    {

    }

    // Pause Menu

    public void OnPauseMenu()
    {
        canvas2.SetActive(!canvas2.activeSelf);
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        statusBar.SetActive(!statusBar.activeSelf);
        if(!tutorialBar.activeSelf)
            tutorialBar.SetActive(false);
        if (pauseMenu.activeSelf)
        {
            time = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
            Time.timeScale = time;
    }

    public void OnRestartClick()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OnMenuClick()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void OnSoundClick()
    {

    }

    // END

    // Lose Menu

    public void OnLoseMenu()
    {
        canvas2.SetActive(!canvas2.activeSelf);
    }

    // END

    public void OnBoostSpeed()
    {
        Time.timeScale = 2f + (GameController.amountBalls / 80f);
        boostSpeedButton.SetActive(false);
    }

}
