using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace FX
{
    public class Popup : MonoBehaviour
    {
        // ANIMATION
        [SerializeField] private Transform xf;
        [SerializeField] private Vector2 startScale;
        [SerializeField] private Vector2 startPos;
        [SerializeField] private float animDuration;
        [SerializeField] private float riseHeight = 0.25f;
        [SerializeField] private float scaleUpDuration = 0.15f;
        [SerializeField] private float minXDrift;
        [SerializeField] private float maxXDrift;
        [SerializeField] private Ease animEase;
        
        // TEXT
        [SerializeField] private TextMeshPro textMesh;
        [ColorUsage(true, true)]
        [SerializeField] private Color startCol;
        [ColorUsage(true, true)]
        [SerializeField] private Color endCol;

        // AUDIO
        [SerializeField] private Audio.SoundFXPlayer soundFX;


        public void Init()
        {
            ResetPopUp();
            Animate();
        }

        public void Init(string newText)
        {
            textMesh.text = newText;
            ResetPopUp();
            Animate();
        }

        public void Init(string newText, float time)
        {
            textMesh.text = newText;
            ResetPopUp();
            Animate();
        }

        public void Init(string newText, Color newColor)
        {
            textMesh.text = newText;
            textMesh.color = newColor;
            ResetPopUp();
            Animate();
        }

        private void ResetPopUp()
        {
            // Cache the transform is it is null
            if (xf == null) xf = transform;
            xf.DOKill();
            xf.localScale = startScale;
            xf.localPosition = startPos;
            textMesh.alpha = 1;
        }

        private void Animate()
        {   
            // Set a random end position on the x
            float xDrift = Random.Range(minXDrift, maxXDrift);

            if (soundFX != null) soundFX.PlayRandomAudio();
            textMesh.DOColor(endCol, animDuration).SetEase(animEase).From(startCol);
            
            Sequence seq = DOTween.Sequence();
            seq.Append(xf.DOScale(Vector2.one, scaleUpDuration).SetEase(animEase))
                .AppendInterval(animDuration - scaleUpDuration)
                .Append(xf.DOScale(0, 0.1f).OnComplete(SetInactive));

            xf.DOLocalMove(new Vector2(startPos.x + xDrift, startPos.y + riseHeight),
                animDuration).SetEase(animEase);
        }

        private void SetInactive()
        {
            gameObject.SetActive(false);
        }
    }
}