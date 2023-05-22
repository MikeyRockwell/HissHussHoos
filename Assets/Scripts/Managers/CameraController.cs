using DG.Tweening;
using UnityEngine;
using Data.Customization;

namespace Managers {
    public class CameraController : MonoBehaviour {
        
        // Class to animate the camera
        [SerializeField] private float animDuration = 0.3f;
        [SerializeField] Vector3 zoomedInPosition;
        [SerializeField] Vector3 zoomedOutPosition;
        [SerializeField] float zoomedInOrthoScale;
        [SerializeField] float zoomedOutOrthoScale;
        [SerializeField] private SpriteRenderer background;


        private Camera cam;
        private Color bgColor;
        private DataWrangler.GameData gd;
        
        
        private void Awake() {
            cam = Camera.main;
            gd = DataWrangler.GetGameData();
            gd.customEvents.OnMenuOpened.AddListener(ZoomCameraToCharacter);
            gd.customEvents.OnMenuClosed.AddListener(ZoomToMain);
            bgColor = background.color;
        }

        private void ZoomCameraToCharacter(SO_CharacterPart unused) {
            // Tween the cameras transform to the zoomed in position
            cam.transform.DOMove(zoomedInPosition, animDuration);
            // Tween the cameras orthographic size to the zoomed in ortho scale
            cam.DOOrthoSize(zoomedInOrthoScale, animDuration);
            // Fade the background to black
            background.DOColor(Color.black, animDuration);
        }

        private void ZoomToMain() {
            // Tween the camera to the zoomed out position
            cam.transform.DOMove(zoomedOutPosition, animDuration);
            // Tween the cameras orthographic size to the zoomed out ortho scale
            cam.DOOrthoSize(zoomedOutOrthoScale, animDuration);
            // Fade the background to the original color
            background.DOColor(bgColor, animDuration);
        }
    }
}