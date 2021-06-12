using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InnerDrumroll : BaseDrumroll
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

    private void OnTriggerEnter(Collider other)
    {
        string colliderTag = other.tag;

        switch(colliderTag)
        {
            case "Good":
                inputManager.innDrumrFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyInnDrumr = gameObject;
                inputManager.innDrumrHitsToClear = hitsToClear;
                break;
            case "Perfect":
                inputManager.innDrumrFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyInnDrumr = gameObject;

                inputManager.innDrumrFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                break;
            case "Late":
                inputManager.innDrumrFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyInnDrumr = gameObject;

                inputManager.innDrumrFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
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
            inputManager.innDrumrFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyInnDrumr = null;
            inputManager.innDrumrHitsToClear = 0;
            Invoke("DestroyButton", 1f);
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion
    #endregion
}
