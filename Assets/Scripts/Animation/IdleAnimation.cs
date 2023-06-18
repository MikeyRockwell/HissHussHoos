using UnityEngine;
using DG.Tweening;

namespace Animation
{
    public class IdleAnimation : MonoBehaviour
    {
        [SerializeField] private Vector2 defaultPos;
        [SerializeField] private Ease ease;
        [SerializeField] private float bobAmount = 0.1f;
        [SerializeField] private float bobDuration = 0.5f;

        private Transform xf;

        private void Start()
        {
            xf = transform;
            defaultPos = xf.position;

            StartIdleLoop();
        }

        private void StartIdleLoop()
        {
            xf.DOMoveY(
                defaultPos.y - bobAmount,
                bobDuration).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
        }
    }
}