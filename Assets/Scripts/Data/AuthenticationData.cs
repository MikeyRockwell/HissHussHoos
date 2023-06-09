﻿using UnityEngine;
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

        public UnityEvent OnLogPlayerOut;
        
        public UnityEvent OnPlayerNameRequired;
        public UnityEvent<string> OnPlayerNameSubmitted;

        public string PlayerName
        {
            get => PlayerPrefs.GetString("PlayerName");
            set => PlayerPrefs.SetString("PlayerName", value);
        }

        public void PlayerNameRequired()=>OnPlayerNameRequired?.Invoke();
        public void PlayerNameSubmitted(string input)=>OnPlayerNameSubmitted?.Invoke(input);
        public void LogPlayerOut()=>OnLogPlayerOut?.Invoke();
        
    }
}