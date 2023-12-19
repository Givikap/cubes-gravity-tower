using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour {

    public GameObject menu, pauseButton;

    public AdsController ads;

    public void RestartGame() {
        Time.timeScale = 1.0f;
        menu.SetActive(false);

        SceneManager.LoadScene("Main");
    }

    public void MusicWork() {
        AudioListener.pause = !AudioListener.pause;
    }

    public void LoadShop() {
        SceneManager.LoadScene("Shop 1");
    }

    public void CloseShop() {
        SceneManager.LoadScene("Main");
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        menu.SetActive(true);
    }

    public void ResumeGame() {
        Time.timeScale = 1.0f;
        menu.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void NextPage() {
        SceneManager.LoadScene("Shop 2");
    }

    public void WatchVideo() {
        ads.PlayRewardedAd();
    }
}