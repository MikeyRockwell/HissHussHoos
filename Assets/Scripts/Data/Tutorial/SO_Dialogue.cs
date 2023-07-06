using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Data.Tutorial
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "ScriptableObjects/Tutorial/Dialogue", order = 0)]
    public class SO_Dialogue : ScriptableObject
    {
        // This is a scriptable object that holds all the data for a dialogue
        // Contains sentences as a struct
        // Sentences contain the strings and tutorial events that are triggered
        
        public string dialogueName;
        public Sentence[] sentences;
        public enum Position { Left, Right }
        public Position position;
        public bool zoomCamera;
        
        [System.Serializable]
        public struct Sentence
        {   
            [TextArea(3, 10)]
            public string text;
            public TutorialEvent tutorialEvent;
            public bool hasAction;
        }
    }
}