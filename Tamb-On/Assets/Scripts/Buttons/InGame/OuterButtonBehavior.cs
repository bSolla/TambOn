using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterButtonBehavior : BaseButtonBehavior
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
                inputManager.outerFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyOuter[(int)InputManager.ButtonStateIndex.GOOD] = this;
                break;

            case "Perfect":
                inputManager.outerFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyOuter[(int)InputManager.ButtonStateIndex.PERFECT] = this;
                inputManager.outerFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                inputManager.destroyOuter[(int)InputManager.ButtonStateIndex.GOOD] = null; //clear the g.o. reference
                break;

            case "Late":
                inputManager.outerFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyOuter[(int)InputManager.ButtonStateIndex.LATE] = this;

                if (inputManager.destroyOuter[(int)InputManager.ButtonStateIndex.PERFECT] == this)
                {
                    inputManager.outerFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
                    inputManager.destroyOuter[(int)InputManager.ButtonStateIndex.PERFECT] = null; //clear the g.o. reference
                }
                
                break;

            default:
                Debug.Log("In OuterButtonBehavior::OnTriggerEnter --- hit unknown trigger");
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
            inputManager.outerFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyOuter[(int)InputManager.ButtonStateIndex.LATE] = null;
            Invoke("DestroyButton", 1f);
            GameManager.instance.scoreManager.IncreaseFail("outer");
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
