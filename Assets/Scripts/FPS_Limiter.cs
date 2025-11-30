using UnityEngine;

public class FPS_Limiter : MonoBehaviour
{
    public int FPS = 60;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPS;
    }
} 
