using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterRightButtonBehavior : BaseButtonBehavior
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
                inputManager.outRighFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyOutRigh[(int)InputManager.ButtonStateIndex.GOOD] = this;
                break;

            case "Perfect":
                inputManager.outRighFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyOutRigh[(int)InputManager.ButtonStateIndex.PERFECT] = this;
                inputManager.outRighFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                inputManager.destroyOutRigh[(int)InputManager.ButtonStateIndex.GOOD] = null; //clear the g.o. reference
                break;

            case "Late":
                inputManager.outRighFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyOutRigh[(int)InputManager.ButtonStateIndex.LATE] = this;

                if (inputManager.destroyOutRigh[(int)InputManager.ButtonStateIndex.PERFECT] == this)
                {
                    inputManager.outRighFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
                    inputManager.destroyOutRigh[(int)InputManager.ButtonStateIndex.PERFECT] = null; //clear the g.o. reference
                }
                
                break;

            default:
                Debug.Log("In OuterLeftButtonBehavior::OnTriggerEnter --- hit unknown trigger");
                return; // out of the function for safety
        }
        /*
        if (other.tag == "Perfect")
        {
            GameManager.instance.inputManager.outerTaikoReadyRPerfect = true;
            GameManager.instance.inputManager.buttonToDestroyORPerfect = gameObject;
        }
        else if (other.tag == "HitLeft")
        {
            GameManager.instance.inputManager.outerTaikoReadyRHitL = true;
            GameManager.instance.inputManager.buttonToDestroyORHitL = gameObject;
        }
        else if (other.tag == "HitRight")
        {
            GameManager.instance.inputManager.outerTaikoReadyRHitR = true;
            GameManager.instance.inputManager.buttonToDestroyORHitR = gameObject;
        }
        else if (other.tag == "MissLeft")
        {
            GameManager.instance.inputManager.outerTaikoReadyRMissL = true;
            GameManager.instance.inputManager.buttonToDestroyORMissL = gameObject;
        }
        else if (other.tag == "MissRight")
        {
            GameManager.instance.inputManager.outerTaikoReadyRMissR = true;
            GameManager.instance.inputManager.buttonToDestroyORMissR = gameObject;
        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Late") //the other "exits" are dealth with in the Enter method
        {
            inputManager.outRighFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyOutRigh[(int)InputManager.ButtonStateIndex.LATE] = null;
            Invoke("DestroyButton", 1f);
            GameManager.instance.scoreManager.IncreaseFail("outerRight");
            GameManager.instance.inputManager.SetHitAnimation("FALLO", RED);
           
        }
        /*
        if (other.tag == "Perfect")
        {
            GameManager.instance.inputManager.outerTaikoReadyRPerfect = false;
            GameManager.instance.inputManager.buttonToDestroyORPerfect = null;


            //GameManager.instance.scoreManager.ScorePenalty();

            Invoke("DestroyButton", 1f);
        }
        else if (other.tag == "HitLeft")
        {
            GameManager.instance.inputManager.outerTaikoReadyRHitL = false;
            GameManager.instance.inputManager.buttonToDestroyORHitL = null;



            Invoke("DestroyButton", 1f);
        }
        else if (other.tag == "HitRight")
        {
            GameManager.instance.inputManager.outerTaikoReadyRHitR = false;
            GameManager.instance.inputManager.buttonToDestroyORHitR = null;



            Invoke("DestroyButton", 1f);
        }
        else if (other.tag == "MissLeft")
        {
            GameManager.instance.inputManager.outerTaikoReadyRMissL = false;
            GameManager.instance.inputManager.buttonToDestroyORMissL = null;


            //GameManager.instance.scoreManager.ScorePenalty();

            Invoke("DestroyButton", 1f);
        }
        else if (other.tag == "MissRight")
        {
            GameManager.instance.inputManager.outerTaikoReadyRMissR = false;
            GameManager.instance.inputManager.buttonToDestroyORMissR = null;


            Invoke("DestroyButton", 1f);
        }
        */
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion

    #endregion
}
