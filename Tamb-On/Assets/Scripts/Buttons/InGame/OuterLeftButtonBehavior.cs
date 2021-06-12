using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterLeftButtonBehavior : BaseButtonBehavior
{
    #region ---- VARIABLES ----
    #region --- PRIVATE ---
    private Color RED = new Color32((byte)255, (byte)139, (byte)148, 255);
    #endregion

    #region --- PUBLIC ---
    #endregion
    #endregion

    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Caching of variables and initialization
    void Start()
    {
        Initialize();
    }


    private void FixedUpdate()
    {
        MoveLeft();
    }

    private void OnTriggerEnter(Collider other)
    {
        string colliderTag = other.tag;

        switch (colliderTag)
        {
            case "Good":
                inputManager.outLeftFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyOutLeft[(int)InputManager.ButtonStateIndex.GOOD] = this;
                break;

            case "Perfect":
                inputManager.outLeftFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyOutLeft[(int)InputManager.ButtonStateIndex.PERFECT] = this;
                inputManager.outLeftFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                inputManager.destroyOutLeft[(int)InputManager.ButtonStateIndex.GOOD] = null; //clear the g.o. reference
                break;

            case "Late":
                inputManager.outLeftFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyOutLeft[(int)InputManager.ButtonStateIndex.LATE] = this;

                if (inputManager.destroyOutLeft[(int)InputManager.ButtonStateIndex.PERFECT] == this)
                {
                    inputManager.outLeftFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
                    inputManager.destroyOutLeft[(int)InputManager.ButtonStateIndex.PERFECT] = null; //clear the g.o. reference
                }
                
                break;

            default:
                Debug.Log("In OuterLeftButtonBehavior::OnTriggerEnter --- hit unknown trigger");
                return; // out of the function for safety
        }

        //if (other.tag == "Perfect")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLPerfect = true;
        //    GameManager.instance.inputManager.buttonToDestroyOLPerfect = gameObject;
        //}
        //else if (other.tag == "HitLeft")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLHitL = true;
        //    GameManager.instance.inputManager.buttonToDestroyOLHitL = gameObject;
        //}
        //else if (other.tag == "HitRight")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLHitR = true;
        //    GameManager.instance.inputManager.buttonToDestroyOLHitR = gameObject;
        //}
        //else if (other.tag == "MissLeft")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLMissL = true;
        //    GameManager.instance.inputManager.buttonToDestroyOLMissL = gameObject;
        //}
        //else if (other.tag == "MissRight")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLMissR = true;
        //    GameManager.instance.inputManager.buttonToDestroyOLMissR = gameObject;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Late") //the other "exits" are dealth with in the Enter method
        {
            inputManager.outLeftFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyOutLeft[(int)InputManager.ButtonStateIndex.LATE] = null;
            Invoke("DestroyButton", 1f);
            GameManager.instance.scoreManager.IncreaseFail("outerLeft");
            GameManager.instance.inputManager.SetHitAnimation("FALLO", RED);
            
        }
        //if (other.tag == "Perfect")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLPerfect = false;
        //    GameManager.instance.inputManager.buttonToDestroyOLPerfect = null;

        //    //GameManager.instance.scoreManager.ScorePenalty();

        //    Invoke("DestroyButton", 1f);
        //}
        //else if (other.tag == "HitLeft")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLHitL = false;
        //    GameManager.instance.inputManager.buttonToDestroyOLHitL = null;



        //    Invoke("DestroyButton", 1f);
        //}
        //else if (other.tag == "HitRight")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLHitR = false;
        //    GameManager.instance.inputManager.buttonToDestroyOLHitR = null;



        //    Invoke("DestroyButton", 1f);
        //}
        //else if (other.tag == "MissLeft")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLMissL = false;
        //    GameManager.instance.inputManager.buttonToDestroyOLMissL = null;


        //    //GameManager.instance.scoreManager.ScorePenalty();

        //    Invoke("DestroyButton", 1f);
        //}
        //else if (other.tag == "MissRight")
        //{
        //    GameManager.instance.inputManager.outerTaikoReadyLMissR = false;
        //    GameManager.instance.inputManager.buttonToDestroyOLMissR = null;


        //    Invoke("DestroyButton", 1f);
        //}
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion

    #endregion
}
