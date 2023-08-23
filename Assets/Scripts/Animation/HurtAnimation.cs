using Managers;
using DG.Tweening;
using UnityEngine;
using Data.Customization;

namespace Animation {
    public class HurtAnimation : MonoBehaviour {
        
        // This script turns the sprite red when taking damage or missing a target
        // Applied to a part of the character
        // If the part is a character, it will turn the whole character red
        // If the part is a color mask, it will turn the color mask red
        
        [SerializeField] private SO_Category category;
        [SerializeField] private CharacterSpriteManager charAnim;
        [SerializeField] private bool isCharacter;

        private SpriteRenderer spriteRenderer;
        private DataWrangler.GameData gd;
        private Color currentColor;
        private Color currentShaderColor;
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        private void Awake() {
            // EVENTS
            gd = DataWrangler.GetGameData();
            gd.eventData.OnMiss.AddListener(Hurt);
            // COMPONENTS
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Hurt() {   // OnMiss
            // If the current item is a character, turn the whole character red
            if (isCharacter) {
                currentColor = Color.white;
                SpriteAnim();
                return;
            }
            // If the current item is a color mask, turn the color mask red
            if (category.CurrentItem.colorMask) {
                ColorMaskAnim();
                SpriteAnimToWhite();
                return;
            }
            // If the current item is a color, turn the sprite red
            SpriteAnim();
        }

        private void SpriteAnimToWhite() {
            // Used by color masks to force the sprite back to white
            // Where the color mask shader will turn to red
            currentColor = Color.white;
            DOTween.Kill(spriteRenderer);
            spriteRenderer.color = Color.red;
            Sequence seq = DOTween.Sequence(spriteRenderer);
            seq.PrependInterval(0.25f).Append(spriteRenderer.DOColor(currentColor, 0.5f));
        }

        private void SpriteAnim() {
            // Stores the current color
            // turns the sprite red
            // then turns it back to the current color
            currentColor = category.CurrentItem.color;
            DOTween.Kill(spriteRenderer);
            spriteRenderer.color = Color.red;
            Sequence seq = DOTween.Sequence(spriteRenderer);
            seq.PrependInterval(0.25f).Append(spriteRenderer.DOColor(currentColor, 0.5f));
        }

        private void ColorMaskAnim() {
            // Stores the current shader color
            // turns the shader red
            // then turns it back to the current shader color
            currentShaderColor = category.CurrentItem.color;
            DOTween.Kill(charAnim.mat);
            charAnim.mat.SetColor(Color1, Color.red);
            Sequence seq = DOTween.Sequence(charAnim.mat);
            seq.PrependInterval(0.25f).Append(charAnim.mat.DOColor(currentShaderColor, 0.5f));
        }
    }
}