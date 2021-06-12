using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MockupButtonMovement : BaseButtonBehavior
{
    #region ---- VARIABLES ----
    #region --- PRIVATE ---

    #endregion

    #region --- PUBLIC ---
    public bool clickable = false;
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
        if (other.tag == "Goal")
        {
            clickable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Goal")
        {
            clickable = false;

            Invoke("DestroyButton", 1f);
        }
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion

    #endregion
}
