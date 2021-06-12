using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    [SerializeField]
    const string MAIN_SCENE_NAME = "SampleScene";

    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public static GameManager instance = null;
    public int currentCharacter = 0;
    public InputManager inputManager;
    public ScoreManager scoreManager;
    public RhythmManager rhythmManager;
    public GameObject finalScreen;
    public GameObject gameOverScreen;
    public string infoForMultiplayer;
    public PlaySongInfo info;

    public GameObject singleMenu;
    public GameObject characterMenu;
    public GameObject mutatorsMenu;



    public bool CameFromPlay;

    public string FIRST_CHARACTER;
    public string SECOND_CHARACTER;
    public string THIRD_CHARACTER ;
    public string FOURTH_CHARACTER ;
    public string FIFTH_CHARACTER ; // used for scene management
    public struct PlaySongInfo
    {
        public string songname;
        public float bpm;
        public string character;
        public List<string> mutators;
        public string username;
    }
   
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---

    // Singleton pattern initialization
    void Awake()
    {
        CameFromPlay = false;

        if (instance == null)
        {
            FIRST_CHARACTER = "Topita";
            SECOND_CHARACTER = "Silvestre";
            THIRD_CHARACTER = "Mr Papa";
            FOURTH_CHARACTER = "Caxper";
            FIFTH_CHARACTER = "Floratoria";
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            instance.singleMenu = singleMenu;
            instance.characterMenu = characterMenu;
            instance.mutatorsMenu = mutatorsMenu;
            // se hace el caching de las variables aprovechando el objeto que está en la playscene, y luego se borra
            if (rhythmManager != null)
            {
                //scoreManager.gameOverScreen = gameOverScreen;
                
                scoreManager.finalScreen = finalScreen;
                instance.rhythmManager = rhythmManager;
                instance.scoreManager = scoreManager;
                instance.inputManager = inputManager;
            }
            Destroy(this.gameObject);
        }

        info.bpm = 120; // ???????

    }
    #endregion

    #region --- CUSTOM METHODS ---

    public void CameFromPlayScene()
    {
        CameFromPlay = true;
    }

    #endregion
    #endregion
}
