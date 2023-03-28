using UnityEngine;

namespace Utils {
    public static class Conversion {
        
        public static int boolToInt(bool val) {
            return val ? 1 : 0;
        }

        public static bool intToBool(int val) {
            return val != 0;
        }  
    
        /// <summary>
        /// Inverse lerp method of remapping any value to a different min/max range
        /// </summary>
        /// <param name="iMin">Lower input number</param>
        /// <param name="iMax">Higher input number</param>
        /// <param name="oMin">Lower output number</param>
        /// <param name="oMax">Higher output number</param>
        /// <param name="v">Input value</param>
        /// <returns>Remapped Value</returns>
        public static float Remap(float iMin, float iMax, float oMin, float oMax, float v) {
            var t = Mathf.InverseLerp(iMin,iMax, v);
            return Mathf.Lerp (oMin, oMax, t);
        }
    }
}