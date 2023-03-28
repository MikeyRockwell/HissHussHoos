using System;
using Data;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Animation {
    public class BoomBoxBounce : MonoBehaviour {

        [SerializeField] private Ease vertBounceEase;
        [SerializeField] private float bounceHeight = 0.8f;
        [SerializeField] private float bpm;
        
        private RectTransform xf;
        private DataWrangler.GameData gd;
        private MusicData md;
        
        private void Awake() {
            
            gd = DataWrangler.GetGameData();
            md = gd.musicData;
            
            md.OnPressPlay.AddListener(OnPressPlay);
            md.OnFastForward.AddListener(UpdateBounce);
            md.OnRewind.AddListener(UpdateBounce);
            
            xf = GetComponent<RectTransform>();
        }

        private void Start() {
            StartBounce();
        }

        private void OnPressPlay(bool playing) {
            if (playing) { StartBounce(); }
            else { StopBounce(); }
        }

        private void UpdateBounce() {
            xf.DOKill();
            xf.localScale = new Vector3(2,2,2);
            StartBounce();
        }

        private void StartBounce() {
            bpm = (float)(md.musicTracks[md.currentTrackIndex].bpm * 2) / 1000;
            xf.DOKill();
            xf.DOScaleY(bounceHeight, bpm).SetLoops(-1, LoopType.Yoyo).SetEase(vertBounceEase);
        }

        private void StopBounce() {
            xf.DOKill();
            xf.DOScaleY(2, 0.5f);
        }
    }
}