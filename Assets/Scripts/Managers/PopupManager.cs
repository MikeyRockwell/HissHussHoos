using FX;
using System;
using UnityEngine;
using BONUS = Data.RoundData.SpeedBonusType;

namespace Managers {
    public class PopupManager : MonoBehaviour {
        
        // Handles in game popups
        // Speed bonus etc - not animated popups
        
        [SerializeField] private Transform fastPopPool;
        [SerializeField] private Transform superFastPopPool;
        [SerializeField] private Transform scoreBonusPopPool;
        [SerializeField] private Popup fastPopUpPrefab;
        [SerializeField] private Popup superPopUpPrefab;
        [SerializeField] private Popup scoreBonusPopUpPrefab;

        private DataWrangler.GameData gd;

        private void Awake() {
            // Subscribe events
            gd = DataWrangler.GetGameData();
            gd.roundData.OnSpeedBonus.AddListener(SpeedBonusPopUp);
        }

        private void SpeedBonusPopUp(BONUS type) {
            // Get a speed popup and initialize it
            Popup newPop = GetBonusPopUpFromPool(type);
            newPop.gameObject.SetActive(true);
            newPop.Init();
            
            // Get a score bonus popup and initialize it
            Popup newScorePop = GetPopUpFromPool(scoreBonusPopUpPrefab, scoreBonusPopPool);
            newScorePop.gameObject.SetActive(true);
            // Initialize score bonus popup based on what type of bonus was received
            switch (type) {
                case BONUS.fast:
                    newScorePop.Init("+" + 2, gd.uIData.LaserGreen);
                    break;
                case BONUS.super:
                    newScorePop.Init("+" + 3, gd.uIData.Gold);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
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