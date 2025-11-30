using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public static AudioVolumeControl instance;


    void Awake()
    {
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


    void Start()
    {
        if (!PlayerPrefs.HasKey("BGM"))
        {
            PlayerPrefs.SetFloat("BGM", 0);
            Save("BGM", 0);
        }
        else
        {
            Load("BGM");
        }

        if (!PlayerPrefs.HasKey("SFX"))
        {
            PlayerPrefs.SetFloat("SFX", 0);
            Save("SFX", 0);
        }
        else
        {
            Load("SFX");
        }

        if (!PlayerPrefs.HasKey("Master"))
        {
            PlayerPrefs.SetFloat("Master", 0);
            Save("Master", 0);
        }
        else
        {
            Load("Master");
        }
    }


    public void ChangeVolumeBGM(float value)
    {
        audioMixer.SetFloat("BGM", 100 * value - 100);
        Save("BGM", value);
    }


    public void ChangeVolumeSFX(float value)
    {
        audioMixer.SetFloat("SFX", 100 * value - 100);
        Save("SFX", value);
    }


    public void ChangeVolumeMaster(float value)
    {
        audioMixer.SetFloat("Master", 100 * value - 100);
        Save("Master", value);
    }

    private void Load(string dataName)
    {
        audioMixer.SetFloat(dataName, 100 * PlayerPrefs.GetFloat(dataName) - 100);
    }

    private void Save(string dataName, float value)
    {
        PlayerPrefs.SetFloat(dataName, value);
    }

}
