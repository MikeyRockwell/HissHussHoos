using UnityEngine;
using UnityEngine.Advertisements;
using Utils;

namespace Managers.Monetization
{
    public class InterAdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private string androidAdUnitId = "Interstitial_Android";
        [SerializeField] private string iOsAdUnitId = "Interstitial_iOS";
        private string adUnitId;
        private DataWrangler.GameData gd;
        
        private void Awake()
        {   
            gd = DataWrangler.GetGameData();
            gd.roundData.OnGameBegin.AddListener(LoadAd);
            gd.eventData.OnGameOver.AddListener(ShowAd);
            // Get the Ad Unit ID for the current platform:
            adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? iOsAdUnitId
                : androidAdUnitId;
        }
        
        // Load content to the Ad Unit:
        private void LoadAd(int arg0)
        {
            Log.Message("Loading Ad: " + adUnitId, Color.red);
            Advertisement.Load(adUnitId, this); 
        }
        
        // Show the loaded content in the Ad Unit:
        private void ShowAd()
        {
            // Note that if the ad content wasn't previously loaded, this method will fail
            Advertisement.Show(adUnitId, this);
        }
        
        // Implement Load Listener and Show Listener interface methods: 
        public void OnUnityAdsAdLoaded(string _adUnitId)
        {
            // Optionally execute code if the Ad Unit successfully loads content.
        }

        public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        }
        
        public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        }
        
        public void OnUnityAdsShowStart(string _adUnitId) { }
        public void OnUnityAdsShowClick(string _adUnitId) { }
        public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) { }
    }
    
}