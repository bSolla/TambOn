using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyControl : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---

    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public LobbyDB currentLobby;

    public GameObject lobbyPrefab;
    public GameObject lobbyScroll;

    public GameObject auth;
    public string uname;
    public List<LobbyDB> allLobbies;
    public GameObject sala;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Update is called once per frame 
    void Update()
    {
        // TODO: change to avoid checking every frame
        if (GameManager.instance.infoForMultiplayer != null && GameManager.instance.infoForMultiplayer != "")
        {
            sala.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---
    public async void Refresh()
    {
        //uname = auth.GetComponent<AuthControl>().userDB.username;
        allLobbies = new List<LobbyDB>();
        await RefreshAllGames();
        ReWrite();
    }

    private void ReWrite()
    {
        foreach (GameObject t in lobbyScroll.transform)
        {
            Debug.Log(lobbyScroll.transform.childCount);
            Destroy(t);
        }
        foreach (LobbyDB auxLobby in allLobbies)
        {

            Text[] t = lobbyPrefab.GetComponentsInChildren<Text>();

            t[0].text = auxLobby.lobbyName;
            t[1].text = auxLobby.players.Count.ToString();
            t[2].text = auxLobby.hostPlayer;
            Instantiate(lobbyPrefab, lobbyScroll.transform);
        }
    }

    public async Task RefreshAllGames()
    {
        await FirebaseDatabase.DefaultInstance.GetReference("lobby").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Can,t get user in database " + task.Exception);
                return;
            }
            else if (task.IsCompleted)
            {
                foreach (var childs in task.Result.Children)
                {

                    allLobbies.Add(JsonUtility.FromJson<LobbyDB>(childs.GetRawJsonValue()));

                    FirebaseDatabase.DefaultInstance.GetReference("lobby/" + childs.Key).ValueChanged += HandleValueChangedForLobby;

                }
            }

        });
    }

    void HandleValueChangedForLobby(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        LobbyDB auxLobby = JsonUtility.FromJson<LobbyDB>(args.Snapshot.GetRawJsonValue());
        bool isNew = true;
        for (int i = 0; i < allLobbies.Count; i++)
        {
            if (allLobbies[i].lobbyName.Contains(auxLobby.lobbyName))
            {
                isNew = false;
                allLobbies[i] = auxLobby;
            }
        }
        if (isNew)
        {
            allLobbies.Add(auxLobby);
        }
    }

    public void CreateNewRoom(GameObject g)
    {
        string lobbyName = g.GetComponent<Text>().text;
        currentLobby = new LobbyDB(lobbyName, "", new List<string>(), new List<string>(), uname);
        currentLobby.players.Add(uname);
        GameManager.instance.infoForMultiplayer = lobbyName;
        FirebaseDatabase.DefaultInstance.GetReference("lobby/" + lobbyName).SetRawJsonValueAsync(JsonUtility.ToJson(currentLobby));
        // FirebaseDatabase.DefaultInstance.GetReference("lobby/" + lobbyName).ValueChanged += HandleValueChangedForMyLobby;
    }
    #endregion
    #endregion

}
