using UI;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class TimingRoundManager : MonoBehaviour {

        [SerializeField] private float endRoundThreshold = 2;
        [SerializeField] private Transform targetPool;
        [SerializeField] private TimingTarget targetPrefab;
        
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            // Sub events
            gd.roundData.OnTimeAttackRoundBegin.AddListener(BeginBonusRound);
            gd.eventData.OnPunchTimeAttack.AddListener(target=> gd.targetData.CheckBonusPunch(target));
        }

        private void BeginBonusRound() {
            gd.targetData.bonusTargets.Clear();
            Sequence seq = DOTween.Sequence();
            // Start the spawner after popup
            seq.AppendInterval(gd.roundData.roundTextTime).OnComplete(SpawnNewTarget);
            
            StartCoroutine(gd.roundData.TimeAttackRoundTimer());
        }

        private void SpawnNewTarget() {
            
            if ((gd.roundData.timeAttackRoundClock < endRoundThreshold)) return;
            
            TimingTarget target = GetTargetFromPool();
            
            TARGET newTarget = (TARGET)Random.Range(0, 3);
            
            target.Init(newTarget);
           
            StartCoroutine(RandomTimer(1, 2));
        }

        private TimingTarget GetTargetFromPool() {
            foreach (Transform target in targetPool) {
                if (target.gameObject.activeSelf) continue;
                return target.GetComponent<TimingTarget>();
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