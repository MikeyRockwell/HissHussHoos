using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [CreateAssetMenu(fileName = "AuthenticationData", menuName = "ScriptableObjects/Data/AuthenticationData",
        order = 0)]
    public class AuthenticationData : ScriptableObject
    {
        // This class is used to manage authentication data
        // Such as the player name which will be save and loaded
        // Also events related to authentication

        public UnityEvent OnPlayerNameRequired;

        public string PlayerName
        {
            get => PlayerPrefs.GetString("PlayerName");
            set => PlayerPrefs.SetString("PlayerName", value);
        }

        public void PlayerNameRequired()
        {
            OnPlayerNameRequired?.Invoke();
        }
    }
}