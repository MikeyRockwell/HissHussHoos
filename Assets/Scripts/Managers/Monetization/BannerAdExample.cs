using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace Managers.Monetization {
    public class BannerAdExample : MonoBehaviour {
        // For the purpose of this example, these buttons are for functionality testing:
        [SerializeField] private Button _loadBannerButton;
        [SerializeField] private Button _showBannerButton;
        [SerializeField] private Button _hideBannerButton;

        [SerializeField] private BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

        [SerializeField] private string _androidAdUnitId = "Banner_Android";
        [SerializeField] private string _iOSAdUnitId = "Banner_iOS";
        private string _adUnitId = null; // This will remain null for unsupported platforms.

        private void Start() {
            // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif

            // Disable the button until an ad is ready to show:
            _showBannerButton.interactable = false;
            _hideBannerButton.interactable = false;

            // Set the banner position:
            Advertisement.Banner.SetPosition(_bannerPosition);

            // Configure the Load Banner button to call the LoadBanner() method when clicked:
            _loadBannerButton.onClick.AddListener(LoadBanner);
            _loadBannerButton.interactable = true;
        }

        // Implement a method to call when the Load Banner button is clicked:
        public void LoadBanner() {
            // Set up options to notify the SDK of load events:
            BannerLoadOptions options = new() {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            // Load the Ad Unit with banner content:
            Advertisement.Banner.Load(_adUnitId, options);
        }

        // Implement code to execute when the loadCallback event triggers:
        private void OnBannerLoaded() {
            Debug.Log("Banner loaded");

            // Configure the Show Banner button to call the ShowBannerAd() method when clicked:
            _showBannerButton.onClick.AddListener(ShowBannerAd);
            // Configure the Hide Banner button to call the HideBannerAd() method when clicked:
            _hideBannerButton.onClick.AddListener(HideBannerAd);

            // Enable both buttons:
            _showBannerButton.interactable = true;
            _hideBannerButton.interactable = true;
        }

        // Implement code to execute when the load errorCallback event triggers:
        private void OnBannerError(string message) {
            Debug.Log($"Banner Error: {message}");
            // Optionally execute additional code, such as attempting to load another ad.
        }

        // Implement a method to call when the Show Banner button is clicked:
        private void ShowBannerAd() {
            // Set up options to notify the SDK of show events:
            BannerOptions options = new() {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            // Show the loaded Banner Ad Unit:
            Advertisement.Banner.Show(_adUnitId, options);
        }

        // Implement a method to call when the Hide Banner button is clicked:
        private void HideBannerAd() {
            // Hide the banner:
            Advertisement.Banner.Hide();
        }

        private void OnBannerClicked() {
        }

        private void OnBannerShown() {
        }

        private void OnBannerHidden() {
        }

        private void OnDestroy() {
            // Clean up the listeners:
            _loadBannerButton.onClick.RemoveAllListeners();
            _showBannerButton.onClick.RemoveAllListeners();
            _hideBannerButton.onClick.RemoveAllListeners();
        }
    }
}