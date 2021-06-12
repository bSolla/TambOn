using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region ---- VARIABLES ----
    #region --- PRIVATE ---
    RhythmManager rhythmManager;
    float bpm, crotchet = 0;
    float timeToReachGoal;

    Queue<ButtonData> spawnQueue = new Queue<ButtonData>();
    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    // HARDCORE BUTTONS
    public BaseButtonBehavior buttonPrefabInnerLeft;
    public BaseButtonBehavior buttonPrefabInnerRight;
    public BaseButtonBehavior buttonPrefabOuterLeft;
    public BaseButtonBehavior buttonPrefabOuterRight;

    // SIMPLE BUTTONS
    public BaseButtonBehavior buttonPrefabInner;
    public BaseButtonBehavior buttonPrefabOuter;

    // LONG BUTTONS
    public BaseDrumroll buttonPrefabInnerDrumroll;
    public BaseDrumroll buttonPrefabOuterDrumroll;
    public BaseSustainButton buttonPrefabInnerSustain;
    public BaseSustainButton buttonPrefabOuterSustain;

    public enum ButtonType
    {
        INNER_LEFT, INNER_RIGHT, OUTER_LEFT, OUTER_RIGHT,
        INNER_DRUMROLL, OUTER_DRUMROLL, INNER_SUSTAIN, OUTER_SUSTAIN,
        INNER, OUTER
    };

    public struct ButtonData
    {
        public BaseButtonBehavior buttonPrefab;
        public double time;
    };

    public bool pause;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Start is called before the first frame update
    void Start()
    {
        rhythmManager = GameManager.instance.rhythmManager;
        bpm = rhythmManager.bpm;
        crotchet = 60 / bpm;
        timeToReachGoal = rhythmManager.timeToReachGoal;
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnQueue.Count != 0 && rhythmManager.audioSource.time-0.3f > spawnQueue.Peek().time - timeToReachGoal && !pause)
        {
            Instantiate(spawnQueue.Peek().buttonPrefab, this.transform, false);

            spawnQueue.Dequeue();
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---
    public void EnqueueButton(ButtonData button)
    {
        spawnQueue.Enqueue(button);
    }
    #endregion
    #endregion
}
