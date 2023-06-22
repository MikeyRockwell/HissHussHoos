using Utils;
using Managers;
using UnityEngine;
using DG.Tweening;
using Data.Customization;
using TARGET = Data.TargetData.Target;

namespace Animation
{
    public class CharacterSpriteManager : MonoBehaviour
    {
        public SO_Category part;

        private DataWrangler.GameData gd;
        private SpriteRenderer spriteRenderer;
        private PunchAnimation punchAnimation;
        
        public Material mat;

        [SerializeField] private int spriteOrder;
        [SerializeField] private int spriteOrderOnTop;

        private static readonly int Color1 = Shader.PropertyToID("_Color");
        private static readonly int ZestLights = Shader.PropertyToID("_ZestLights");
        private static readonly int MaskTex = Shader.PropertyToID("_MaskTex");

        private void Awake()
        {
            // Cache renderer and material
            punchAnimation = GetComponent<PunchAnimation>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            mat = spriteRenderer.material;
            
            gd = DataWrangler.GetGameData();

            // Listen for item changes on the character part
            part.OnChangeItem.AddListener(UpdateSprites);
            part.OnChangeItemColor.AddListener(UpdateSpriteColor);
        }

        private void Start()
        {
            part.ChangeItem(part.CurrentItem, false);
        }

        private void UpdateSprites(SO_Item item)
        {
            // Update the sprite arrays
            punchAnimation.punchSprites = item.animSprites;
            if (item.colorMask)
            {
                punchAnimation.maskSprites = item.maskSprites;
                mat.SetTexture(MaskTex, punchAnimation.maskSprites[0].texture);
            }
            else
            {
                mat.SetTexture(MaskTex, null);
            }

            spriteRenderer.sprite = punchAnimation.punchSprites[0];

            // Update the sprite order
            spriteRenderer.sortingOrder = item.torsoOnTop ? spriteOrderOnTop : spriteOrder;

            // Update the color of the zest lights
            mat.SetColor(ZestLights, item.zestGlasses ? item.zestLightColor : Color.black);
        }

        private void UpdateSpriteColor(SO_Item item, Color newColor)
        {   
            if (item.colorMask)
            {
                mat.SetColor(Color1, newColor);
                spriteRenderer.color = Color.white;
            }
            else
            {
                mat.SetColor(Color1, Color.white);
                spriteRenderer.color = newColor;
            }
        }
    }
}