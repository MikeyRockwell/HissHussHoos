using System;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace FX
{
    public class MoraleBoostFX : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private Transform xf;

        private DataWrangler.GameData gd;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.playerData.md.OnMoraleBoost.AddListener(ShowMoraleBoostFX);
            gd.playerData.md.OnMoraleBoostEnd.AddListener(HideMoraleBoostFX);

            xf = GetComponent<Transform>();
            xf.localScale = Vector3.zero;
            animator.enabled = false;
        }

        private void ShowMoraleBoostFX()
        {
            xf.DOKill();
            xf.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f);
            animator.enabled = true;
        }

        private void HideMoraleBoostFX()
        {
            xf.DOScale(Vector3.zero, 0.5f);
            animator.StopPlayback();
        }
    }
}