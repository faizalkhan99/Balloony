using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Ump.Api;
using UnityEngine.SceneManagement;

public class BroAdsMan : MonoBehaviour
{
    public static BroAdsMan Instance { get; private set; }

    [Header("AdMob IDs")]
    [SerializeField] private string _androidGameId; // Not strictly needed for AdMob init, but good for reference
    [SerializeField] private string _iOSGameId;

    [Header("Ad Units")]
    public InterstitialAdUnit InterstitialAdUnit;
    //public RewardedAdUnit SecondChanceRewarded;
    //public RewardedAdUnit X2Rewarded;
    public AppOpenAdUint AppOpenAdUnit;
    public BannerAdUnit BannerAdUnit;

    [Header("Settings")]
    [SerializeField] private int AdFreq = 3;
    private int AdCount = 0;

    public static bool IsReady;

    private void Awake()
    {
        // Singleton Implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    [Obsolete]
    private void Start()
    {
        InitializeAds();
    }


    // Separate the actual AdMob Start
    [Obsolete]
    public void RunAdMobInitialization(Action onComplete)
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initstatus =>
        {
            IsReady = true;
            LoadAds();
            onComplete?.Invoke();
        });
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 3. THIS RUNS EVERY TIME SCENE RELOADS
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ShowBanner();
    }

    [Obsolete]
    public void InitializeAds()
    {
        // 1. Configure Threading (Critical for Unity)
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        //MobileAdsEventExecutor.ExecuteInUpdate(null);

        // 2. Initialize SDK
        MobileAds.Initialize(initstatus =>
        {
            Dictionary<string, AdapterStatus> map = initstatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.Ready:
                        Log("Adapter: " + className + " is initialized.");
                        break;
                    case AdapterState.NotReady:
                        Log("Adapter: " + className + " is NOT initialized.");
                        break;
                }
            }

            OnInitializationComplete();
        });

        // 3. Set Test Device IDs (Optional but recommended)

        // List<string> testDeviceIds = new List<string>
        // {
        //     AdRequest.TestDeviceSimulator,
        //     "f7a952a9-44dd-4ba6-a5b5-10ca1ad1abe1" //Redmi 9A
        // };
        // RequestConfiguration requestConfiguration = new RequestConfiguration
        // {
        //     TestDeviceIds = testDeviceIds
        // };
        // MobileAds.SetRequestConfiguration(requestConfiguration);


        // 4. App Open Ad Events
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnInitializationComplete()
    {
        IsReady = true;
        Log("Google Mobile Ads initialization complete. Loading Ads...");
        // BootstrapManager.Instance?.NotifyAdsDone();
        // Trigger initial loads
        LoadAds();
    }

    private void LoadAds()
    {
        // Load all units
        InterstitialAdUnit?.LoadAd();
        //SecondChanceRewarded?.LoadAd();
        //X2Rewarded?.LoadAd();
        AppOpenAdUnit?.LoadAd();
        BannerAdUnit?.LoadAd();
    }

    private void OnAppStateChanged(AppState state)
    {
        // Handle App Open Ad on Foreground
        if (state == AppState.Foreground)
        {
            if (AppOpenAdUnit != null && AppOpenAdUnit.IsAdAvailable)
            {
                AppOpenAdUnit.ShowAd(null, null);
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
            // Cleanup all ads
            InterstitialAdUnit?.CleanUp();
            //SecondChanceRewarded?.CleanUp();
            //X2Rewarded?.CleanUp();
            AppOpenAdUnit?.CleanUp();
            BannerAdUnit?.CleanUp();
        }
    }

    // --- Static Helper Methods ---

    public static void ShowInterstitialAd(Action onDone)
    {
        if (Instance == null || !IsReady)
        {
            onDone?.Invoke();
            return;
        }

        Instance.AdCount++;

        // Frequency Check
        if (Instance.AdCount % Instance.AdFreq != 0)
        {
            onDone?.Invoke();
            return;
        }

        if (Instance.InterstitialAdUnit != null)
        {
            Instance.InterstitialAdUnit.ShowAd(onDone);
        }
        else
        {
            onDone?.Invoke();
        }
    }


    //Don't need these at the moment but might in future.

    // public static void ShowSecondChanceRewarded(Action onReward, Action onFail)
    // {
    //     if (Instance != null && Instance.SecondChanceRewarded != null)
    //     {
    //         Instance.SecondChanceRewarded.ShowAd(onReward, onFail);
    //     }
    //     else
    //     {
    //         onFail?.Invoke();
    //     }
    // }

    // public static void ShowX2Rewarded(Action onReward, Action onFail)
    // {
    //     if (Instance != null && Instance.X2Rewarded != null)
    //     {
    //         Instance.X2Rewarded.ShowAd(onReward, onFail);
    //     }
    //     else
    //     {
    //         onFail?.Invoke();
    //     }
    // }

    public static void ShowBanner()
    {
        if (Instance == null || !IsReady || Instance.BannerAdUnit == null) return;

        // ONLY Load if we don't have a view yet. 
        // If it exists, just call Show() to unhide it.
        Instance.BannerAdUnit.ShowAd();
    }
    public static void HideBanner() => Instance?.BannerAdUnit?.HideAd();

    public static void Pause()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public static void Resume()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void Log(string msg)
    {
        Debug.Log($"[BroAdsMan] {msg}");
    }
}

