using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using UnityEditor;
using UnityEngine.Serialization;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using TMPro;

public class AuthControl : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---

    public DependencyStatus dependencyStatus;

    public LobbyDB currentLobby;
    
    //public string AuthKey = "AIzaSyC-y6yqQy17BgGkQ-Pl4vtuYCoo3YbOT8g";


    public User CurrentUser;
   
    
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Start is called before the first frame update
    async void Start()
    {
        
        await FixDependenciesAsync();
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null && user.DisplayName != null && user.DisplayName != "")
        {
        await GetUserAsync();
        CurrentUser.status = "En Linea";
            GameManager.instance.info.username = CurrentUser.username;
            if (CurrentUser.username != "" && CurrentUser.username != null)
            {
                await FirebaseDatabase.DefaultInstance.GetReference("users/" + CurrentUser.username).SetRawJsonValueAsync(JsonUtility.ToJson(CurrentUser));
                Debug.Log("Succesful Login");
            }
        }
        else
        {
            Debug.Log("No user logged");
        }
    }

    
    #endregion

    #region --- CUSTOM METHODS FOR AUTH---
    private async Task FixDependenciesAsync()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));

            }

        });
    }


    
    public void QuitGame()
    {
        Application.Quit();
    }

    async void OnApplicationQuit()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            if (user.UserId != "" && user.UserId != null)
            {
                await GetUserAsync();
                CurrentUser.status = "Desconectado";
                if (user.DisplayName != "" && user.DisplayName != null)
                {
                    await FirebaseDatabase.DefaultInstance.GetReference("users/" + user.DisplayName).SetRawJsonValueAsync(JsonUtility.ToJson(CurrentUser));
                }
            }
        }
    }

    private async Task GetUserAsync()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        
        await FirebaseDatabase.DefaultInstance.GetReference("users/" + user.DisplayName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("Current user fail");
                return;
            }
            else if (task.IsCompleted)
            {
                CurrentUser = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());
                //Debug.Log("Got current user");
            }

        });
       
    }
    #endregion

    #endregion
}
