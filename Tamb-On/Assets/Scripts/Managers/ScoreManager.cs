using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Auth;
using Firebase;
using System;
using System.Threading.Tasks;

public class ScoreManager : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
   [SerializeField]
    private Text scoreText;
    private Text scoreEndGame;
    private int note;

    const int PENALTY = 5;
    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public enum Chars { Topita, Gatito, Fantasma, Bailarina, Patata};
    public Chars currentChar;

    public string noteS;
    public float score;
    public int hits;
    public int bono;
    public bool noMortal;
    public bool fail;
    public bool combo;
    public bool topita; 
    public bool gatito;
    public bool bailarina;
    public bool fantasma;
    public int numFail;
    public int baseMod;
    public int acumulative;
    public float mutator;
    public GameObject finalScreen;
    public GameObject gameOverScreen;

    public bool personalRecord;
    public bool worldRecord;
    public int totalFail;
    public int totalGood;
    public int totalPerfect;
    public User CurrentUser;

    public bool authControl;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Start is called before the first frame update
    void Start()
    {
        personalRecord = false;
        worldRecord = false;
        totalFail = 0;
        totalGood = 0;
        totalPerfect = 0;
        noMortal = false;
        acumulative = 0;
        combo = true;
        topita = false;
        fantasma = false;
        bailarina = false;
        gatito = false;
        baseMod = 5;
        score = 0;
        hits = 0;
        mutator = CalculateModif(GameManager.instance.info.mutators);
        CalculateChar(GameManager.instance.info.character);
        numFail = 0;

        if (GameManager.instance.info.character == GameManager.instance.FIFTH_CHARACTER)
        {
            if (GameManager.instance.info.mutators.Contains("Dificil"))
            {
                note = UnityEngine.Random.Range(0, 4);
                switch (note)
                {
                    case 0:
                        noteS = "innerLeft";
                        break;
                    case 1:
                        noteS = "outerLeft";
                        break;
                    case 2:
                        noteS = "innerRight";
                        break;
                    case 3:
                        noteS = "outerRight";
                        break;
                    default:
                        throw new InvalidOperationException("Error al elegir nota aleatoria");
                }
            }
            else
            {
                note = UnityEngine.Random.Range(4, 6);
                switch (note)
                {
                    case 4:
                        noteS = "innerLeft";
                        break;
                    case 5:
                        noteS = "innerRight";
                        break;
                    default:
                        throw new InvalidOperationException("Error al elegir nota aleatoria");
                }
            }
        }

    }


    #endregion

    #region --- CUSTOM METHODS ---

    private void CalculateChar(string character)
    {
        if (character == GameManager.instance.FIRST_CHARACTER)
        {
            topita = true;
            currentChar = Chars.Topita;
        }
        if (character == GameManager.instance.SECOND_CHARACTER)
        {
            gatito = true;
            currentChar = Chars.Gatito;
        }
        if (character == GameManager.instance.FOURTH_CHARACTER)
        {
            fantasma = true;
            currentChar = Chars.Fantasma;
        }
        if (character == GameManager.instance.FIFTH_CHARACTER)
        {
            bailarina = true;
            currentChar = Chars.Bailarina;
        }
        if (character == GameManager.instance.THIRD_CHARACTER)
        {
            foreach (string mut in GameManager.instance.info.mutators)
            {
                mutator += 0.1f;
            }
        }
    }

    private float CalculateModif(List<string> mutators)
    {
        float result = 1;
        foreach (string mut in mutators)
        {
            if (mut == "Inmortal")
            {
                result *= 0.5f;
                noMortal = true;
            }
            if (mut == "Dificil")
            {
                result *= 1.2f;
            }
            if (mut == "Precision")
            {
                combo = false;
                baseMod = 3;
            }
            if (mut == "Ciego")
            {
                result *= 1.3f;
            }

        }
        return result;
    }

    public void IncreaseFail(string boton)
    {
        totalFail++;
        numFail++;
        acumulative = 0;

        if (numFail == 10 && !noMortal)
        {
            FinalScreen();
            //gameOverScreen.SetActive(true);
            GameManager.instance.rhythmManager.SetPause(true);
        }
        if (bailarina && boton == noteS)
        {
            bailarina = false;
        }
        if (fantasma)
        {
            score = score - (5 * numFail);
            if (score < 0)
            {
                score = 0;
            }
            scoreText.text = Math.Floor(score).ToString();
        }

    }
    public float ComboAccuracy(float first, float second)
    {
        if (combo)
        {
            return first + acumulative;
        }
        else
        {
            return second;
        }
    }

    public void IncreaseScore(bool perfect)
    {
        if (perfect)
        {
            totalPerfect++;
        }
        else
        {
            totalGood++;
        }
        numFail = 0;
        float scoreParcial = 10;

        if (topita)
        {
            scoreParcial = ComboAccuracy(20, 30);
            
        }
        else if (gatito)
        {
            if (perfect)
            {
                scoreParcial = ComboAccuracy(60, 100);
            }
            else
            {
                scoreParcial = 0;
            }            
        }
        else if (perfect) {
            scoreParcial = ComboAccuracy(30, 50);
        }
        scoreParcial *= mutator;
        score += scoreParcial;
        scoreText.text = Math.Floor(score).ToString();
        if (fantasma)
        {
            if (perfect)
            {
                acumulative += 2;
            }
            else
            {
                acumulative += 1;
            }
        }
        if (perfect && !topita)
        {
            acumulative += 2;
        }
        else
        {
            acumulative += 1;
        }
    }

    public void FinalScreen()
    {
        finalScreen.SetActive(true);
    }

    public void FinalClick()
    {
        authControl = true;
        if (bailarina)
        {
            score += (int)Math.Floor(((score / 100) * 30));
        }
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            return;
        }
        FirebaseDatabase.DefaultInstance.GetReference("Songs/"+GameManager.instance.info.songname).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Can't get user in database " + task.Exception);
                return;
            }
            else if (task.IsCompleted)
            {

                Records ToWrite = JsonUtility.FromJson<Records>(task.Result.GetRawJsonValue());
                if (ToWrite == null)
                {
                    ToWrite = new Records();
                }
                if (authControl)
                {
                    authControl = false;
                    CheckRecord(ToWrite, (int)Math.Floor(score));
                }
                

                 FirebaseDatabase.DefaultInstance.GetReference("Songs/"+GameManager.instance.info.songname).SetRawJsonValueAsync(JsonUtility.ToJson(ToWrite));

            }

        });

        string s = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + GameManager.instance.info.songname;
        FirebaseDatabase.DefaultInstance.GetReference("userSongs/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId+"/"+ GameManager.instance.info.songname.Replace("Songs/","")).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Can't get user in database " + task.Exception);
                return;
            }
            else if (task.IsCompleted)
            {

                Records ToWrite = JsonUtility.FromJson<Records>(task.Result.GetRawJsonValue());
                if (ToWrite == null)
                {
                    ToWrite = new Records();
                    ToWrite.scores.Add(-1);
                }
                if(ToWrite.scores[0] < Math.Floor(score))
                {
                    personalRecord = true;
                    ToWrite = new Records();
                    ToWrite.scores.Add((int)Math.Floor(score));
                    ToWrite.usernames.Add(GameManager.instance.info.username);
                    ToWrite.mutators.Add(Codificate(GameManager.instance.info.mutators));
                    ToWrite.character.Add(GameManager.instance.info.character);
                    if (FirebaseAuth.DefaultInstance.CurrentUser.UserId != "")
                    {
                        FirebaseDatabase.DefaultInstance.GetReference("userSongs/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/" + GameManager.instance.info.songname).SetRawJsonValueAsync(JsonUtility.ToJson(ToWrite));
                    }
                }
            }

        });
    }

    public async void ReturnToMenu()
    {
        FinalClick();
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            await GetUserAsync();
            CurrentUser.status = "En Linea";
            if (CurrentUser.username != "" && CurrentUser.username != null)
            {
                await FirebaseDatabase.DefaultInstance.GetReference("users/" + CurrentUser.username).SetRawJsonValueAsync(JsonUtility.ToJson(CurrentUser));
            }
        }
        SceneManager.LoadScene(0);
        GameManager.instance.CameFromPlayScene();
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
    public int Codificate(List<string> muts)
    {
        int i = 0;
        foreach (string mut in muts)
        {
            if(mut == "Precision")
            {
                i = i * 10;
                i = i + 1;
            }
            else if (mut == "Inmortal")
            {
                i = i * 10;
                i = i + 2;
            }
            else if (mut == "Ciego")
            {
                i = i * 10;
                i = i + 3;
            }
            else if (mut == "Dificil")
            {
                i = i * 10;
                i = i + 4;
            }
        }
        return i;
    }


    public Records CheckRecord(Records r, int score)
    {
        int i = 0, j = 0;
        bool wh = true;
        /*
        while (j < 100)
        {
            r.scores.Insert(j, score);
            r.usernames.Insert(j, GameManager.instance.info.username);
            r.mutators.Insert(j, Codificate(GameManager.instance.info.mutators));
            r.character.Insert(j, GameManager.instance.info.character);
            j++;
        }
        */
        
        while (i < r.scores.Count && wh)
        {
            if (r.scores[i] < score)
            {
                r.scores.Insert(i, score);
                r.usernames.Insert(i, GameManager.instance.info.username);
                r.mutators.Insert(i,Codificate(GameManager.instance.info.mutators));
                r.character.Insert(i,GameManager.instance.info.character);
                if (i < 10)
                {
                    worldRecord = true;
                }
                wh = false;
                if(r.scores.Count > 100)
                {
                    r.scores.RemoveAt(r.scores.Count-1);
                    r.usernames.RemoveAt(r.usernames.Count-1);
                    r.mutators.RemoveAt(r.scores.Count - 1);
                    r.character.RemoveAt(r.usernames.Count - 1);
                }
            }
            else
            {
                i++;
            }
        }
        if(r.scores.Count < 100 && wh)
        {
            r.scores.Add(score);
            r.usernames.Add(GameManager.instance.info.username);
            r.mutators.Add(Codificate(GameManager.instance.info.mutators));
            r.character.Add(GameManager.instance.info.character);
        }
        
        return r;
    }

    #endregion
    #endregion

}
