using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
//using SmfLite;
//using Melanchall.DryWetMidi.Core;
//using Melanchall.DryWetMidi.Interaction;
using System;
using System.IO;
using UnityEngine.Networking;
using SmfLite;

public class RhythmManager : MonoBehaviour
{
    #region ---- VARIABLES ----
    #region --- PRIVATE ---
    [SerializeField]
    Spawner spawner;
    private List<double> times;
    private List<long> bpms;

    private bool pause;
    private GameObject[] notes;

    public AudioSource audioSource = default;
    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public TextAsset sourceFile;
    public float bpm; // beats per minute, ie tempo
    public float crotchet; // duration of a beat
    public float timeToReachGoal;
    public GameObject endGameMenu;
    public int totalNotes;
    public AudioClip monodyMp3;
    public AudioClip highscoreMp3;
    public AudioClip rockthedayMp3;

    public TextAsset monodyMid;
    public TextAsset highscoreMid;
    public TextAsset rockthedayMid;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // initialization of internal values
    void Awake()
    {
        times = new List<double>();
        bpms = new List<long>();
        bpm = GameManager.instance.info.bpm;
        crotchet = 60 / bpm;
        timeToReachGoal = crotchet * 4f;
        pause = false;
        totalNotes = 0;
    }
    
    void Start()
    {
        TextAsset midiFile = StringToTextAsset(GameManager.instance.info.songname);
        MidiFileContainer songNueva = MidiFileLoader.Load(midiFile.bytes);
        ProcessMidi(songNueva);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = StringToAudioClip(GameManager.instance.info.songname);
        audioSource.Play();
    }
    private AudioClip StringToAudioClip(string song)
    {
        switch (song)
        {
            case "Monody": return monodyMp3;
            case "Highscore": return highscoreMp3;
            case "RockTheDay": return rockthedayMp3;
        }
        return null;
    }
    private TextAsset StringToTextAsset(string song)
    {
        switch (song)
        {
            case "Monody": return monodyMid;
            case "Highscore": return highscoreMid;
            case "RockTheDay": return rockthedayMid;
        }
        return null;
    }
    void Update()
    {
        if (!audioSource.isPlaying && !pause)
        {
            GameManager.instance.scoreManager.FinalScreen();
        }
    }

    #endregion

    #region --- CUSTOM METHODS ---

    private void ProcessMidi(MidiFileContainer songNueva)
    {
        List<MidiTrack.DeltaEventPair> midtrack;
        if (songNueva.tracks[0].ToString() == "")
        {
            midtrack = songNueva.tracks[1].sequence;
        }
        else
        {
            midtrack = songNueva.tracks[0].sequence;
        }
        int absoluteDelta = 0;
        foreach (MidiTrack.DeltaEventPair note in midtrack)
        {
            absoluteDelta += note.delta;

            Spawner.ButtonData buttonData = new Spawner.ButtonData();
            buttonData.time = absoluteDelta / (bpm / 60 * songNueva.division);
            if ((note.midiEvent.status & 0xf0) == 0x90)
            {
                if (note.midiEvent.data1 == 0x30)
                {
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                    {
                        buttonData.buttonPrefab = spawner.buttonPrefabInnerLeft;
                    }
                    else
                    {
                        buttonData.buttonPrefab = spawner.buttonPrefabInnerLeft;
                    }
                }
                else if (note.midiEvent.data1 == 0x31)
                {
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                    {
                        buttonData.buttonPrefab = spawner.buttonPrefabInnerRight;
                    }
                    else
                    {
                        buttonData.buttonPrefab = spawner.buttonPrefabInnerRight;
                    }
                }
                else if (note.midiEvent.data1 == 0x32)
                {
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                    {
                        buttonData.buttonPrefab = spawner.buttonPrefabOuterLeft;
                    }
                    else
                    {
                        buttonData.buttonPrefab = spawner.buttonPrefabInnerLeft;
                    }
                }
                else if (note.midiEvent.data1 == 0x33)
                {
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                    {
                        buttonData.buttonPrefab = spawner.buttonPrefabOuterRight;
                    }
                    else
                    {
                        buttonData.buttonPrefab = spawner.buttonPrefabInnerRight;
                    }
                }
                totalNotes++;
                spawner.EnqueueButton(buttonData);
            }
        }
    }


    public void SetPause(bool isPaused)
    {
        if (isPaused)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
        BaseButtonBehavior n;
        pause = isPaused;
        spawner.pause = isPaused;
        notes = GameObject.FindGameObjectsWithTag("Note"); // TODO: link them up so we dont have to search for them in runtime
       
        foreach (GameObject note in notes)
        {
            n = (BaseButtonBehavior)note.GetComponent(typeof(BaseButtonBehavior));
            n.SetPause(isPaused);
        }
    }

    #endregion
    #endregion
}
