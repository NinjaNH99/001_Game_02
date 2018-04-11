using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private const string FACEBOOK_URL = "https://www.facebook.com";

    public GameObject canvas2;
    public Button boostSpeedButton;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject statusBar;
    public GameObject tutorialBar;

    private GameController gameContr;

    private float time;

    private void Awake()
    {
        time = 1f;
        canvas2.SetActive(false);
        pauseMenu.SetActive(false);
        statusBar.SetActive(true);
        tutorialBar.SetActive(true);
        gameContr = GameController.Instance;
    }

    public void OnPlayClick()
    {
        //SceneManager.LoadScene("Game");
        LevelLoader.Instance.LoadLevel("Game");
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
        if (!tutorialBar.activeSelf)
            tutorialBar.SetActive(false);
        pauseMenu.GetComponent<UpdatePauseMenu>().UpdateGameStatus();
        time = Time.timeScale;
        Time.timeScale = 0f;
        pauseMenu.GetComponent<Animator>().SetTrigger("PanelON");
    }

    public void OffPauseMenu()
    {
        if (!tutorialBar.activeSelf)
            tutorialBar.SetActive(false);
        pauseMenu.GetComponent<Animator>().SetTrigger("PanelOFF");
        Time.timeScale = time;
        canvas2.SetActive(!canvas2.activeSelf);
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        statusBar.SetActive(!statusBar.activeSelf);
    }

    public void OnRestartClick()
    {
        gameContr.SaveDataRestart();
        LevelLoader.Instance.LoadLevel("Game");
    }

    public void OnMenuClick()
    {
        LevelLoader.Instance.LoadLevel("Main");
    }

    public void OnSoundClick()
    {

    }

    // END

    public void OnBoostSpeed()
    {
        Time.timeScale = 2f + (gameContr.amountBalls / 80f);
        boostSpeedButton.interactable = false;
        boostSpeedButton.GetComponent<Animator>().SetTrigger("BoostSpeed_Exit");
    }

}
