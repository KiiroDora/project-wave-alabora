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
            Time.timeScale = isPaused ? 1 : 0;
            staticPauseScreen.SetActive(!staticPauseScreen.activeSelf);
            isPaused = staticPauseScreen.activeSelf;
        }
    }

    public void LoseGame()
    {
        isGameOver = true;
        Time.timeScale = 0;
        loseScreen.SetActive(true);
    }

    public void WinGame()
    {
        isGameOver = true;
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }
}
