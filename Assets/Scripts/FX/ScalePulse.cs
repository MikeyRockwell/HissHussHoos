using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FX
{
    public class ScalePulse : MonoBehaviour
    {
        [SerializeField] private float minScale = 0.8f;
        [SerializeField] private float maxScale = 1.2f;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private Transform xf;

        private void OnEnable()
        {
            ScaleUp();
        }

        private void ScaleUp()
        {
            Vector3 origin = new(minScale, minScale, minScale);
            xf.DOScale(origin, 0.1f).From(Vector3.zero).OnComplete(Pulse);
        }

        private void Pulse()
        {
            Vector3 target = new(maxScale, maxScale, maxScale);
            Vector3 origin = new(minScale, minScale, minScale);
            xf.DOScale(target, duration).From(origin).SetLoops(-1, LoopType.Yoyo);
        }

        public void Disable()
        {
            xf.DOKill();
            xf.DOScale(Vector3.zero, 0.1f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}