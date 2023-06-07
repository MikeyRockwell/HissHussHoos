using System;
using Data;
using DG.Tweening;
using Managers;
using MoreMountains.Tools;
using UnityEngine;

namespace Animation {
    public class BoomBoxBounce : MonoBehaviour
    {

        [SerializeField] private MMAudioAnalyzer analyzer;
        [SerializeField] private Ease vertBounceEase;
        [SerializeField] private float bounceHeight = 0.8f;
        [SerializeField] private float bpm;
        
        private RectTransform xf;
        private DataWrangler.GameData gd;
        private MusicData md;
        
        private void Awake() {
            
            gd = DataWrangler.GetGameData();
            md = gd.musicData;
            xf = GetComponent<RectTransform>();
        }

        private void Update()
        {
            float intensity = analyzer.NormalizedBufferedAmplitude;
            if (float.IsNaN(intensity)) intensity = 0;
            float scaleY = Utils.Conversion.Remap(0, 1, 1.8f, bounceHeight, intensity);
            scaleY = Mathf.SmoothStep(xf.localScale.y, scaleY, Time.time + 1);
            xf.localScale = new Vector3(2, scaleY, 2);
        }
    }
}