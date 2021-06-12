using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuOperations : MonoBehaviour
{

    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    private List<string> totalMut = new List<string>() { "Mortal", "Ciego", "Facil", "Combo" };
    #endregion
    #region --- PROTECTED ---

    #endregion

    #region --- PUBLIC ---
    public GameObject mut;
    public GameObject scrollMut;
    public GameObject scrollMutContent;
    public GameObject character;
    public SongUtilities botonCancion;
    public GameObject mutatorPrefab;
    public GameObject comboFantasma;

    public Sprite floratoria;
    public Sprite topita;
    public Sprite mrpapa;
    public Sprite silvestre;
    public Sprite caxper;

    public Sprite facil;
    public Sprite dificil;
    public Sprite ciego;
    public Sprite combo;
    public Sprite precision; 
    public Sprite mortal;
    public Sprite inmortal;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region --- CUSTOM METHODS ---



    public void FillSongs(GameObject soloMenu)
    {
        foreach (Transform child in soloMenu.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        string path = "Songs/";
        UnityEngine.Object[] fileInfo = Resources.LoadAll(path, typeof(TextAsset));
      
        foreach (TextAsset s in fileInfo)
        {
            
            string[] lines = s.text.Split('\n');
            SongUtilities currentSong = Instantiate(botonCancion, soloMenu.transform);
            currentSong.SetSongName(lines[0].Replace("\r",""));
            currentSong.SetImageColor(new Color((byte)Random.Range(180, 240), (byte)Random.Range(180, 240), (byte)Random.Range(180, 240), 0.75f));
            currentSong.SetSongInfo(lines[1].Replace("\r", ""));
            currentSong.SetStarRating(System.Int32.Parse(lines[2]));
            currentSong.SetBpm(System.Int32.Parse(lines[3]));
            currentSong.SetSongDuration(lines[4].Replace("\r", ""));
        }
    }



    public void RefreshCharacter(string ch)
    {
        TextMeshProUGUI t = character.GetComponentInChildren<TextMeshProUGUI>();
        Image i = character.GetComponent<Image>();
        t.text = ch;
        i.sprite = StringToSprite(ch);
        GameManager.instance.info.character = ch;
    }
    private Sprite StringToSprite(string ch)
    {
        switch (ch)
        {
            case "Topita": return topita;
            case "Silvestre": return silvestre;
            case "Mr Papa": return mrpapa;
            case "Caxper": return caxper;
            case "Floratoria": return floratoria;
            case "Facil": return facil;
            case "Dificil": return dificil;
            case "Combo": return combo;
            case "Precision": return precision;
            case "Mortal": return mortal;
            case "Inmortal": return inmortal;
            case "Ciego": return ciego;

        }
        return null;
    }


    #endregion
    #endregion
}
