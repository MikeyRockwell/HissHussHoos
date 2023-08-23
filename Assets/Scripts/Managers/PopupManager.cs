using FX;
using System;
using UnityEngine;
using BONUS = Data.RoundData.SpeedBonusType;

namespace Managers {
    public class PopupManager : MonoBehaviour {
        // Handles in game popups
        // Speed bonus etc - not animated popups

        [SerializeField] private Transform scoreNumberPool;

        [SerializeField] private Transform scoreBonusPopPool;
        [SerializeField] private Transform fastPopPool;
        [SerializeField] private Transform superFastPopPool;
        [SerializeField] private Transform stringPopPool;

        [SerializeField] private Popup scoreNumberPrefab;

        [SerializeField] private Popup scoreBonusPopUpPrefab;
        [SerializeField] private Popup fastPopUpPrefab;
        [SerializeField] private Popup superPopUpPrefab;
        [SerializeField] private Popup stringPopUpPrefab;

        private DataWrangler.GameData gd;

        private void Awake() {
            // Subscribe events
            gd = DataWrangler.GetGameData();
            gd.roundData.OnSpeedBonus.AddListener(SpeedBonusPopUp);
            gd.roundData.OnScoreAdded.AddListener(Popup);
            gd.roundData.OnBonusScoreAdded.AddListener(BonusNumberPopUp);
            gd.roundData.OnTimeAttackPerfectRound.AddListener(StringPopUp);
        }

        private void Popup(int score) {
            // Get a popup and initialize it
            Popup newPop = GetPopUpFromPool(scoreNumberPrefab, scoreNumberPool);
            newPop.gameObject.SetActive(true);
            newPop.Init(score.ToString());
        }

        private void BonusNumberPopUp(int score) {
            // Get a popup and initialize it
            Popup newPop = GetPopUpFromPool(scoreBonusPopUpPrefab, scoreBonusPopPool);
            newPop.gameObject.SetActive(true);
            newPop.Init("+" + score);
        }

        private void SpeedBonusPopUp(BONUS type) {
            // Get a speed popup and initialize it
            Popup newPop = GetBonusPopUpFromPool(type);
            newPop.gameObject.SetActive(true);
            newPop.Init();
        }
        
        private void StringPopUp(string text) {
            // Get a string popup and initialize it
            Popup newPop = GetPopUpFromPool(stringPopUpPrefab, stringPopPool);
            newPop.gameObject.SetActive(true);
            newPop.Init(text);
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