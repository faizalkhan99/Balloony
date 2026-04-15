using UnityEngine;

[CreateAssetMenu(fileName = "AdSettings", menuName = "Ads/AdSettings")]
public class AdSettings : ScriptableObject
{
    [Tooltip("Check this to use Google Test IDs. Uncheck for production.")]
    public bool UseTestAds = true;

    [Header("Android IDs")]
    public string AndroidRealId;
    public string AndroidTestId;

    [Header("iOS IDs")]
    public string iOSRealId;
    public string iOSTestId;

    /// <summary>
    /// Automatically returns the correct ID based on Platform and Test Mode.
    /// </summary>
    public string GetId()
    {
        if (UseTestAds)
        {
#if UNITY_ANDROID
            return AndroidTestId;
#elif UNITY_IOS
            return iOSTestId;
#else
            return AndroidTestId; // Default for Editor
#endif
        }
        else
        {
#if UNITY_ANDROID
            return AndroidRealId;
#elif UNITY_IOS
            return iOSRealId;
#else
            return AndroidRealId;
#endif
        }
    }
}