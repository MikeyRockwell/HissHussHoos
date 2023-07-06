using UnityEngine;
using UnityEngine.Video;

namespace Managers
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private SceneManager sceneManager;

        private double delay;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            delay = videoPlayer.clip.length;
            Invoke(nameof(OnSplashScreenFinished), (float) delay);
        }

        private void OnSplashScreenFinished()
        {
            sceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
    
    
}