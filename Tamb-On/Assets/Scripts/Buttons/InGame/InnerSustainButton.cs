using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerSustainButton : BaseSustainButton
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---

    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    private void FixedUpdate()
    {
        if (!hit)
            MoveLeft();
        else
        {
            if (ScaleTail()) // true when the tail gets very small
            {
                //inputManager.CheckInnerSustain(true, true);
                DestroyButton();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string colliderTag = other.tag;

        switch (colliderTag)
        {
            case "Good":
                inputManager.innSustFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyInnSust = gameObject;
                break;
            case "Perfect":
                inputManager.innSustFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyInnSust = gameObject;

                inputManager.innSustFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                break;
            case "Late":
                inputManager.innSustFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyInnSust = gameObject;

                inputManager.innSustFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
                break;
            default:
                hit = false;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Late") //the other "exits" are dealth with in the Enter method
        {
            inputManager.innSustFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyInnSust = null;
            Invoke("DestroyButton", 1f);
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion
    #endregion
}
