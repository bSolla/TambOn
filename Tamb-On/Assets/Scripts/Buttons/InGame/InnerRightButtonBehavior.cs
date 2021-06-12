using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerRightButtonBehavior : BaseButtonBehavior
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
                inputManager.innRighFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyInnRigh[(int)InputManager.ButtonStateIndex.GOOD] = this;
                break;

            case "Perfect":
                inputManager.innRighFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyInnRigh[(int)InputManager.ButtonStateIndex.PERFECT] = this;
                inputManager.innRighFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                inputManager.destroyInnRigh[(int)InputManager.ButtonStateIndex.GOOD] = null; //clear the g.o. reference
                break;

            case "Late":
                inputManager.innRighFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyInnRigh[(int)InputManager.ButtonStateIndex.LATE] = this;

                if (inputManager.destroyInnRigh[(int)InputManager.ButtonStateIndex.PERFECT] == this)
                {
                    inputManager.innRighFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
                    inputManager.destroyInnRigh[(int)InputManager.ButtonStateIndex.PERFECT] = null; //clear the g.o. reference
                }
                
                
                break;

            default:
                Debug.Log("In InnerRightButtonBehavior::OnTriggerEnter --- hit unknown trigger");
                return; // out of the function for safety
        }
        /*
        if (other.tag == "Perfect")
        {
            GameManager.instance.inputManager.innerTaikoReadyRPerfect = true;
            GameManager.instance.inputManager.buttonToDestroyIRPerfect = gameObject;
        }
        else if (other.tag == "HitLeft")
        {
            GameManager.instance.inputManager.innerTaikoReadyRHitL = true;
            GameManager.instance.inputManager.buttonToDestroyIRHitL = gameObject;
        }
        else if (other.tag == "HitRight")
        {
            GameManager.instance.inputManager.innerTaikoReadyRHitR = true;
            GameManager.instance.inputManager.buttonToDestroyIRHitR = gameObject;
        }
        else if (other.tag == "MissLeft")
        {
            GameManager.instance.inputManager.innerTaikoReadyRMissL = true;
            GameManager.instance.inputManager.buttonToDestroyIRMissL = gameObject;
        }
        else if (other.tag == "MissRight")
        {
            GameManager.instance.inputManager.innerTaikoReadyRMissR = true;
            GameManager.instance.inputManager.buttonToDestroyIRMissR = gameObject;
        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Late") //the other "exits" are dealth with in the Enter method
        {
            inputManager.innRighFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyInnRigh[(int)InputManager.ButtonStateIndex.LATE] = null;
            Invoke("DestroyButton", 1f);
            GameManager.instance.scoreManager.IncreaseFail("innerRight");
            GameManager.instance.inputManager.SetHitAnimation("FALLO", RED);
            
        }
        /*
        if (other.tag == "Perfect")
        {
            GameManager.instance.inputManager.innerTaikoReadyRPerfect = false;
            GameManager.instance.inputManager.buttonToDestroyIRPerfect = null;


            //GameManager.instance.scoreManager.ScorePenalty();

            Invoke("DestroyButton", 1f);
        }
        else if (other.tag == "HitLeft")
        {
            GameManager.instance.inputManager.innerTaikoReadyRHitL = false;
            GameManager.instance.inputManager.buttonToDestroyIRHitL = null;



            Invoke("DestroyButton", 1f);
        }
        else if (other.tag == "HitRight")
        {
            GameManager.instance.inputManager.innerTaikoReadyRHitR = false;
            GameManager.instance.inputManager.buttonToDestroyIRHitR = null;



            Invoke("DestroyButton", 1f);
        }
        else if (other.tag == "MissLeft")
        {
            GameManager.instance.inputManager.innerTaikoReadyRMissL = false;
            GameManager.instance.inputManager.buttonToDestroyIRMissL = null;


            //GameManager.instance.scoreManager.ScorePenalty();

            Invoke("DestroyButton", 1f);
        }
        else if (other.tag == "MissRight")
        {
            GameManager.instance.inputManager.innerTaikoReadyRMissR = false;
            GameManager.instance.inputManager.buttonToDestroyIRMissR = null;


            Invoke("DestroyButton", 1f);
        }
        */
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion

    #endregion
}
