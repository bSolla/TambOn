using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RecordsOperations : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    private List<Records> storedRecords;
    private List<string> songnames;
    private List<Records> storedRecordsPer;
    private List<string> songnamesPer;
    private bool foundFriend;
    private string friendID;
    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public GameObject songname;
    public int index;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    public async void Initialize()
    {
        storedRecords = new List<Records>();
        songnames = new List<string>();
        storedRecordsPer = new List<Records>();
        songnamesPer = new List<string>();
        index = 0;
        await Refresh();
        TableFill();
    }

    
    #endregion

    #region --- CUSTOM METHODS ---
    public void NextSong()
    {
        index++;
        if (songnames.Count == index)
        {
            index = 0;
        }
        TableFill();
    }


    public void PreSong()
    {
        index--;
        if (-1 == index)
        {
            index = songnames.Count - 1;
        }
        TableFill();
    }


    public void TableFill()
    {
        songname.GetComponent<Text>().text = songnames[index];
        int i = 0;
        GameObject[] allChildren = GameObject.FindGameObjectsWithTag("FilaRecord");
        foreach (GameObject fila in allChildren)
        {
            Text[] t = fila.GetComponentsInChildren<Text>();
            if (i < storedRecords[index].scores.Count)
            {

                t[0].text = (i+1).ToString();
                t[2].text = storedRecords[index].scores[i].ToString();
                t[1].text = storedRecords[index].usernames[i];
                t[3].text = storedRecords[index].character[i];
                //t[4].text = "ja";
                t[4].text = decodificate(storedRecords[index].mutators[i].ToString());
                if (storedRecords[index].usernames[i] == GameManager.instance.info.username)
                {
                    fila.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    fila.GetComponent<Image>().color = new Color(218,218,255);
                }
            }
            else
            {
                t[0].text = "";
                t[1].text = "";
                t[2].text = "";
                t[3].text = "";
                t[4].text = "";
                fila.GetComponent<Image>().color = new Color(218, 218, 255);
            }
            i++;
        }
        GameObject personal = GameObject.FindGameObjectWithTag("FilaRecordPersonal");
        Text[] tAux = personal.GetComponentsInChildren<Text>();
            tAux[0].text = "";
            tAux[1].text = "";
            tAux[2].text = "";
            tAux[3].text = "";
            tAux[4].text = "";
        for (int j = 0;j < songnamesPer.Count; j++)
        {
            if (songnamesPer[j] == songnames[index])
            {
                bool found = false;
                int pos = 0;
                while (!found && pos < storedRecords[index].usernames.Count)
                {
                    if(storedRecords[index].usernames[pos] == storedRecordsPer[j].usernames[0])
                    {
                        found = true;
                    }
                    else
                    {
                        pos++;
                    }
                }
                if (found)
                {
                    tAux[0].text = (pos + 1).ToString();
                }
                else
                {
                    tAux[0].text = "+100";
                }
                tAux[2].text = storedRecordsPer[j].scores[0].ToString();
                tAux[1].text = storedRecordsPer[j].usernames[0];
                tAux[3].text = storedRecordsPer[j].character[0];
                tAux[4].text = storedRecordsPer[j].mutators[0].ToString();
            }
        }
    }

    private string decodificate(string v)
    {
        int s = Int32.Parse(v);
        string dec = "";
        while (s > 0)
        {
            if (s % 10 == 1)
            {
                dec = dec + "precision";
            }
            else if (s % 10 == 2)
            {
                dec = dec + "inmortal";
            }
            else if (s % 10 == 3)
            {
                dec = dec + "ciego";
            }
            else if (s % 10 == 4)
            {
                dec = dec + "dificil";
            }
            if (s > 4)
            {
                dec = dec + ", ";
            }
            s = s / 10;
            
        }
        return dec;
    }

    public async Task Refresh()
    {
        await FirebaseDatabase.DefaultInstance.GetReference("Songs").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Can,t get user in database " + task.Exception);
                return;
            }
            else if (task.IsCompleted)
            {
                foreach (DataSnapshot childs in task.Result.Children)
                {
                    storedRecords.Add(JsonUtility.FromJson<Records>(childs.GetRawJsonValue()));
                    songnames.Add(childs.Key);
                }

            }

        });
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            await FirebaseDatabase.DefaultInstance.GetReference("userSongs/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
           {
               if (task.IsFaulted)
               {
                   Debug.LogError("Can,t get user in database " + task.Exception);
                   return;
               }
               else if (task.IsCompleted)
               {
                   foreach (DataSnapshot childs in task.Result.Children)
                   {
                       storedRecordsPer.Add(JsonUtility.FromJson<Records>(childs.GetRawJsonValue()));
                       songnamesPer.Add(childs.Key);
                   }

               }

           });
        }
    }
    #endregion
    #endregion
}
