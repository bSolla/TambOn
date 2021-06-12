using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.UI;
using Firebase.Database;
using System.Threading.Tasks;

public class Control_StartMenu : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---

    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public GameObject logIn;
    public GameObject logOut;
    public User CurrentUser;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---

    // Update is called once per frame
    void Update()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            logIn.SetActive(false);
            logOut.SetActive(true);
        }
        else
        {
            logIn.SetActive(true);
            logOut.SetActive(false);        
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---
    public void closeFriends(GameObject friendButton)
    {
        friendButton.GetComponent<Button>().onClick.Invoke();
    }
    public async void Logout()
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
                FirebaseAuth.DefaultInstance.SignOut();
                GameManager.instance.info.username = "";
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
