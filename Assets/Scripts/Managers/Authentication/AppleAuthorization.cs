using UnityEngine;
using System.Text;
// External dependencies
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;

namespace Managers
{
    public class AppleAuthorization : MonoBehaviour
    {
            IAppleAuthManager m_AppleAuthManager;
            public string Token { get; private set; }
            public string Error { get; private set; }

            public void Initialize()
            {
                PayloadDeserializer deserializer = new PayloadDeserializer();
                m_AppleAuthManager = new AppleAuthManager(deserializer);
            }

            public void Update()
            {
                if (m_AppleAuthManager != null)
                {
                    m_AppleAuthManager.Update();
                }
            }

            public void LoginToApple()
            {
                // Initialize the Apple Auth Manager
                if (m_AppleAuthManager == null)
                {
                    Initialize();
                }

                // Set the login arguments
                AppleAuthLoginArgs loginArgs = 
                    new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

                // Perform the login
                m_AppleAuthManager.LoginWithAppleId(
                    loginArgs,
                    credential =>
                    {
                        if (credential is IAppleIDCredential appleIDCredential)
                        {
                            string idToken = Encoding.UTF8.GetString(
                                appleIDCredential.IdentityToken,
                                0,
                                appleIDCredential.IdentityToken.Length);
                            Debug.Log("Sign-in with Apple successfully done. IDToken: " + idToken);
                            Token = idToken;
                        }
                        else
                        {
                            Debug.Log("Sign-in with Apple error. Message: appleIDCredential is null");
                            Error = "Retrieving Apple Id Token failed.";
                        }
                    },
                    error =>
                    {
                        Debug.Log("Sign-in with Apple error. Message: " + error);
                        Error = "Retrieving Apple Id Token failed.";
                    }
                );
            }
        }
    }