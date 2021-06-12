using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class MutatorMenuOps : MonoBehaviour
{
    public GameObject facil;
    public GameObject dificil;
    public GameObject mortal;
    public GameObject practica;
    public GameObject ciego;
    public GameObject visible;
    public GameObject combo;
    public GameObject precision;

    public User CurrentUser;
    // Start is called before the first frame update
    void Awake()
    {
        //Init();
    }
    public void Init()
    {
        combo.GetComponentInChildren<Button>().interactable = true;
        precision.GetComponentInChildren<Button>().interactable = true;
        facil.GetComponentInChildren<Button>().interactable = true;
        dificil.GetComponentInChildren<Button>().interactable = true;
        ciego.GetComponentInChildren<Button>().interactable = true;
        visible.GetComponentInChildren<Button>().interactable = true;
        mortal.GetComponentInChildren<Button>().interactable = true;
        practica.GetComponentInChildren<Button>().interactable = true;
        precision.GetComponentInChildren<Button>().interactable = true;
        GameManager.instance.info.mutators = new List<string>();
        GameManager.instance.info.mutators.Add("Facil");
        GameManager.instance.info.mutators.Add("Combo");
        GameManager.instance.info.mutators.Add("Mortal");
        GameManager.instance.info.mutators.Add("Visible");
        dificil.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
        practica.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
        ciego.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
        precision.GetComponent<Image>().color = new Color32(50, 50, 50, 200);

        facil.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        mortal.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        visible.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        combo.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        if (GameManager.instance.info.character == "Caxper")
        {
            precision.GetComponentInChildren<Button>().interactable = false;
        }
        if (GameManager.instance.info.character == "Mr Papa")
        {
            Random rnd = new Random();
            int i = rnd.Next(0, 2);
            if (i == 1)
            {
                FlipMut("Dificil");
            }
            i = rnd.Next(0, 2);
            if (i == 1)
            {
                FlipMut("Practica");
            }
            i = rnd.Next(0, 2);
            if (i == 1)
            {
                FlipMut("Ciego");
            }
            i = rnd.Next(0, 2);
            if (i == 1)
            {
                FlipMut("Precision");
            }
            combo.GetComponentInChildren<Button>().interactable = false;
            precision.GetComponentInChildren<Button>().interactable = false;
            facil.GetComponentInChildren<Button>().interactable = false;
            dificil.GetComponentInChildren<Button>().interactable = false;
            ciego.GetComponentInChildren<Button>().interactable = false;
            visible.GetComponentInChildren<Button>().interactable = false;
            mortal.GetComponentInChildren<Button>().interactable = false;
            practica.GetComponentInChildren<Button>().interactable = false;
            precision.GetComponentInChildren<Button>().interactable = false;


        }

    }
    public void FlipMut(string mut)
    {
        switch (mut)
        {
            case "Facil": 
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                        {
                        dificil.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
                        facil.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        GameManager.instance.info.mutators.Remove("Dificil");
                        GameManager.instance.info.mutators.Add("Facil");
                        }
                    break;
            case "Dificil":
                if (GameManager.instance.info.mutators.Contains("Facil"))
                {
                    facil.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
                    dificil.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    GameManager.instance.info.mutators.Remove("Facil");
                    GameManager.instance.info.mutators.Add("Dificil");
                }
                break;
            case "Mortal":
                if (GameManager.instance.info.mutators.Contains("Practica"))
                {
                    practica.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
                    mortal.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    GameManager.instance.info.mutators.Remove("Practica");
                    GameManager.instance.info.mutators.Add("Mortal");
                }
                break;
            case "Practica":
                if (GameManager.instance.info.mutators.Contains("Mortal"))
                {
                    mortal.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
                    practica.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    GameManager.instance.info.mutators.Remove("Mortal");
                    GameManager.instance.info.mutators.Add("Practica");
                }
                break;
            case "Ciego":
                if (GameManager.instance.info.mutators.Contains("Visible"))
                {
                    visible.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
                    ciego.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    GameManager.instance.info.mutators.Remove("Visible");
                    GameManager.instance.info.mutators.Add("Ciego");
                }
                break;
            case "Visible":
                if (GameManager.instance.info.mutators.Contains("Ciego"))
                {
                    ciego.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
                    visible.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    GameManager.instance.info.mutators.Remove("Ciego");
                    GameManager.instance.info.mutators.Add("Visible");
                }
                break;
            case "Combo":
                if (GameManager.instance.info.mutators.Contains("Precision"))
                {
                    precision.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
                    combo.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    GameManager.instance.info.mutators.Remove("Precision");
                    GameManager.instance.info.mutators.Add("Combo");
                }
                break;
            case "Precision":
                if (GameManager.instance.info.mutators.Contains("Combo"))
                {
                    combo.GetComponent<Image>().color = new Color32(50, 50, 50, 200);
                    precision.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    GameManager.instance.info.mutators.Remove("Combo");
                    GameManager.instance.info.mutators.Add("Precision");
                }
                break;
   
        }
    }
    public async void Play()
    {


        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            await GetUserAsync();
            CurrentUser.status = "Jugando";
            if (CurrentUser.username != "" && CurrentUser.username != null)
            {
                await FirebaseDatabase.DefaultInstance.GetReference("users/" + CurrentUser.username).SetRawJsonValueAsync(JsonUtility.ToJson(CurrentUser));
            }
        }


        SceneManager.LoadScene("SampleScene");
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
}
