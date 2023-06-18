using FX;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using BONUS = Data.RoundData.SpeedBonusType;

namespace Managers
{
    public class PopupManager : MonoBehaviour
    {
        // Handles in game popups
        // Speed bonus etc - not animated popups
        
        [FormerlySerializedAs("popUpPool")] [SerializeField] private Transform scoreNumberPool;
        [SerializeField] private Transform scoreBonusPopPool;
        [SerializeField] private Transform fastPopPool;
        [SerializeField] private Transform superFastPopPool;
        [FormerlySerializedAs("popUpPrefab")] [SerializeField] private Popup scoreNumberPrefab;
        [SerializeField] private Popup scoreBonusPopUpPrefab;
        [SerializeField] private Popup fastPopUpPrefab;
        [SerializeField] private Popup superPopUpPrefab;

        private DataWrangler.GameData gd;

        private void Awake()
        {
            // Subscribe events
            gd = DataWrangler.GetGameData();
            gd.roundData.OnSpeedBonus.AddListener(SpeedBonusPopUp);
            gd.roundData.OnScoreAdded.AddListener(Popup);
            gd.roundData.OnBonusScoreAdded.AddListener(BonusNumberPopUp);
        }
        
        private void Popup(int score)
        {
            // Get a popup and initialize it
            Popup newPop = GetPopUpFromPool(scoreNumberPrefab, scoreNumberPool);
            newPop.gameObject.SetActive(true);
            newPop.Init(score.ToString());
        }
        
        private void BonusNumberPopUp(int score)
        {
            // Get a popup and initialize it
            Popup newPop = GetPopUpFromPool(scoreBonusPopUpPrefab, scoreBonusPopPool);
            newPop.gameObject.SetActive(true);
            newPop.Init("+" + score);
        }

        private void SpeedBonusPopUp(BONUS type)
        {
            // Get a speed popup and initialize it
            Popup newPop = GetBonusPopUpFromPool(type);
            newPop.gameObject.SetActive(true);
            newPop.Init();

            /*// Get a score bonus popup and initialize it
            Popup newScorePop = GetPopUpFromPool(scoreBonusPopUpPrefab, scoreBonusPopPool);
            newScorePop.gameObject.SetActive(true);
            // Initialize score bonus popup based on what type of bonus was received
            switch (type)
            {
                case BONUS.fast:
                    newScorePop.Init("", gd.uIData.LaserGreen);
                    break;
                case BONUS.super:
                    newScorePop.Init("", gd.uIData.HotPink);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }*/
        }

        private static Popup GetPopUpFromPool(Popup prefab, Transform pool)
        {
            // Regular popup getter
            foreach (Transform popup in pool)
            {
                if (popup.gameObject.activeSelf) continue;
                return popup.GetComponent<Popup>();
            }

            return Instantiate(prefab, pool);
        }

        private Popup GetBonusPopUpFromPool(BONUS type)
        {
            // Speed bonus popup getter
            Transform pool = GetPool(type);
            foreach (Transform popup in pool)
            {
                if (popup.gameObject.activeSelf) continue;
                return popup.GetComponent<Popup>();
            }

            return type switch
            {
                BONUS.fast => Instantiate(fastPopUpPrefab, fastPopPool),
                BONUS.super => Instantiate(superPopUpPrefab, superFastPopPool),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        private Transform GetPool(BONUS type)
        {
            Transform pool = type switch
            {
                BONUS.fast => fastPopPool,
                BONUS.super => superFastPopPool,
                _ => null
            };
            return pool;
        }
    }
}