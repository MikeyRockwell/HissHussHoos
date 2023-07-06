using Managers;
using DG.Tweening;
using UnityEngine;
using Data.Customization;
using UnityEngine.Serialization;

namespace Animation
{
    public class HurtAnimation : MonoBehaviour
    {
        [FormerlySerializedAs("part")] [SerializeField] private SO_Category category;
        [SerializeField] private CharacterSpriteManager charAnim;
        [SerializeField] private bool isCharacter;
        
        private SpriteRenderer spriteRenderer;
        private DataWrangler.GameData gd;
        private Color currentColor;
        private Color currentShaderColor;
        private static readonly int Color1 = Shader.PropertyToID("_Color");
        
        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnMiss.AddListener(Hurt);
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Hurt()
        {

            if (isCharacter)
            {
                currentColor = Color.white;
                SpriteAnim();
                return;
            }
            
            if (category.CurrentItem.colorMask) ColorMaskAnim();

            SpriteAnim();

        }

        private void SpriteAnim()
        {
            currentColor =  category.CurrentItem.color;
            
            DOTween.Kill(spriteRenderer);
            spriteRenderer.color = Color.red;
            Sequence seq = DOTween.Sequence(spriteRenderer);
            seq.PrependInterval(0.25f).Append(spriteRenderer.DOColor(currentColor, 0.5f));
        }

        private void ColorMaskAnim()
        {
            currentShaderColor = category.CurrentItem.color;
            
            DOTween.Kill(charAnim.mat);
            charAnim.mat.SetColor(Color1, Color.red);
            Sequence seq = DOTween.Sequence(charAnim.mat);
            seq.PrependInterval(0.25f).Append(charAnim.mat.DOColor(currentShaderColor, 0.5f));
        }
    }
}