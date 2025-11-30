using UnityEngine;

public class DynamicAudioSwitcher : MonoBehaviour
{
    [SerializeField] public AudioClip[] dynamicBGM_Array;
    public static AudioSource audioSource;

    public static DynamicAudioSwitcher instance;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (instance != null && instance != this)  // singleton
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }


    public void SwitchAudioSeamless(AudioClip clip)
    {
        float currentTimeRate = audioSource.time / audioSource.clip.length;
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.time = currentTimeRate * audioSource.clip.length;
    }

}
