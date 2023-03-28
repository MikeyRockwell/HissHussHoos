using UI;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class BonusRoundManager : MonoBehaviour {

        [SerializeField] private float endRoundThreshold = 2;
        [SerializeField] private Transform targetPool;
        [SerializeField] private BonusTarget targetPrefab;
        
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            // Sub events
            gd.roundData.OnBonusRoundBegin.AddListener(BeginBonusRound);
            gd.eventData.OnPunchBonus.AddListener(target=> gd.targetData.CheckBonusPunch(target));
        }

        private void BeginBonusRound() {
            gd.targetData.bonusTargets.Clear();
            Sequence seq = DOTween.Sequence();
            // Start the spawner after popup
            seq.AppendInterval(gd.roundData.roundTextTime).OnComplete(SpawnNewTarget);
            
            StartCoroutine(gd.roundData.BonusRoundTimer());
        }

        private void SpawnNewTarget() {
            
            if ((gd.roundData.bonusTime < endRoundThreshold)) return;
            
            BonusTarget target = GetTargetFromPool();
            
            TARGET newTarget = (TARGET)Random.Range(0, 3);
            
            target.Init(newTarget);
           
            StartCoroutine(RandomTimer(1, 2));
        }

        private BonusTarget GetTargetFromPool() {
            foreach (Transform target in targetPool) {
                if (target.gameObject.activeSelf) continue;
                return target.GetComponent<BonusTarget>();
            }

            return Instantiate(targetPrefab, targetPool);
        }

        private IEnumerator RandomTimer(float min, float max) {

            float time = Random.Range(min, max);
            while (time > 0) {
                time -= Time.deltaTime;
                yield return null;
            }
            SpawnNewTarget();
        }
    }
}