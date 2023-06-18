using Managers;
using UnityEngine;
using DG.Tweening;

namespace FX
{
    public class ScreenShake : MonoBehaviour
    {
        private Camera cam;
        private DataWrangler.GameData gd;
        private Vector3 restPosition;

        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private float shakeStrength = 0.5f;
        [SerializeField] private int shakeVibrato = 15;
        [SerializeField] private float hitMult = 0.5f;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnHit.AddListener(ShakeHit);
            gd.eventData.OnMiss.AddListener(ShakeMiss);
            restPosition = transform.position;
            cam = GetComponent<Camera>();
        }

        private void ShakeHit(int arg0)
        {
            CameraShake(hitMult);
        }

        private void ShakeMiss()
        {
            CameraShake(1);
        }

        private void CameraShake(float mult)
        {
            cam.DOKill();
            cam.DOShakePosition(
                shakeDuration * mult,
                shakeStrength * mult,
                Mathf.RoundToInt(shakeVibrato * mult)).OnComplete(SetToRestPosition);
        }

        private void SetToRestPosition()
        {
            cam.transform.DOMove(restPosition, 0.1f);
        }
    }
}