// =========================================================
// ABSTRACT BASE CLASS
// =========================================================
// public abstract class AdUnit
// {
//     [Header("Placement IDs")]
//     [SerializeField] private string _androidId;
//     [SerializeField] private string _iOSId;

//     [SerializeField] private AdSettings _settings;

//     public string AdUnitId => _settings.GetId();

//     // This property automatically picks the right ID based on the platform
//     public string AdUnitId
//     {
//         get
//         {
// #if UNITY_ANDROID
//             return _androidId.Trim();
// #elif UNITY_IPHONE
//             return _iOSId.Trim();
// #else
//             return _androidId.Trim(); // Default to Android in Editor
// #endif
//         }
//     }

//     protected bool isLoaded = false;
//     public bool IsLoaded => isLoaded;

//     // Callbacks used during Show
//     protected Action onComplete;
//     protected Action onFail;

//     public abstract void LoadAd(); // Void, because AdMob is callback based
//     public abstract void ShowAd(Action onComplete, Action onFail = null);
//     public abstract void CleanUp();

//     protected void Log(string msg)
//     {
//         Debug.Log($"[{this.GetType().Name}] {msg}");
//     }
// }




[Serializable]
public abstract class AdUnit
{
    [Header("Ad Configuration")]
    [SerializeField] private AdSettings _settings;

    // This property now pulls the correct ID (Real or Test) directly from your ScriptableObject
    public string AdUnitId
    {
        get
        {
            if (_settings == null)
            {
                Debug.LogError($"[{this.GetType().Name}] AdSettings asset is missing in the Inspector!");
                return string.Empty;
            }
            return _settings.GetId();
        }
    }

    protected bool isLoaded = false;
    public bool IsLoaded => isLoaded;

    // Callbacks used during Show
    protected Action onComplete;
    protected Action onFail;

    public abstract void LoadAd();
    public abstract void ShowAd(Action onComplete, Action onFail = null);
    public abstract void CleanUp();

    protected void Log(string msg)
    {
        Debug.Log($"[{this.GetType().Name}] {msg}");
    }
}





// =========================================================
// INTERSTITIAL
// =========================================================
[Serializable]
public class InterstitialAdUnit : AdUnit
{
    private InterstitialAd _ad;

    public override void LoadAd()
    {
        if (string.IsNullOrEmpty(AdUnitId)) return;

        // Clean up old ad before loading new one
        CleanUp();

        Log("Loading Interstitial...");
        var request = new AdRequest();

        InterstitialAd.Load(AdUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Log("Load Failed: " + error);
                isLoaded = false;
                return;
            }

            _ad = ad;
            isLoaded = true;
            Log("Loaded Successfully.");

            // Register Event Handler
            _ad.OnAdFullScreenContentClosed += HandleAdClosed;
            _ad.OnAdFullScreenContentFailed += HandleAdFailed;
        });
    }

    public override void ShowAd(Action onComplete, Action onFail = null)
    {
        this.onComplete = onComplete;
        this.onFail = onFail;

        if (_ad != null && _ad.CanShowAd())
        {
            BroAdsMan.Pause();
            _ad.Show();
        }
        else
        {
            Log("Not ready.");
            onComplete?.Invoke(); // Don't block game
            LoadAd(); // Try loading again
        }
    }

    private void HandleAdClosed()
    {
        Log("Closed.");
        BroAdsMan.Resume();
        onComplete?.Invoke();
        LoadAd(); // Auto Reload
    }

    private void HandleAdFailed(AdError error)
    {
        Log("Show Failed: " + error);
        BroAdsMan.Resume();
        onComplete?.Invoke(); // Treat fail as closed so game continues
        LoadAd();
    }

    public override void CleanUp()
    {
        if (_ad != null)
        {
            _ad.Destroy();
            _ad = null;
        }
        isLoaded = false;
    }
}

