using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseDrumroll : BaseButtonBehavior
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---

    #endregion
    #region --- PROTECTED ---
 

    [SerializeField]
    [Tooltip("Reference to the tail of the button")]
    protected Transform tail;
    protected const float TAIL_CUT = 0.05f;

    [SerializeField]
    [Tooltip("Reference to the text mesh of the button")]
    protected TextMeshPro countText;
    #endregion

    #region --- PUBLIC ---
    public bool hit = false;
    public int hitsToClear = 4;    // we'll need to read this from the midi
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        countText.text = hitsToClear.ToString();
        tail.localScale = new Vector3(hitsToClear * tail.localScale.x, tail.localScale.y, tail.localScale.z);
    }

    private void FixedUpdate()
    {
        if (!hit)
            MoveLeft();
        else
        {
            if (ScaleTail()) // true when the tail gets very small
            {
                DestroyButton();
            }
        }
    }

    #endregion

    #region --- CUSTOM METHODS ---
    /// <summary>
    /// Scales the tail of the button according to the speed set for the button
    /// </summary>
    /// <returns>true when the tail reaches a value lesser or equal to TAIL_CUT</returns>
    protected bool ScaleTail()
    {
        float step = speed * Time.deltaTime;
        tail.localScale = new Vector3(tail.localScale.x - step, tail.localScale.y, tail.localScale.z);

        return (tail.localScale.x <= TAIL_CUT);
    }


    public void UpdateTextMesh(int hitCount)
    {
        countText.text = hitCount.ToString();
    }
    #endregion
    #endregion
}
