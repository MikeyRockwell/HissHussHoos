using Managers;
using UnityEngine;
using DG.Tweening;
using Data.Customization;
using TARGET = Data.TargetData.Target;

namespace Animation {
    public class CharacterAnimation : MonoBehaviour {

        public SO_CharacterPart part;
        public Sprite[] punchSprites;

        private DataWrangler.GameData gd;
        private SpriteRenderer spriteRenderer;

        private Material mat;

        private static readonly int Color1 = Shader.PropertyToID("_Color");

        private void Awake() {
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            mat = spriteRenderer.material;
            gd = DataWrangler.GetGameData();
            
            gd.eventData.OnPunchNormal.AddListener(Punch);
            gd.eventData.OnPunchWarmup.AddListener(Punch);
            gd.eventData.OnPunchBonus.AddListener(Punch);
            
            part.OnChangeItem.AddListener(UpdateSprites);
            part.OnChangeItemColor.AddListener(UpdateSpriteColor);
            
            // Default item initialization - from here OK??
            // It's done here so that the event is subscribed prior to changing
            part.ChangeItem(part.DefaultItem, false);
        }

        private void UpdateSprites(SO_Item item) {
            punchSprites = item.animSprites;
            spriteRenderer.sprite = punchSprites[0];
        }

        private void UpdateSpriteColor(SO_Item item, Color newColor) {
            if (item.colorMask) {
                mat.SetColor(Color1, newColor);
            }
            else {
                mat.SetColor(Color1, Color.white);
                spriteRenderer.color = newColor;
            }
        }

        private void Punch(TARGET punch) {
            
            Sequence seq = DOTween.Sequence();
            spriteRenderer.sprite = punchSprites[(int)punch+1];
            seq.AppendInterval(gd.playerData.punchSpeed).OnComplete(() => spriteRenderer.sprite = punchSprites[0]);
        }
    }
    
}