using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentLobbyControl : MonoBehaviour
{

    #region ---- VARIABLES ----

    #region --- PRIVATE ---

    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public GameObject playerPrefab;
    public GameObject playerList;
    public GameObject auth;
    public string uname;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    void Awake()
    {
        //uname = auth.GetComponent<AuthControl>().userDB.username;
        if (GameManager.instance.infoForMultiplayer != null && GameManager.instance.infoForMultiplayer != "")
        {
            JoinRoom();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region --- CUSTOM METHODS ---
    public void JoinRoom()
    {
        string lobbyName = GameManager.instance.infoForMultiplayer;
        FirebaseDatabase.DefaultInstance.GetReference("lobby/" + lobbyName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Can,t get user in database " + task.Exception);
                return;
            }
            else if (task.IsCompleted)
            {
                LobbyDB currentLobby = JsonUtility.FromJson<LobbyDB>(task.Result.GetRawJsonValue());
                currentLobby.players.Add(uname);
                FirebaseDatabase.DefaultInstance.GetReference("lobby/" + lobbyName).SetRawJsonValueAsync(JsonUtility.ToJson(currentLobby));
                FirebaseDatabase.DefaultInstance.GetReference("lobby/" + lobbyName).ValueChanged += HandleValueChangedForMyLobby;
            }

        });
    }

    void HandleValueChangedForMyLobby(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        foreach (Transform child in playerList.transform)
        {
            //Debug.Log(FriendList.transform.childCount);
            GameObject.Destroy(child.gameObject);
        }
        LobbyDB currentLobby = JsonUtility.FromJson<LobbyDB>(args.Snapshot.GetRawJsonValue());
        foreach (string player in currentLobby.players)
        {

            playerPrefab.GetComponent<Text>().text = player;
            GameObject.Instantiate(playerPrefab, playerList.transform);
        }
    }
    #endregion
    #endregion

}
