using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static UnityEvent loseTrigger = new();
    public static UnityEvent winTrigger = new();

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject pauseScreen;
    public static GameObject staticPauseScreen;

    public static bool isGameOver = false;
    public static bool isPaused = false;

    public static List<GameObject> enemies = new();

    void Awake()
    {
        staticPauseScreen = pauseScreen;
        isGameOver = false;
        isPaused = false;
        enemies = new();
        loseTrigger.AddListener(LoseGame);
        winTrigger.AddListener(WinGame);
    }

    public static void Pause()
    {
        if (!isGameOver)
        {
            if (isPaused)
            {
                DynamicAudioSwitcher.audioSource.UnPause();
                Time.timeScale = 1;
            }
            else
            {
                DynamicAudioSwitcher.audioSource.Pause();
                Time.timeScale = 0;
            }
            
            staticPauseScreen.SetActive(!staticPauseScreen.activeSelf);
            isPaused = staticPauseScreen.activeSelf;
        }
    }

    public void LoseGame()
    {
        isGameOver = true;
        DynamicAudioSwitcher.audioSource.Stop();
        loseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        isGameOver = true;
        DynamicAudioSwitcher.audioSource.Stop();
        winScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
