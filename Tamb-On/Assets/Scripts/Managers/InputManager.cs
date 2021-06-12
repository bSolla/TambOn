using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    ScoreManager scoreManager;

    const KeyCode INNER_LEFT_TAIKO = KeyCode.F;
    const KeyCode OUTER_LEFT_TAIKO = KeyCode.Q;
    const KeyCode INNER_RIGHT_TAIKO = KeyCode.J;
    const KeyCode OUTER_RIGHT_TAIKO = KeyCode.P;
    const KeyCode ESCAPE = KeyCode.Escape;
    private Color RED = new Color32((byte)255, (byte)139, (byte)148, 255);
    private Color GREEN = new Color32((byte)63, (byte)176, (byte)60, 255);
    private Color BLUE = new Color32((byte)16, (byte)174, (byte)186, 255);

    private bool pause;

    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public enum ButtonState
    {
        LATE = (1 << 0),
        GOOD = (1 << 1),
        PERFECT = (1 << 2)
    }

    public ButtonState innLeftFlags;
    public ButtonState innRighFlags;
    public ButtonState outLeftFlags;
    public ButtonState outRighFlags;

    public ButtonState innerFlags;
    public ButtonState outerFlags;


    public ButtonState innDrumrFlags; public int innDrumrHitsToClear;
    public ButtonState outDrumrFlags; public int outDrumrHitsToClear;
    public ButtonState innSustFlags; 
    public ButtonState outSustFlags;

    public enum ButtonStateIndex { LATE, GOOD, PERFECT }
    public BaseButtonBehavior[] destroyInnLeft = { default, default, default };
    public BaseButtonBehavior[] destroyInnRigh = { default, default, default };
    public BaseButtonBehavior[] destroyOutLeft = { default, default, default };
    public BaseButtonBehavior[] destroyOutRigh = { default, default, default };

    public BaseButtonBehavior[] destroyInner = { default, default, default };
    public BaseButtonBehavior[] destroyOuter = { default, default, default };

    public GameObject destroyInnDrumr = default;  //only storing one reference since it makes no sense for drumrolls to be placed so 
    public GameObject destroyOutDrumr = default;  //close together that they overlap
    public GameObject destroyInnSust = default;
    public GameObject destroyOutSust = default;

    public GameObject perfectPrefab;
    public GameObject comboObject;

    public GameObject randomNotePrefab;
    public GameObject mutatorPrefab;

    /*
    public GameObject randomNotePrefabLeft;
    public GameObject randomNotePrefabRight;
    public GameObject randomNote;
    */

    public Sprite innerLeft;
    public Sprite innerRight;
    public Sprite outerLeft;
    public Sprite outerRight;

    public Sprite blind;
    public Sprite hard;
    public Sprite easy;
    public Sprite combo;
    public Sprite precision;
    public Sprite mortal;
    public Sprite inmortal;

    public GameObject pausePlay;
    public GameObject continuePlay;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Start is called before the first frame update
    void Start()
    {
        pause = false;
        scoreManager = GameManager.instance.scoreManager;
        if (GameManager.instance.info.character == GameManager.instance.FIFTH_CHARACTER)
        {
            /*
            randomNotePrefabLeft.SetActive(true);
            randomNotePrefabRight.SetActive(true);
            randomNote.SetActive(true);
            */
            switch (GameManager.instance.scoreManager.noteS)
            {
                case "innerLeft": SetRandomNoteAnimation(innerLeft); break;
                case "innerRight": SetRandomNoteAnimation(innerRight); break;
                case "outerLeft": SetRandomNoteAnimation(outerLeft); break;
                case "outerRight": SetRandomNoteAnimation(outerRight); break;
            }
        }
        if (GameManager.instance.info.character == GameManager.instance.THIRD_CHARACTER)
        {
            mutatorPrefab.SetActive(true);
            Sprite first, second, third, fourth;
            if (GameManager.instance.info.mutators.Contains("Dificil")) first = hard;
            else first = easy;
            if (GameManager.instance.info.mutators.Contains("Combo")) second = combo;
            else second = precision;
            if (GameManager.instance.info.mutators.Contains("Mortal")) third = mortal;
            else third = inmortal;
            if (GameManager.instance.info.mutators.Contains("Ciego")) fourth = blind;
            else fourth = null;
            SetMutatorAnimation(first, second, third, fourth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPresses();

        //CheckReleases();
    }
    #endregion


    #region --- CUSTOM METHODS ---
    #region -- PRIVATE --
    public void SetHitAnimation(string name, Color color)
    {
        GameObject result = GameObject.FindGameObjectWithTag("perf");
        perfectPrefab.GetComponent<TextMesh>().text = name;
        perfectPrefab.GetComponent<TextMesh>().color = color;
        GameObject g = Instantiate(perfectPrefab, result.transform);
        g.GetComponent<Animator>().SetBool("playOnce", true);

        if (GameManager.instance.info.mutators.Contains("Combo") && GameManager.instance.scoreManager.acumulative > 0)
        {
            comboObject.GetComponent<TextMesh>().text = "X" + GameManager.instance.scoreManager.acumulative.ToString();
            comboObject.GetComponent<TextMesh>().color = color;
           
            comboObject.GetComponent<Animator>().SetBool("playOnce", true);
        }
        else
        {
            comboObject.GetComponent<TextMesh>().text = "";
        }
    }/*
    public void SetComboAnimation(string name, Color color)
    {
        GameObject result = GameObject.FindGameObjectWithTag("perf");
        perfectPrefab.GetComponent<TextMesh>().text = name;
        perfectPrefab.GetComponent<TextMesh>().color = color;
        GameObject g = Instantiate(perfectPrefab, result.transform);
        g.GetComponent<Animator>().SetBool("playOnce", true);
    }*/

    public void SetRandomNoteAnimation(Sprite note)
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("PlayCanvas");
        GameObject noteLeft = randomNotePrefab.transform.GetChild(1).gameObject;
        GameObject noteRight = randomNotePrefab.transform.GetChild(2).gameObject;
        noteLeft.GetComponent<Image>().sprite = note;
        noteRight.GetComponent<Image>().sprite = note;
        GameObject g = Instantiate(noteLeft, canvas.transform);
        GameObject h = Instantiate(noteRight, canvas.transform);
        g.GetComponent<Animator>().SetBool("left", true);
        h.GetComponent<Animator>().SetBool("right", true);

        /*
        randomNotePrefabLeft.GetComponent<Image>().sprite = note;
        randomNotePrefabRight.GetComponent<Image>().sprite = note;
        randomNote.GetComponent<Image>().sprite = note;
        randomNotePrefabRight.GetComponent<Animator>().SetBool("right", true);
        randomNotePrefabRight.GetComponent<Animator>().SetBool("playOnce", true);
        randomNotePrefabLeft.GetComponent<Animator>().SetBool("playOnce", true);
        */
    }

    public void SetMutatorAnimation(Sprite hard, Sprite combo, Sprite mortal, Sprite blind)
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("PlayCanvas");
        mutatorPrefab.transform.GetChild(1).GetComponent<Image>().sprite = hard;
        mutatorPrefab.transform.GetChild(2).GetComponent<Image>().sprite = combo;
        mutatorPrefab.transform.GetChild(3).GetComponent<Image>().sprite = mortal;
        mutatorPrefab.transform.GetChild(4).GetComponent<Image>().sprite = blind;
        GameObject g = Instantiate(mutatorPrefab, canvas.transform);
        g.GetComponent<Animator>().SetBool("playOnce", true);
    }
    

    private void CheckPresses()
    {
        if (Input.GetKeyDown(INNER_LEFT_TAIKO) && !pause)
        {
            if (!CheckInnerLeftTaiko() /*&& !CheckInnerSustain(true)*/ && !CheckInnerTaiko())
            {
                if (GameManager.instance.info.mutators.Contains("Dificil"))
                {
                    scoreManager.IncreaseFail("innerLeft");
                }
                else
                {
                    scoreManager.IncreaseFail("inner");
                }
                SetHitAnimation("FALLO", RED);
            }
        }
        else if (Input.GetKeyDown(INNER_RIGHT_TAIKO) && !pause)
        {
            if (!CheckInnerRightTaiko() /*&& !CheckInnerSustain(true)*/ && !CheckInnerTaiko())
            {
                if (GameManager.instance.info.mutators.Contains("Dificil"))
                {
                    scoreManager.IncreaseFail("innerRight");
                }
                else
                {
                    scoreManager.IncreaseFail("inner");
                }
                SetHitAnimation("FALLO", RED);

            }
        }
        else if (Input.GetKeyDown(OUTER_LEFT_TAIKO) && !pause)
        {
            if (!CheckOuterLeftTaiko() /*&& !CheckOuterSustain(true)*/ && !CheckOuterTaiko())
            {
                if (GameManager.instance.info.mutators.Contains("Dificil"))
                {
                    scoreManager.IncreaseFail("outerLeft");
                }
                else
                {
                    scoreManager.IncreaseFail("outer");
                }
                SetHitAnimation("FALLO", RED);

            }
        }
        else if (Input.GetKeyDown(OUTER_RIGHT_TAIKO) && !pause)
        {
            if (!CheckOuterRightTaiko() /*&& !CheckOuterSustain(true)*/ && !CheckOuterTaiko())
            {
                if (GameManager.instance.info.mutators.Contains("Dificil"))
                {
                    scoreManager.IncreaseFail("outerRight");
                }
                else
                {
                    scoreManager.IncreaseFail("outer");
                }
                SetHitAnimation("FALLO", RED);

            }
        }
        else if (Input.GetKeyDown(ESCAPE))
        {
            if (pause)
            {
                // GameManager.instance.rhythmManager.SetPause(false);
                pause = false;
                continuePlay.GetComponent<Button>().onClick.Invoke();
            }
            else {
                
                //GameManager.instance.rhythmManager.SetPause(true);
                pause = true;
                pausePlay.GetComponent<Button>().onClick.Invoke();
            }
            
        }
    }

    /*
    private void CheckReleases()
    {
        if (Input.GetKeyUp(INNER_LEFT_TAIKO))
        {
            if (!CheckInnerDrumroll() && !CheckInnerSustain(false))
            {
                //scoreManager.IncreaseFail("innerLeft");
            }
        }
        else if (Input.GetKeyUp(INNER_RIGHT_TAIKO))
        {
            if (!CheckInnerDrumroll() && !CheckInnerSustain(false))
            {
                //scoreManager.IncreaseFail("innerRight");
            }
        }
        else if (Input.GetKeyUp(OUTER_LEFT_TAIKO))
        {
            if (!CheckOuterDrumroll() && !CheckOuterSustain(false))
            {
                //scoreManager.IncreaseFail("outerLeft");
            }
        }
        else if (Input.GetKeyUp(OUTER_RIGHT_TAIKO))
        {
            if (!CheckOuterDrumroll() && !CheckOuterSustain(false))
            {
                //scoreManager.IncreaseFail("outerRight");
            }
        }
    }
    */


    private void DealWithFlags(BaseButtonBehavior toDestroy, ButtonState flags, ButtonState state, bool perfect)
    {
        flags &= ~state;
        //toDestroy.GetComponent<Animation>().Play();
        //toDestroy.GetComponent<Animator>().SetBool("playOnce", true);

        toDestroy.DestroyButton();
       
        if (perfect && GameManager.instance.info.character != GameManager.instance.FIRST_CHARACTER)
        {
            SetHitAnimation("PERFECTO", GREEN);
        }
        else if (GameManager.instance.info.character != GameManager.instance.SECOND_CHARACTER)
        {
            SetHitAnimation("BIEN", BLUE);
        }
        else
        {
            SetHitAnimation("FALLO", RED);
        }
        scoreManager.IncreaseScore(perfect);
    }


    private void CheckStates(BaseButtonBehavior toDestroy, ButtonState flags)
    {
        if ((flags & ButtonState.PERFECT) != 0) // PERFECT
        {
            DealWithFlags(toDestroy, flags, ButtonState.PERFECT, true);
        }
        else if ((flags & ButtonState.GOOD) != 0) // GOOD
        {
            DealWithFlags(toDestroy, flags, ButtonState.GOOD, false);
        }
        else if ((flags & ButtonState.LATE) != 0) // LATE 
        {
            DealWithFlags(toDestroy, flags, ButtonState.LATE, false);
        }
    }

    
    private bool GeneralizedSimpleButtonCheck(ref ButtonState buttonState, ref BaseButtonBehavior[] destroyArray)
    {
        bool hit = buttonState != 0;

        if ((buttonState & ButtonState.PERFECT) != 0) // PERFECT
        {
            DealWithFlags(destroyArray[(int)ButtonStateIndex.PERFECT], buttonState, ButtonState.PERFECT, true);            
        }
        else if ((buttonState & ButtonState.GOOD) != 0) // GOOD
        {
            DealWithFlags(destroyArray[(int)ButtonStateIndex.GOOD], buttonState, ButtonState.GOOD, false);
        }
        else if ((buttonState & ButtonState.LATE) != 0) // LATE
        {
            DealWithFlags(destroyArray[(int)ButtonStateIndex.LATE], buttonState, ButtonState.LATE, false);
        }
        if (hit) buttonState &= 0;

        return hit;
    }


    /*
    private bool GeneralizedSustainCheck(ref ButtonState flags, ref GameObject toDestroy, bool pressing, bool destroy)
    {
        bool hit = false;

        if (toDestroy != null)
        {
            BaseSustainButton sustComponent = toDestroy.GetComponent<BaseSustainButton>();

            if (pressing) //button pressed
            {
                sustComponent.hit = true;

                hit = (flags != 0);

                if (destroy) //if called from the button itself it means it can be destroyed
                {
                    CheckStates(toDestroy, flags);
                }
            }
            else //button released, miss
            {
                Destroy(toDestroy);
            }
        }
        //if (hit) flags &= 0;      No estoy seguro que esto funcione para sustain, en las notas normales debe estar para limpiar las flags despues del acierto (buttonState en vez de flags)

        return hit;
    }
    */

    /*
    private bool GeneralizedDrumrollCheck(ref ButtonState flags, ref GameObject toDestroy, ref int hitsToClear)
    {
        bool hit = flags != 0;

        if (hit) 
        {
            BaseDrumroll drumrollComponent = toDestroy.GetComponent<BaseDrumroll>();

            if ((flags & ButtonState.PERFECT) != 0) // PERFECT
            {
                if (hitsToClear == 1)
                {
                    DealWithFlags(toDestroy, flags, ButtonState.PERFECT, true);
                }
            }
            else if ((innDrumrFlags & ButtonState.GOOD) != 0) // GOOD
            {
                if (hitsToClear == 1)
                {
                    DealWithFlags(toDestroy, flags, ButtonState.GOOD, false);
                }
            }
            else if ((innDrumrFlags & ButtonState.LATE) != 0) // LATE
            {
                if (hitsToClear == 1)
                {
                    DealWithFlags(toDestroy, flags, ButtonState.LATE, false);
                }
            }

            hitsToClear--;
            if (drumrollComponent != null)
            {
                drumrollComponent.UpdateTextMesh(hitsToClear);
                drumrollComponent.hit = true;
            }
        }
        //if (hit) flags &= 0;      No estoy seguro que esto funcione para drumroll, en las notas normales debe estar para limpiar las flags despues del acierto (buttonState en vez de flags)

        return hit;
    }
    */

    #endregion

    #region -- PUBLIC --
    public bool CheckInnerLeftTaiko()
    {
        return GeneralizedSimpleButtonCheck(ref innLeftFlags, ref destroyInnLeft);
    }


    public bool CheckInnerRightTaiko()
    {
        return GeneralizedSimpleButtonCheck(ref innRighFlags, ref destroyInnRigh);
    }


    public bool CheckOuterLeftTaiko()
    {
        return GeneralizedSimpleButtonCheck(ref outLeftFlags, ref destroyOutLeft);
    }


    public bool CheckOuterRightTaiko()
    {
        return GeneralizedSimpleButtonCheck(ref outRighFlags, ref destroyOutRigh);
    }


    public bool CheckInnerTaiko()
    {
        return GeneralizedSimpleButtonCheck(ref innerFlags, ref destroyInner);
    }


    public bool CheckOuterTaiko()
    {
        return GeneralizedSimpleButtonCheck(ref outerFlags, ref destroyOuter);
    }


    /*
    public bool CheckInnerDrumroll()
    {
        return GeneralizedDrumrollCheck(ref innDrumrFlags, ref destroyInnDrumr, ref innDrumrHitsToClear);
    }
    */


    /*
    public bool CheckOuterDrumroll()
    {
        return GeneralizedDrumrollCheck(ref outDrumrFlags, ref destroyOutDrumr, ref outDrumrHitsToClear);
        
    }
    */

    /*
    public bool CheckInnerSustain(bool pressing, bool destroy = false)
    {
        return GeneralizedSustainCheck(ref innSustFlags, ref destroyInnSust, pressing, destroy);
    }
    */


    /*
    public bool CheckOuterSustain(bool pressing, bool destroy = false)
    {
        return GeneralizedSustainCheck(ref outSustFlags, ref destroyOutSust, pressing, destroy);
    }
    */

    #endregion
    #endregion
    #endregion
}
