using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Control_Friend : MonoBehaviour
{
    public GameObject FriendList;
    public GameObject friendListPrefab;

    public GameObject FriendRequestAll;
    public GameObject FriendRequest;

    public User CurrentUser;
    public string friendStatus;
    private Image statusSprite;
    // Start is called before the first frame update
    void Start()
    {
        statusSprite = friendListPrefab.GetComponentInChildren<Image>();
    }
    public async void refresh()
    {
        foreach (Transform child in FriendList.transform)
        {
            //Debug.Log(FriendList.transform.childCount);
            GameObject.Destroy(child.gameObject);
        }
        await GetUserAsync();
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        FirebaseDatabase.DefaultInstance.GetReference("users/" + user.DisplayName).ValueChanged += HandleValueChanged;
        foreach (string friend in CurrentUser.friends)
        {
            await FindFriendStatus(friend);

            Text[] t = friendListPrefab.GetComponentsInChildren<Text>();
            t[0].text = friend;
            //t[1].text = friendStatus;
            switch (friendStatus)
            {
                case "En Linea":
                    statusSprite.color = Color.green;
                    break;
                case "Jugando":
                    statusSprite.color = Color.red;
                    break;
                case "Desconectado":
                    statusSprite.color = Color.gray;
                    break;
                default:
                    statusSprite.color = Color.gray;
                    break;
            }

            Instantiate(friendListPrefab, FriendList.transform);
        }
    }
    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        User u = JsonUtility.FromJson<User>(args.Snapshot.GetRawJsonValue());
        if (u.friendsRequest.Count > 0)
        {
            FriendRequestAll.SetActive(true);
            FriendRequest.GetComponent<Text>().text = u.friendsRequest[0];
        }
        else
        {
            FriendRequestAll.SetActive(false);
        }
        

        // Do something with the data in args.Snapshot
    }
    private async Task FindFriendStatus(string friend)
    {
        await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Cant find friend status for " + friend);

            }
            else if (task.IsCompleted)
            {
                friendStatus = "";
                foreach (var childs in task.Result.Children)
                {
                    
                    User u = JsonUtility.FromJson<User>(childs.GetRawJsonValue());
                    if (u.username == friend)
                    {      
                        friendStatus = u.status;                      
                    }
                    FirebaseDatabase.DefaultInstance.GetReference("users/" + childs.Key).ValueChanged += HandleValueChangedForFriend;
                }
            }
        });
    }

    private void HandleValueChangedForFriend(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        User u = JsonUtility.FromJson<User>(args.Snapshot.GetRawJsonValue());
        foreach (Transform child in FriendList.transform)
        {
            Text[] t = child.GetComponentsInChildren<Text>();
           
            if (t[0].text == u.username)
            {
                switch (u.status)
                {
                    case "En Linea":
                        child.GetComponentInChildren<Image>().color = Color.green;
                        break;
                    case "Jugando":
                        child.GetComponentInChildren<Image>().color = Color.red;
                        break;
                    case "Desconectado":
                        child.GetComponentInChildren<Image>().color = Color.gray;
                        break;

                    default:
                        child.GetComponentInChildren<Image>().color = Color.gray;
                        break;
                }
                //t[1].text = u.status;
            }
        }
    }

    public async void SendFriendRequest(GameObject friendName)
    {
        string friendN = friendName.GetComponent<Text>().text;
        if (friendN != null && friendN != "")
        {
            await GetUserAsync();
            await FirebaseDatabase.DefaultInstance.GetReference("users/" + friendN).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Cant find friend" + friendName);
                    return;
                }
                else if (task.IsCompleted)
                {
                    User u = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());
                    if (u == null)
                    {
                        Debug.Log("No existe ese usuario");
                    }
                    else if (!u.friendsRequest.Contains(CurrentUser.username) && !u.friends.Contains(CurrentUser.username) && u.username != CurrentUser.username)
                    {
                        u.friendsRequest.Add(CurrentUser.username);
                        FirebaseDatabase.DefaultInstance.GetReference("users/" + friendN).SetRawJsonValueAsync(JsonUtility.ToJson(u));
                        Debug.Log("Friend Request Sent");
                    }
                }

            });
        }
    }
    public async void AcceptFriendRequest(GameObject name)
    {
        string friend = name.GetComponent<Text>().text;
        await GetUserAsync();
        CurrentUser.friendsRequest.Remove(friend);
        CurrentUser.friends.Add(friend);
        await FirebaseDatabase.DefaultInstance.GetReference("users/" + CurrentUser.username).SetRawJsonValueAsync(JsonUtility.ToJson(CurrentUser));
        await AddedFriend(CurrentUser.username,friend);


    }

    private async Task AddedFriend(string friend,string original)
    {
        await FirebaseDatabase.DefaultInstance.GetReference("users/" + original).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Didnt add friend from other side");
                return;
            }
            else if (task.IsCompleted)
            {
                User u = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());
                u.friends.Add(friend);
                FirebaseDatabase.DefaultInstance.GetReference("users/" + original).SetRawJsonValueAsync(JsonUtility.ToJson(u));
            }
        });
    }

    public async void RejectFriendRequestAsync(GameObject name)
    {
        string friend = name.GetComponent<Text>().text;
        await GetUserAsync();
        CurrentUser.friendsRequest.Remove(friend);
        await FirebaseDatabase.DefaultInstance.GetReference("users/" + CurrentUser.username).SetRawJsonValueAsync(JsonUtility.ToJson(CurrentUser));
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
    }
