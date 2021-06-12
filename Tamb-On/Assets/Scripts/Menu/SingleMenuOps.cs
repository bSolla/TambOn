using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMenuOps : MonoBehaviour
{
    public SongUtilities botonCancion;
    public GameObject soloMenu;
    // Start is called before the first frame update
    void Start()
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
            currentSong.SetSongName(lines[0].Replace("\r", ""));
            currentSong.SetImageColor(new Color32((byte)Random.Range(180, 240), (byte)Random.Range(180, 240), (byte)Random.Range(180, 240), 255));
            currentSong.SetSongInfo(lines[1].Replace("\r", ""));
            currentSong.SetStarRating(System.Int32.Parse(lines[2]));
            currentSong.SetBpm(System.Int32.Parse(lines[3]));
            currentSong.SetSongDuration(lines[4].Replace("\r", ""));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
