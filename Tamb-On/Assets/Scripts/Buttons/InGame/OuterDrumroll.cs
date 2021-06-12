using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterDrumroll : BaseDrumroll
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

        switch (colliderTag)
        {
            case "Good":
                inputManager.outDrumrFlags |= InputManager.ButtonState.GOOD;
                inputManager.destroyOutDrumr = gameObject;
                inputManager.outDrumrHitsToClear = hitsToClear;
                break;
            case "Perfect":
                inputManager.outDrumrFlags |= InputManager.ButtonState.PERFECT;
                inputManager.destroyOutDrumr = gameObject;

                inputManager.outDrumrFlags &= ~InputManager.ButtonState.GOOD; //it just left the Good area, so put that flag to false
                break;
            case "Late":
                inputManager.outDrumrFlags |= InputManager.ButtonState.LATE;
                inputManager.destroyOutDrumr = gameObject;

                inputManager.outDrumrFlags &= ~InputManager.ButtonState.PERFECT; //it just left the Perfect area, so put that flag to false
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
            inputManager.outDrumrFlags &= ~InputManager.ButtonState.LATE;
            inputManager.destroyOutDrumr = null;
            inputManager.outDrumrHitsToClear = 0;
            Invoke("DestroyButton", 1f);
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion
    #endregion
}
