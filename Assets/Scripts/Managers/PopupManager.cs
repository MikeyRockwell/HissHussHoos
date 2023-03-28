using FX;
using System;
using UnityEngine;
using BONUS = Data.RoundData.SpeedBonusType;

namespace Managers {
    public class PopupManager : MonoBehaviour {

        [SerializeField] private Transform fastPopPool;
        [SerializeField] private Transform superFastPopPool;
        [SerializeField] private Transform roundEndPopPool;
        [SerializeField] private Popup fastPopUpPrefab;
        [SerializeField] private Popup superPopUpPrefab;
        [SerializeField] private Popup roundEndPopUpPrefab;

        private DataWrangler.GameData gd;

        private void Awake() {
            // Subscribe events
            gd = DataWrangler.GetGameData();
            gd.roundData.OnSpeedBonus.AddListener(SpeedBonusPopUp);
            gd.roundData.OnGameBegin.AddListener(RoundBeginPopUp);
            gd.roundData.OnRoundComplete.AddListener(RoundBeginPopUp);
            gd.roundData.OnBonusRoundComplete.AddListener(RoundBeginPopUp);
        }

        private void RoundBeginPopUp(int round) {
            // Get a popup from the pool
            // could be just one reusable popup
            Popup newPop = GetPopUpFromPool(roundEndPopUpPrefab, roundEndPopPool);
            // Initialize
            newPop.gameObject.SetActive(true);
            newPop.Init("ROUND " + round);
        }
        
        private void SpeedBonusPopUp(BONUS type) {
            // Get a speed popup and initialize it
            Popup newPop = GetBonusPopUpFromPool(type);
            newPop.gameObject.SetActive(true);
            newPop.Init();
        }

        private static Popup GetPopUpFromPool(Popup prefab, Transform pool) {
            // Regular popup getter
            foreach (Transform popup in pool) {
                if (popup.gameObject.activeSelf) continue;
                return popup.GetComponent<Popup>();
            }

            return Instantiate(prefab, pool);
        }

        private Popup GetBonusPopUpFromPool(BONUS type) {
            // Speed bonus popup getter
            Transform pool = GetPool(type);
            foreach (Transform popup in pool) {
                if (popup.gameObject.activeSelf) continue;
                return popup.GetComponent<Popup>();
            }

            return type switch {
                BONUS.fast => Instantiate(fastPopUpPrefab, fastPopPool),
                BONUS.super => Instantiate(superPopUpPrefab, superFastPopPool),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        private Transform GetPool(BONUS type) {
            
            Transform pool = type switch {
                BONUS.fast => fastPopPool,
                BONUS.super => superFastPopPool,
                _ => null
            };
            return pool;
        }
    }
}