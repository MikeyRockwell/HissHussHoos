using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.Serialization;

namespace Managers.Monetization
{
    public class BannerAdManager : MonoBehaviour
    {
        [SerializeField] private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;
        [SerializeField] private string androidAdUnitId = "Banner_Android";
        [SerializeField] private string iOSAdUnitId = "Banner_iOS";
        [SerializeField] private float adLength = 5f;
        private readonly string adUnitId = null; // This will remain null for unsupported platforms.

        private DataWrangler.GameData gd;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnGameBegin.AddListener(InitBannerAd);
        }

        private void Start()
        {
            // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif
            // Set the banner position:
            Advertisement.Banner.SetPosition(bannerPosition);
        }

        private void InitBannerAd(int arg0)
        {
            StopAllCoroutines();
            StartCoroutine(nameof(PlayBannerAd));
        }

        private IEnumerator PlayBannerAd()
        {
            LoadBanner();
            while (!Advertisement.Banner.isLoaded)
            {
                yield return null;
            }
            ShowBannerAd();
            yield return new WaitForSecondsRealtime(adLength);
            HideBannerAd();
        }

        // Implement a method to call when the Load Banner button is clicked:
        private void LoadBanner()
        {
            // Set up options to notify the SDK of load events:
            BannerLoadOptions options = new BannerLoadOptions { };
     
            // Load the Ad Unit with banner content:
            Advertisement.Banner.Load(adUnitId);
        }
        
        // Implement a method to call when the Show Banner button is clicked:
        private void ShowBannerAd()
        {
            // Set up options to notify the SDK of show events:
            BannerOptions options = new BannerOptions { };
     
            // Show the loaded Banner Ad Unit:
            Advertisement.Banner.Show(adUnitId);
        }
        
        // Implement a method to call when the Hide Banner button is clicked:
        private void HideBannerAd()
        {
            // Hide the banner:
            Advertisement.Banner.Hide();
        }
    }
}