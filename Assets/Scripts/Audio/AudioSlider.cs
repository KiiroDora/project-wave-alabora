using UnityEngine;

public class AudioSlider : MonoBehaviour
{
    public void ChangeVolumeBGM(float value)
    {
        AudioVolumeControl.instance.ChangeVolumeBGM(value);
    }


    public void ChangeVolumeSFX(float value)
    {
        AudioVolumeControl.instance.ChangeVolumeSFX(value);
    }


    public void ChangeVolumeMaster(float value)
    {
        AudioVolumeControl.instance.ChangeVolumeMaster(value);
    }
}
