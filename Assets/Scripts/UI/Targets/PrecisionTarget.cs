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
        [SerializeField] private float rotationSpeedMultiplier = 100.0f;
        [SerializeField] private Vector2 rotationRange = new (10, 20);
        [SerializeField] private SoundFXPlayer sfx;
        
        private DataWrangler.GameData gd;
        private float rotationSpeed;
        private SpriteRenderer spriteRenderer;
        private bool finalTarget;
        private bool active;
        private int index;
        
        private void CacheReference() {
            if (!ReferenceEquals(gd.roundData, null)) return;
            gd = DataWrangler.GetGameData();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        public void Init(TARGET newType, bool final, int _index) {
            CacheReference();
            // Reset the target
            xf.localPosition = spawnPosition[(int)newType];
            xf.rotation = Quaternion.identity;
            type = newType;
            index = _index;
            // Set the speed randomly               
            speed = Random.Range(speedRange.x, speedRange.y);
            // Set the rotation speed randomly
            rotationSpeed = Random.Range(rotationRange.x, rotationRange.y);
            rotationSpeed *= speed;
            // Enable the target
            xf.gameObject.SetActive(true);
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
            // Rotate the target
            xf.Rotate(0, 0, (rotationSpeed * rotationSpeedMultiplier) * Time.deltaTime);
            // Check if the target is at max range
            if (!(xf.localPosition.x < killXPosition)) return;
            // If it is, kill it
            gd.eventData.MissPrecision(index);
            sfx.PlayRandomAudio();
            Kill(false);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!active) return;
            
            if (!other.gameObject.TryGetComponent(out PrecisionGoal component)) return;
            
            if (type != component.type) return;
            component.sfx.PlayRandomAudio();
            gd.eventData.HitPrecision(index);
            gd.eventData.HitPrecisionFX(type);
            Kill(true);
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
            
            // Disable the target after a short delay using the particle system's duration
            Invoke(nameof(DisableSelf), 1.1f);
        }
        
        private void DisableSelf() {
            Destroy(gameObject);
            // xf.gameObject.SetActive(false);
        }
        
    }
}