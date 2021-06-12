using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Control_Register : MonoBehaviour
{
    public AuthError error;
    public bool existsError;
    public GameObject registerMenu;
    public GameObject startMenu;
    private bool userExists;

    void Start()
    {
        existsError = false;
        userExists = false;
    }
    private void HandleError(AuthError errorCode)
    {
        string message = errorCode.ToString();
        GameObject g = GameObject.FindGameObjectWithTag("Error");
        g.GetComponent<TextMeshProUGUI>().text = message;
    }

    public async void Register(GameObject go)
    {
        existsError = false;
        userExists = false;
        InputField[] t = go.GetComponentsInChildren<InputField>();
        await CheckUser(t[2].text);
        if (userExists)
        {
            existsError = true;
            error = AuthError.UserMismatch;
            
        }
        else
        {
            await RegisterAuthAsync(t[0].text, t[1].text, t[2].text);
        }
        if (existsError)
        {
            HandleError(error);
        }
        else
        {
            registerMenu.SetActive(false);
            startMenu.SetActive(true);
        }
    }


    public async Task RegisterAuthAsync(string email, string password, string username)
    {
        string e = email;
        string p = password;
        string u = username;
        await FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(async task =>
        {

            if (task.Exception != null)
            {
                Debug.Log("Error");
                FirebaseException exception = task.Exception.GetBaseException() as FirebaseException;
                error = (AuthError)exception.ErrorCode;
                existsError = true;

                return;
            }
            
            existsError = false;
            userExists = false;
            FirebaseUser user;
            user = task.Result;
            await user.UpdateUserProfileAsync(new UserProfile { DisplayName = username });

            User newUser = new User(username, email, 1, new List<string>(), "En Linea", new List<string>());
            GameManager.instance.info.username = newUser.username;
            await FirebaseDatabase.DefaultInstance.GetReference("users/" + user.DisplayName).SetRawJsonValueAsync(JsonUtility.ToJson(newUser));
            Debug.Log("Registered");

        });
       
    }

    private async Task CheckUser(string username)
    {
        await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("User already exists");

            }
            else if (task.IsCompleted)
            {
                
                foreach (var childs in task.Result.Children)
                {
                    try
                    {
                        User u = JsonUtility.FromJson<User>(childs.GetRawJsonValue());
                        if (u.username == username)
                        {
                            userExists = true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        });
        if (username.Contains("/") || username ==  "")
        {
            userExists = true ;
        }


    }
}
