using UnityEngine;

namespace FX {
    public class ScreenImageEffect : MonoBehaviour {

        [SerializeField] private Shader shader;
        private Material material;

        private void Awake() {
            material = new Material(shader);
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            Graphics.Blit(src, dest, material);
        }
    }
}