// =========================================================
// REWARDED
// =========================================================
[Serializable]
public class RewardedAdUnit : AdUnit
{
    private RewardedAd _ad;

    public override void LoadAd()
    {
        if (string.IsNullOrEmpty(AdUnitId)) return;
        CleanUp();

        Log("Loading Rewarded...");
        var request = new AdRequest();

        RewardedAd.Load(AdUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Log("Load Failed: " + error);
                isLoaded = false;
                return;
            }

            _ad = ad;
            isLoaded = true;
            Log("Loaded Successfully.");

            _ad.OnAdFullScreenContentClosed += HandleAdClosed;
            _ad.OnAdFullScreenContentFailed += HandleAdFailed;
        });
    }

    public override void ShowAd(Action onComplete, Action onFail = null)
    {
        this.onComplete = onComplete; // This is the Reward Action
        this.onFail = onFail;         // This is the Fail Action

        if (_ad != null && _ad.CanShowAd())
        {
            BroAdsMan.Pause();
            _ad.Show((Reward reward) =>
            {
                Log($"User Earned Reward: {reward.Amount} {reward.Type}");
                this.onComplete?.Invoke();
                this.onComplete = null; // Ensure we don't call it twice
            });
        }
        else
        {
            Log("Not ready.");
            this.onFail?.Invoke();
            LoadAd();
        }
    }

    private void HandleAdClosed()
    {
        Log("Closed.");
        BroAdsMan.Resume();
        // If onComplete is still not null, it means they closed it without finishing (refused)
        if (this.onComplete != null)
        {
            Log("Closed without reward.");
            this.onFail?.Invoke();
        }
        LoadAd();
    }

    private void HandleAdFailed(AdError error)
    {
        Log("Show Failed: " + error);
        BroAdsMan.Resume();
        this.onFail?.Invoke();
        LoadAd();
    }

    public override void CleanUp()
    {
        if (_ad != null)
        {
            _ad.Destroy();
            _ad = null;
        }
        isLoaded = false;
    }
}

// =========================================================
// APP OPEN
// =========================================================
[Serializable]
public class AppOpenAdUint : AdUnit
{
    private AppOpenAd _ad;
    private DateTime _expireTime;

    public bool IsAdAvailable => _ad != null && isLoaded && DateTime.Now < _expireTime;

    public override void LoadAd()
    {
        if (string.IsNullOrEmpty(AdUnitId)) return;
        // Don't cleanup if valid, to prevent flicker
        if (IsAdAvailable) return;

        CleanUp();

        Log("Loading App Open...");
        var request = new AdRequest();

        AppOpenAd.Load(AdUnitId, request, (AppOpenAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Log("Load Failed: " + error);
                isLoaded = false;
                return;
            }

            _ad = ad;
            isLoaded = true;
            _expireTime = DateTime.Now + TimeSpan.FromHours(4);
            Log("Loaded.");

            _ad.OnAdFullScreenContentClosed += HandleAdClosed;
            _ad.OnAdFullScreenContentFailed += HandleAdFailed;
        });
    }

    public override void ShowAd(Action onComplete, Action onFail = null)
    {
        if (IsAdAvailable)
        {
            _ad.Show();
        }
        else
        {
            LoadAd();
        }
    }

    private void HandleAdClosed()
    {
        Log("Closed.");
        CleanUp();
        LoadAd();
    }

    private void HandleAdFailed(AdError error)
    {
        Log("Failed: " + error);
        CleanUp();
        LoadAd();
    }

    public override void CleanUp()
    {
        if (_ad != null)
        {
            _ad.Destroy();
            _ad = null;
        }
        isLoaded = false;
    }
}

// =========================================================
// BANNER
// =========================================================
[Serializable]
public class BannerAdUnit : AdUnit
{
    private BannerView _bannerView;

    public override void LoadAd()
    {
        if (string.IsNullOrEmpty(AdUnitId)) return;

        // Only cleanup if we actually want to refresh the ad content
        if (_bannerView != null) return;

        Log("Creating Banner View...");
        // Use standard Banner size first to verify it works, then switch back to Adaptive
        _bannerView = new BannerView(AdUnitId, AdSize.Banner, AdPosition.Bottom);

        var request = new AdRequest();
        _bannerView.LoadAd(request);

        // Banners show automatically upon LoadAd success in AdMob
        Log("Banner Load Requested.");
    }

    public override void ShowAd(Action onComplete = null, Action onFail = null)
    {
        if (_bannerView != null)
        {
            _bannerView.Show();
        }
        else
        {
            // If it's null, we MUST load it
            LoadAd();
        }
    }

    public void HideAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Hide();
        }
    }

    public override void CleanUp()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
}