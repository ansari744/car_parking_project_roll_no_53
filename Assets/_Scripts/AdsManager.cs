using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    private string appID = "ca-app-pub-3940256099942544~3347511713"; // Test App ID
    private InterstitialAd interstitialAd;
    private BannerView bannerView;

#if UNITY_ANDROID
    private string interstitialAdUnit = "ca-app-pub-3940256099942544/1033173712"; // Test Interstitial Ad
    private string bannerAdUnit = "ca-app-pub-3940256099942544/6300978111"; // Test Banner Ad
#elif UNITY_IPHONE
    private string interstitialAdUnit = "ca-app-pub-3940256099942544/4411468910";
#else
    private string interstitialAdUnit = "unused";
#endif

    private void Start()
    {
        // Initialize Google Mobile Ads
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK initialized.");
            LoadInterstitialAd();
            LoadBannerAd();
        });
    }

    // ---------------- Banner Ad Functions ----------------
    public void LoadBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(bannerAdUnit, AdSize.Banner, AdPosition.Top);
        AdRequest adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);
        RegisterBannerEvents();
    }

    private void RegisterBannerEvents()
    {
        bannerView.OnBannerAdLoaded += () => Debug.Log("Banner loaded.");
        bannerView.OnBannerAdLoadFailed += (error) => Debug.LogError($"Banner failed to load: {error}");
        bannerView.OnAdClicked += () => Debug.Log("Banner clicked.");
        bannerView.OnAdFullScreenContentOpened += () => Debug.Log("Banner opened full screen.");
        bannerView.OnAdFullScreenContentClosed += () => Debug.Log("Banner closed full screen.");
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }

    // ---------------- Interstitial Ad Functions ----------------
    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(interstitialAdUnit, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError($"Interstitial failed to load: {error}");
                return;
            }

            interstitialAd = ad;
            RegisterInterstitialEvents(interstitialAd);
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready.");
            LoadInterstitialAd(); // Reload if not ready
        }
    }

    private void RegisterInterstitialEvents(InterstitialAd ad)
    {
        ad.OnAdImpressionRecorded += () => Debug.Log("Interstitial recorded an impression.");
        ad.OnAdClicked += () => Debug.Log("Interstitial clicked.");
        ad.OnAdFullScreenContentOpened += () => Debug.Log("Interstitial opened full screen.");
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial closed. Reloading...");
            LoadInterstitialAd();
        };
        ad.OnAdFullScreenContentFailed += (error) =>
        {
            Debug.LogError($"Interstitial failed to open full screen: {error}");
            LoadInterstitialAd();
        };
    }
}
