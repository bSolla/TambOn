using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Control_Login : MonoBehaviour
{
    public AuthError error;
    public bool existsError;
    public GameObject loginMenu;
    public GameObject startMenu;
    public GameObject email;
    public GameObject password;

    void Start()
    {
        existsError = false;
    }
    private void HandleError(AuthError errorCode)
    {
        string message = errorCode.ToString();
        GameObject g = GameObject.FindGameObjectWithTag("Error");
        g.GetComponent<TextMeshProUGUI>().text = message;
    }
    public async void Login()
    {
        existsError = false;
        await LoginFirebaseAsync(email.GetComponent<InputField>().text, password.GetComponent<InputField>().text);
        if (existsError)
        {
            HandleError(error);
        }
        else
        {
            loginMenu.SetActive(false);
            startMenu.SetActive(true);
        }
    }
    public void ResetPassword(GameObject g)
    {

        InputField[] t = g.GetComponentsInChildren<InputField>();
        string email = t[0].text;
        FirebaseAuth.DefaultInstance.SendPasswordResetEmailAsync(email).ContinueWith(task => {
            if (task.Exception != null)
            {
                FirebaseException exception = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)exception.ErrorCode;
                HandleError(errorCode);
                return;
            }

        });
    }

    public async Task LoginFirebaseAsync(string email, string password)
    {
        FirebaseUser user = null;
        string e = email;
        string p = password;
        await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                FirebaseException exception = task.Exception.GetBaseException() as FirebaseException;
                error = (AuthError)exception.ErrorCode;
                Debug.Log("Error en auth");
                existsError = true;
            }
            else
            {
                user = task.Result;
                Debug.Log("Auth success");
            }
            
        });
        if (!existsError)
        {
            await FirebaseDatabase.DefaultInstance.GetReference("users/" + user.DisplayName).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Can,t get user in database " + task.Exception);
                    return;
                }
                else if (task.IsCompleted)
                {

                    DataSnapshot snapshot = task.Result;
                    User u = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                    u.status = "En Linea";
                    GameManager.instance.info.username = u.username;
                    FirebaseDatabase.DefaultInstance.GetReference("users/" + user.DisplayName).SetRawJsonValueAsync(JsonUtility.ToJson(u));
                }
            });
        }
    }
}
