﻿using TMPro;
using Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace UI {
    public class MoraleMeter : MonoBehaviour {
        
        [GradientUsage(true)] 
        [SerializeField] private Gradient gradient;
        [SerializeField] private Slider moraleSlider;
        [SerializeField] private Image fill;
        [SerializeField] private TextMeshProUGUI underText;

        private DataWrangler.GameData gd;

        private void Awake() {
            // Cache components
            gd = DataWrangler.GetGameData();
            // Subscribe to events
            gd.roundData.OnGameBegin.AddListener(ShowMeter);
            gd.eventData.OnGameOver.AddListener(HideMeter);
            gd.playerData.md.OnMoraleBoost.AddListener(MoraleBoost);
            gd.playerData.md.OnMoraleBoostEnd.AddListener(EndMoraleBoost);
            gd.playerData.md.OnMoraleUpdated.AddListener(UpdateGraphics);
            gd.playerData.md.OnMoraleBoostTick.AddListener(MeterTick);
            // HideMeter();
        }

        private void HideMeter() {
            // Animate the meter closing
            transform.DOKill();
            transform.DOScaleY(0, 0.5f).OnComplete(() => gameObject.SetActive(false));
        }

        private void ShowMeter(int arg0) {
            // Animate the meter opening
            transform.DOKill();
            gameObject.SetActive(true);
            transform.DOScaleY(1, 0.5f);
            UpdateGraphics(0);
        }

        // Update the morale graphics
        private void UpdateGraphics(float morale) {
            // Update the slider using DOTween
            moraleSlider.DOValue(morale, 0.5f);
            fill.DOColor(gradient.Evaluate(morale), 0.5f);
            underText.DOColor(gradient.Evaluate(morale), 0.5f);
        }
        
        // Update the morale graphics during a morale boost
        private void MeterTick(float morale) {
            // Update the slider using DOTween
            moraleSlider.value = morale;
            fill.color = gradient.Evaluate(morale);
            underText.color = gradient.Evaluate(morale);
        }

        // Trigger the morale boost animation
        private void MoraleBoost() {
            // Animate the morale boost
            transform.DOKill();
            transform.DOPunchScale(Vector3.one * 0.12f, 0.5f, 2).SetLoops(-1, LoopType.Yoyo);
        }

        private void EndMoraleBoost() {
            // Stop the morale boost animation
            transform.DOKill();
            transform.DOScale(Vector3.one, 0.5f);
        }
    }
}