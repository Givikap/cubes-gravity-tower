using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsController : MonoBehaviour, IUnityAdsListener {
    [SerializeField] bool _testMode = true;

#if UNITY_IOS
    string _gameId = "4407500";
    string _RewardedAd = "Rewarded_iOS";
    string _InterstitialAd = "Interstitial_iOS";
    string _BannerAd = "Banner_iOS";
#else
    string _gameId = "4407501";
    string _RewardedAd = "Rewarded_Android"; 
    string _InterstitialAd = "Interstitial_Android";
    string _BannerAd = "Banner_Android";
#endif

    private void OnEnable() {
        Advertisement.AddListener(this);
    }

    private void Start() {
        Advertisement.Initialize(_gameId, _testMode);
    }

    private void OnDisable() {
        Advertisement.RemoveListener(this);
    }

    public void PlayInterstitialAd() {
        if (Advertisement.IsReady(_InterstitialAd)) {
            Advertisement.Show(_InterstitialAd);
        }
    }

    public void PlayRewardedAd() {
        if (Advertisement.IsReady(_RewardedAd)) {
            Advertisement.Show(_RewardedAd);
        }
    }

    public void ShowBanner()
    {
        if (Advertisement.IsReady(_BannerAd)) {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(_BannerAd);
        }
    }

    IEnumerator RepeatShowBanner() {
        yield return new WaitForSeconds(1f);
        ShowBanner();
    }

    public void OnInitializationComplete() { }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsReady(string placementId){ }
    public void OnUnityAdsDidStart(string placementId) { }

    public void OnUnityAdsDidError(string message) {
        Debug.Log($"Error showing Ad Unit : {message}");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if (placementId == _RewardedAd && showResult == ShowResult.Finished) {
            PlayerPrefs.SetInt("videosCount", PlayerPrefs.GetInt("videosCount") + 1);
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Shop 2"))
                SceneManager.LoadScene("Shop 2");
        }
    }
}