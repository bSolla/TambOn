using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerButtonBehavior : BaseButtonBehavior
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
                inputManager.innerFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyInner[(int)InputManager.ButtonStateIndex.GOOD] = this;
                break;

            case "Perfect":
                inputManager.innerFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyInner[(int)InputManager.ButtonStateIndex.PERFECT] = this;

                if (inputManager.destroyInnLeft[(int)InputManager.ButtonStateIndex.GOOD] == this)
                {
                    inputManager.innerFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                    inputManager.destroyInner[(int)InputManager.ButtonStateIndex.GOOD] = null; //clear the g.o. reference
                }
                break;

            case "Late":
                inputManager.innerFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyInner[(int)InputManager.ButtonStateIndex.LATE] = this;

                if (inputManager.destroyInner[(int)InputManager.ButtonStateIndex.PERFECT] == this)
                {
                    inputManager.innerFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
                    inputManager.destroyInner[(int)InputManager.ButtonStateIndex.PERFECT] = null; //clear the g.o. reference
                }
                
                break;

            default:
                Debug.Log("In InnerButtonBehavior::OnTriggerEnter --- hit unknown trigger");
                return; // out of the function for safety
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Late") //the other "exits" are dealth with in the Enter method
        {
            inputManager.innerFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyInner[(int)InputManager.ButtonStateIndex.LATE] = null;
            Invoke("DestroyButton", 1f);
            GameManager.instance.inputManager.SetHitAnimation("FALLO", RED);
            GameManager.instance.scoreManager.IncreaseFail("inner");
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion

    #endregion
}
