using UnityEngine;

namespace Utils {
    public static class Log {
        
        public static void Message(string message) {
            #if UNITY_EDITOR
            Debug.Log(message);
            #endif
        }
    
        public static void Message(string message, Color color) {

            #if UNITY_EDITOR
            string hexCode = ColorUtility.ToHtmlStringRGBA(color);
            Debug.Log("<color=#" + hexCode + ">" + message + "</color>");
            #endif
        }
    
        public static void Warning(string message) {
        
            #if UNITY_EDITOR
            Debug.Log("<color=yellow>WARNING: " + message);
            #endif
        }
    
        public static void Error(string message) {
        
            #if UNITY_EDITOR
            Debug.Log("<color=red><b>ERROR: " + message);
            #endif
        }
    }

}