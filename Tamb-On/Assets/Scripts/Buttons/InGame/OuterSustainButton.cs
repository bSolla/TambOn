using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterSustainButton : BaseSustainButton
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
                //inputManager.CheckOuterSustain(true, true);
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
                inputManager.outSustFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyOutSust = gameObject;
                break;
            case "Perfect":
                inputManager.outSustFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyOutSust = gameObject;

                inputManager.outSustFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                break;
            case "Late":
                inputManager.outSustFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyOutSust = gameObject;

                inputManager.outSustFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
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
            inputManager.outSustFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyOutSust = null;
            Invoke("DestroyButton", 1f);
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion
    #endregion
}
