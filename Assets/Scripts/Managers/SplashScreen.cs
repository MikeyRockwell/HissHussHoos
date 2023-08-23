using UnityEngine;
using UnityEngine.Video;

namespace Managers {
    public class SplashScreen : MonoBehaviour {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private SceneManager sceneManager;

        private double delay;

        private void Awake() {
            Application.targetFrameRate = 60;
        }

        private void Start() {
            // Splash video delay
            // delay = videoPlayer.clip.length;
            // Invoke(nameof(OnSplashScreenFinished), (float)delay);
            
            // TODO remove this once the splash screen is done
            OnSplashScreenFinished();
        }

        private void OnSplashScreenFinished() {
            sceneManager.LoadScene(1);
        }
    }
}