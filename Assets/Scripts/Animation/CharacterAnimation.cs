using Utils;
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

        [SerializeField] private Material mat;

        private static readonly int Color1 = Shader.PropertyToID("_Color");
        private static readonly int ZestLights = Shader.PropertyToID("_ZestLights");

        private void Awake() {
            
            // Cache renderer and material
            spriteRenderer = GetComponent<SpriteRenderer>();
            mat = spriteRenderer.material;
            
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchNormal.AddListener(Punch);
            gd.eventData.OnPunchWarmup.AddListener(Punch);
            gd.eventData.OnPunchBonus.AddListener(Punch);
            
            // Listen for item changes on the character part
            part.OnChangeItem.AddListener(UpdateSprites);
            part.OnChangeItemColor.AddListener(UpdateSpriteColor);
            
            // Default item initialization - from here OK??
            // It's done here so that the event is subscribed prior to changing
            // part.ChangeItem(part.DefaultItem, false);
        }

        private void Start() {
            part.ChangeItem(part.CurrentItem, false);
        }

        private void UpdateSprites(SO_Item item) {
            punchSprites = item.animSprites;
            spriteRenderer.sprite = punchSprites[0];
            if (item.zestGlasses) {

                mat.SetColor(ZestLights, item.zestLightColor);
                Log.Message(mat.GetColor(Color1).ToString());
            }
            else {
                mat.SetColor(ZestLights, Color.black);
            };
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