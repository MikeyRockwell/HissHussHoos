using DG.Tweening;
using UnityEngine;
using Data.Customization;
using Data.Tutorial;

namespace Managers
{
    public class CameraController : MonoBehaviour
    {
        // Class to animate the camera
        [SerializeField] private float animDuration = 0.3f;
        [SerializeField] private Vector3 zoomedInPosition;
        [SerializeField] private Vector3 zoomedOutPosition;
        [SerializeField] private float zoomedInOrthoScale;
        [SerializeField] private float zoomedOutOrthoScale;
        [SerializeField] private SpriteRenderer background;

        private Camera cam;
        private Color bgColor;
        private DataWrangler.GameData gd;


        private void Awake()
        {
            cam = Camera.main;
            gd = DataWrangler.GetGameData();
            gd.customEvents.OnMenuOpened.AddListener(ZoomCameraToCharacter);
            gd.customEvents.OnMenuClosed.AddListener(ZoomToMain);
            gd.eventData.OnDialogueStart.AddListener(TutorialZoomIn);
            gd.eventData.OnDialogueEnd.AddListener(TutorialReturnZoom);
            bgColor = background.color;
        }

        private void TutorialZoomIn(SO_Dialogue dialogue)
        {
            if (!dialogue.zoomCamera) return;
            if (dialogue.position == SO_Dialogue.Position.Left) ZoomCameraToCharacter(null);
        }

        private void TutorialReturnZoom(SO_Dialogue dialogue)
        {
            if (!dialogue.zoomCamera) return;
            ZoomToMain();
        }

        private void ZoomCameraToCharacter(SO_Category unused)
        {
            cam.DOKill();
            // Tween the cameras transform to the zoomed in position
            cam.transform.DOMove(zoomedInPosition, animDuration).SetUpdate(true);
            // Tween the cameras orthographic size to the zoomed in ortho scale
            cam.DOOrthoSize(zoomedInOrthoScale, animDuration).SetUpdate(true);
            // Fade the background to black
            // background.DOColor(Color.clear, animDuration);
        }

        private void ZoomToMain()
        {
            cam.DOKill();
            // Tween the camera to the zoomed out position
            cam.transform.DOMove(zoomedOutPosition, animDuration).SetUpdate(true);
            // Tween the cameras orthographic size to the zoomed out ortho scale
            cam.DOOrthoSize(zoomedOutOrthoScale, animDuration).SetUpdate(true);
            // Fade the background to the original color
            // background.DOColor(bgColor, animDuration);
        }
    }
}