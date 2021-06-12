using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSustainButton : BaseButtonBehavior
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---

    #endregion
    #region --- PROTECTED ---
    [SerializeField]
    [Tooltip("Reference to the tail of the button")]
    protected Transform tail;
    protected const float TAIL_CUT = 0.1f;

    [SerializeField]
    [Tooltip("Reference to the cosmetic tail end")]
    protected Transform tailEnd;

    [SerializeField]
    [Tooltip("Reference to the anchor for the tail end")]
    protected Transform endAnchor;



    protected Vector3 initialAnchorPosition;
    #endregion
    #region --- PUBLIC ---
    public bool hit = false;
    public int duration = 4;    // we'll need to read this from the midi
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        tail.localScale = new Vector3(duration * tail.localScale.x, tail.localScale.y, tail.localScale.z);
        initialAnchorPosition = endAnchor.localPosition;
        tailEnd.position = endAnchor.position;
    }

    #endregion

    #region --- CUSTOM METHODS ---
    /// <summary>
    /// Scales the tail of the button according to the speed set for the button
    /// </summary>
    /// <returns>true when the tail reaches a value lesser or equal to TAIL_CUT</returns>
    protected bool ScaleTail()
    {
        if (tail.localScale.x <= TAIL_CUT)
            return true;

        float step = speed * Time.deltaTime;
        tail.localScale = new Vector3(tail.localScale.x - step, tail.localScale.y, tail.localScale.z);

        tailEnd.position = new Vector3(endAnchor.position.x, tailEnd.position.y, tailEnd.position.z);

        return false;
    }
    #endregion
    #endregion
}
