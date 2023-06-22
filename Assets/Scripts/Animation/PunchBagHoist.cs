using Data;
using Data.Customization;
using Managers;
using DG.Tweening;
using UnityEngine;

namespace Animation
{
    public class PunchBagHoist : MonoBehaviour
    {
        [SerializeField] private float yDefault;
        [SerializeField] private float yHoist;
        [SerializeField] private float animDuration = 0.5f;

        private Transform xf;
        private DataWrangler.GameData gd;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnRoundBegin.AddListener(LowerBag);
            gd.eventData.OnGameOver.AddListener(RaiseBag);
            gd.customEvents.OnMenuOpened.AddListener(LowerBag1);
            gd.customEvents.OnMenuClosed.AddListener(RaiseBag);
            xf = transform;
        }

        private void LowerBag1(SO_Category arg0)
        {
            LowerBag(0);
        }

        private void LowerBag(int arg0)
        {
            xf.DOKill();
            xf.DOLocalMove(new Vector2(xf.position.x, yDefault), animDuration);
        }

        private void RaiseBag()
        {
            xf.DOKill();
            xf.DOLocalMove(new Vector2(xf.position.x, yHoist), animDuration);
        }
    }
}