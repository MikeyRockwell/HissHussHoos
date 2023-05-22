using TMPro;
using Managers;
using DG.Tweening;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace UI {
    public class TimingTarget : MonoBehaviour {

        public TARGET type;
        
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private Vector2[] spawnPositions;
        [SerializeField] private Vector2[] targetPos;
        
        private Transform xf;
        private DataWrangler.GameData gd;

        private void Awake() {
            CacheReference();
        }
        
        private void CacheReference() {
            if (!ReferenceEquals(gd.roundData, null)) return;
            gd = DataWrangler.GetGameData();
        }


        public void Init(TARGET newType) {
            
            gameObject.SetActive(true);
            type = newType;
            textMesh.text = type.ToString();
            
            xf ??= transform;
            xf.position = spawnPositions[(int)type];
            
            gd.targetData.bonusTargets.Add(this);
            MoveTo();
        }

        private void MoveTo() {
            xf.DOMove(targetPos[(int)type], CalculateSpeed()).SetEase(Ease.Linear).
                OnComplete(DisableSelf);
        }

        private float CalculateSpeed() {
            float distance = Vector2.Distance(spawnPositions[(int)type], targetPos[(int)type]);
            return distance * gd.targetData.bonusTargetSpeedMultiplier;
        }

        public void DisableSelf() {
            xf.DOKill();
            gd.targetData.bonusTargets.Remove(this);
            gameObject.SetActive(false);
        }
    }
}