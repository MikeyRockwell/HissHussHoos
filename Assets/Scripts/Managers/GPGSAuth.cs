﻿using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using LootLocker.Requests;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
 
 
public class GPGSAuth : MonoBehaviour
{
    public string Token;
    public string Error;
 
    void Awake()
    {
        PlayGamesPlatform.Activate();
    }
 
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await LoginGooglePlayGames();
        await SignInWithGooglePlayGamesAsync(Token);
        await LoginToLootLocker(Token);
    }

    private Task LoginToLootLocker(string token) {
        LootLockerSDKManager.StartGoogleSession(token, (response) => {
            if (!response.success) {
                Debug.Log("Error starting LootLocker GOOGLE Session");
                return;
            }

            Debug.Log("Successfully started LootLocker GOOGLE Session");
            string refreshToken = response.refresh_token;
        });
        return Task.CompletedTask;
    }

    //Fetch the Token / Auth code
    public Task LoginGooglePlayGames()
    {
        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                    tcs.SetResult(null);
                });
            }
            else
            {
                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
                tcs.SetException(new Exception("Failed"));
            }
        });
        return tcs.Task;
    }
 
 
    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); //Display the Unity Authentication PlayerID
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}