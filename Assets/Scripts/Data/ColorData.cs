using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/Data/ColorData", order = 0)]
    public class ColorData : ScriptableObject
    {
        public Color[] defaultClothingColors;
    }
}