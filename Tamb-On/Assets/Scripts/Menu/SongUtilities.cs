using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class SongUtilities : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    [SerializeField] Image headerImage;
    [SerializeField] GameObject[] starRatings;
    [SerializeField] TextMeshProUGUI songTitle;
    [SerializeField] Text additionalInfo;
    [SerializeField] Text durationInfo;
    [SerializeField] int bpm;

    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public User CurrentUser;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    #endregion

    #region --- CUSTOM METHODS ---
    public void SetSongName(string songName)
    {
        songTitle.text = songName;
    }

    public void SetSongInfo(string songInfo)
    {
        additionalInfo.text = songInfo;
    }
    public void SetSongDuration(string songInfo)
    {
        durationInfo.text = songInfo;
    }
    public void SetBpm(int bpm)
    {
        this.bpm = bpm;
    }


    /// <summary>
    /// Sets the star rating of the song
    /// </summary>
    /// <param name="rating">A value from 1 star to 5 stars</param>
    public void SetStarRating (int rating)
    {
        starRatings[rating - 1].SetActive(true);
    }

    public void SetImageColor(Color imageColor)
    {
        headerImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0.75f);
    }

    private void AddDelMutator(string nombre, int mode)
    {
        if (nombre == "Ciego")
        {
            GameManager.instance.info.mutators.Add(nombre);
        }
        else if (nombre == "Combo" && mode == 0)
        {
            GameManager.instance.info.mutators.Add(nombre);
        }
        else if (nombre == "Combo")
        {
            GameManager.instance.info.mutators.Add("Precision");
        }
        else if (nombre == "Facil" && mode == 0)
        {
            GameManager.instance.info.mutators.Add(nombre);
        }
        else if (nombre == "Facil")
        {
            GameManager.instance.info.mutators.Add("Dificil");
        }
        else if (nombre == "Mortal" && mode == 0)
        {
            GameManager.instance.info.mutators.Add(nombre);
        }
        else if (nombre == "Mortal")
        {
            GameManager.instance.info.mutators.Add("Inmortal");
        }
    }

    public async void Play()
    {
        GameManager.instance.info.bpm = this.bpm;
        GameManager.instance.info.songname = songTitle.text;
        GameManager.instance.singleMenu.SetActive(false);
        GameManager.instance.characterMenu.SetActive(true);
        /*
        if (GameManager.instance.info.character == GameManager.instance.THIRD_CHARACTER)
        {
            List<string> totalMut = new List<string>() { "Mortal", "Ciego", "Facil", "Combo" };
            GameManager.instance.info.mutators.Clear();
            foreach (string s in totalMut)
            {
                int i = Random.Range(0, 2);
                AddDelMutator(s, i);
            }
        }
        if (GameManager.instance.info.character == GameManager.instance.FOURTH_CHARACTER)
        {
            if (GameManager.instance.info.mutators.Contains("Precision"))
            {
                GameManager.instance.info.mutators.Remove("Precision");

            }
            if (!GameManager.instance.info.mutators.Contains("Combo"))
            {
                GameManager.instance.info.mutators.Add("Combo");
            }
        }
            GameManager.instance.info.songname = songTitle.text;
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            await GetUserAsync();
            CurrentUser.status = "Jugando";
            if (CurrentUser.username != "")
            {
                await FirebaseDatabase.DefaultInstance.GetReference("users/" + CurrentUser.username).SetRawJsonValueAsync(JsonUtility.ToJson(CurrentUser));
            }
        }
        GameManager.instance.info.bpm = this.bpm;
        SceneManager.LoadScene("SampleScene");
        */
    }

    private async Task GetUserAsync()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        await FirebaseDatabase.DefaultInstance.GetReference("users/" + user.DisplayName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Current user fail");
                return;
            }
            else if (task.IsCompleted)
            {
                CurrentUser = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());
                Debug.Log("Got current user");
            }

        });

    }
    #endregion
    #endregion
}
