using UnityEngine;

public class VibrationSystem : PersistentSingleton<VibrationSystem>
{
    public bool VibrationEnabled = true; // Vibrations actives ou non.

    private void OnEnable()
    {
        VibrationEnabled = SaveSystem.Instance.LoadElement<bool>("_vibrationIsOn", true);
    }

    public void SetVibrationEnabled(bool enabled)
    {
        VibrationEnabled = enabled;
        SaveSystem.Instance.SaveElement("_vibrationIsOn", VibrationEnabled);
    }

    /// <summary>
    /// Déclenche des vibrations.
    /// </summary>
    /// <param name="strength">Force de la vibration.</param>
    /// <param name="duration">Durée de la vibration.</param>
    public void TriggerVibration(float strength, float duration)
    {
        if (!VibrationEnabled) return;
#if UNITY_ANDROID
        AndroidVibrate(strength, duration);
#elif UNITY_IOS
            iOSVibrate(strength);
#endif
    }

    /// <summary>
    /// Déclenche des vibrations sur Android.
    /// </summary>
    /// <param name="strength">Force de la vibration.</param>
    /// <param name="duration">Durée de la vibration.</param>
    private void AndroidVibrate(float strength, float duration)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            long milliseconds = (long)(duration * 1000);
            int amplitude = Mathf.Clamp((int)(strength * 255), 0, 255);

            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
            {
                if (vibrator != null)
                {
                    using (AndroidJavaClass vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect"))
                    {
                        AndroidJavaObject effect = vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude);
                        vibrator.Call("vibrate", effect);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Déclenche des vibrations sur iOS.
    /// </summary>
    /// <param name="strength">Force de la vibration.</param>
    private void iOSVibrate(float strength)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.Vibrate();
        }
    }
}
