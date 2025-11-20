using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static UnityEvent loseTrigger = new();
    public static UnityEvent winTrigger = new();

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    public static List<GameObject> enemies = new();

    void Awake()
    {
        enemies = new();
        loseTrigger.AddListener(LoseGame);
        winTrigger.AddListener(WinGame);
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        loseScreen.SetActive(true);
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }
}
