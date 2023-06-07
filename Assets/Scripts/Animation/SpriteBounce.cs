using Data;
using Managers;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine.Serialization;

namespace Animation
{
    public class SpriteBounce : MonoBehaviour
    {
        
        [SerializeField] private SmoothAudio smoothAudio;
        [SerializeField] private bool positionBounce;
        [SerializeField] private float minBounceHeight;
        [SerializeField] private float maxBounceHeight = 0.8f;
        [SerializeField] private float timeMultiplier = 0.8f;
        [SerializeField] private float bounceSubtraction = 0.08f;

        private Transform xf;
        private float bounce;
        
        private Vector3 currentScale;
        private float currentY;
        
        private void Awake() {
            
            xf = transform;
            if (!positionBounce) minBounceHeight = xf.localScale.y;
            else minBounceHeight = xf.position.y;
        }

        private void Update()
        {   
            currentScale = xf.localScale;
            currentY = xf.position.y;

            bounce = Utils.Conversion.Remap
                (0, 1, minBounceHeight, maxBounceHeight, smoothAudio.intensity);

            if (positionBounce) BouncePosition();
            else BounceScale();
        }

        private void BounceScale()
        {
            bounce = Mathf.Lerp(currentScale.y, bounce, Time.deltaTime * timeMultiplier);
            currentScale = new Vector3(currentScale.x, bounce, currentScale.z);
            
            xf.localScale = currentScale;
        }

        private void BouncePosition()
        {
            Vector3 position = xf.position;
            if (!smoothAudio.idling) bounce -= bounceSubtraction;
                
            currentY = Mathf.Lerp(currentY, bounce, Time.deltaTime * timeMultiplier);
            position = new Vector3(position.x, currentY, position.z);
          
            xf.position = position;
        }
    }
}