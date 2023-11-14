using System;
using Audio;
using Data;
using Utils;
using TMPro;
using Managers;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using TARGET = Data.TargetData.Target;

namespace UI {
    public class PrecisionTarget : MonoBehaviour {
        
        // Behaviour for the Precision Target
        // Target spawns on the right side of the screen and moves to the left
        // Vertical position is based on the target type

        public TARGET type;
        
        [SerializeField] private Transform xf;
        [SerializeField] private Vector2 speedRange = new (3.0f, 5.0f);
        [SerializeField] private float speed;
        [SerializeField] private float killXPosition = 0;
        [SerializeField] private Vector2[] spawnPosition;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private float rotationSpeedMultiplier = 100.0f;
        [SerializeField] private Vector2 rotationRange = new Vector2(10, 20);
        [SerializeField] private Color farColor;
        [SerializeField] private Color nearColor;
        [SerializeField] private ParticleSystem deathParticles;
        [SerializeField] private Transform spriteTransform;
        
        private DataWrangler.GameData gd;
        private float distanceToTarget;
        private float totalDistance;
        private float rotationSpeed;
        private SpriteRenderer spriteRenderer;
        private TrailRenderer trail;
        private bool finalTarget;
        private ParticleSystem.MainModule main;
        private bool active;
        private int index;
        
        private void CacheReference() {
            if (!ReferenceEquals(gd.roundData, null)) return;
            gd = DataWrangler.GetGameData();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            // Calculate the total distance the target will travel
            totalDistance = Mathf.Abs(spawnPosition[0].x - gd.targetData.perfectXPosition);
            main = deathParticles.main;
            trail = GetComponentInChildren<TrailRenderer>();
        }
        
        public void Init(TARGET newType, bool final, int _index) {
            CacheReference();
            // Reset the target
            xf.localPosition = spawnPosition[(int)newType];
            xf.rotation = Quaternion.identity;
            type = newType;
            index = _index;
            // Set the sprite randomly from the sprite array
            // spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            // spriteRenderer.color = Color.white;
            
            speed = Random.Range(speedRange.x, speedRange.y);
            // Set the rotation speed randomly
            rotationSpeed = Random.Range(rotationRange.x, rotationRange.y);
            rotationSpeed *= speed;
            // Enable the target
            xf.gameObject.SetActive(true);
            
            // spriteTransform.localScale = Vector3.one;
            // Set the speed of the target
            finalTarget = final;
            active = true;
        }
        
        private void Update() {
            
            if (!active) return;
            
            // Move the target to the left
            Vector3 localPosition = xf.localPosition;
            
            Vector2 anchoredPosition = localPosition;
            
            anchoredPosition += Vector2.left * (speed * Time.deltaTime);
            localPosition = anchoredPosition;
            xf.localPosition = localPosition;

            distanceToTarget = Mathf.Abs(localPosition.x - gd.targetData.perfectXPosition);
            
            // Rotate the target
            xf.Rotate(0, 0, (rotationSpeed * rotationSpeedMultiplier) * Time.deltaTime);
            
            
            // Display the distance to the target up to 2 decimal places
            // debugText.text = distanceToTarget.ToString("F2", CultureInfo.InvariantCulture);
            
            // spriteRenderer.color = Color.Lerp(
            //     nearColor, 
            //     farColor, 
            //     (distanceToTarget / totalDistance));

            if (!(xf.localPosition.x < killXPosition)) return;
            
            gd.eventData.MissPrecision(index);
            Kill(false);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!active) return;
            
            if (!other.gameObject.TryGetComponent(out PrecisionGoal component)) return;
            
            if (type != component.type) return;
            component.sfx.PlayRandomAudio();
            gd.eventData.HitPrecision(index);
            Kill(true);
        }

        public void CheckHit(TARGET punchType) {
            if (!active) return;
            // Check if the punch type is correct
            if (punchType != type) return;
            
            // Check if the target is within the miss threshold
            // if (distanceToTarget > gd.targetData.missThreshold) return;
            
            // Check if the target is within the OK threshold
            // if (distanceToTarget < gd.targetData.okDistanceThresh) {
            //     // If so, check if the target is within the perfect threshold
            //     if (distanceToTarget < gd.targetData.perfectDistanceThresh) {
            //         // If so, give a perfect hit
            //         gd.eventData.HitPrecision(TargetData.PrecisionHitType.perfect, punchType, index);
            //         Log.Message("Perfect Hit!", Color.yellow);
            //     }
            //     // Otherwise give an OK hit
            //     else {
            //         gd.eventData.HitPrecision(TargetData.PrecisionHitType.ok, punchType, index);
            //         Log.Message("OK Hit!", Color.green);
            //     }
            //     Kill(true);
            // }
        }

        private void Kill(bool hit) {
            active = false;
            if (finalTarget) {
                gd.roundData.EndPrecisionRound();
            }
            
            // Make the ball bounce away using DOTween
            Vector3 localPosition = xf.localPosition;
            
            Vector3 jumpTarget = hit ?
                new Vector3(localPosition.x + 5, localPosition.y -2.5f, 0) :
                new Vector3(localPosition.x + 1, localPosition.y -2.5f, 0);
            
            float jumpPower = hit ? 2f : 1.5f;
            
            // Hit Bounce
            xf.DOJump(
                jumpTarget,
                jumpPower,
                1,
                1f).SetUpdate(true);

            // Fade the sprite out
            spriteRenderer.DOFade(0, 1f).SetUpdate(true).SetEase(Ease.OutBounce);
            
            // Scale the target down
            // spriteTransform.DOScale(Vector3.zero, 0.1f).SetUpdate(true);
            // Disable the target after a short delay using the particle system's duration
            Invoke(nameof(DisableSelf), 1.1f);
            // gameObject.SetActive(false);
        }
        
        private void DisableSelf() {
            Destroy(gameObject);
            // xf.gameObject.SetActive(false);
        }
        
    }
}