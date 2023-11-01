using UnityEngine;
using static EyeCandyX.Configuration;

public class TimeManager
{
    
    public static float CustomTimeScale
    {

        get
        {
            Preset PresetInstance = new Preset();
            return PresetInstance.customTimeScale;
        }
        set
        {
            Preset PresetInstance = new Preset();
            PresetInstance.customTimeScale = value;
            Time.timeScale = PresetInstance.customTimeScale;
        }
    }
}
