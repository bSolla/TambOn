using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTaiko : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    private Color RED = new Color32((byte)255, (byte)139, (byte)148, 255);
    private Color GREEN = new Color32((byte)63, (byte)176, (byte)60, 255);
    private Color BLUE = new Color32((byte)16, (byte)174, (byte)186, 255);
    #endregion
    #region --- PROTECTED ---
    #endregion
    #region --- PUBLIC ---
    public enum AreaType
    {
        INNER_R,
        INNER_L,
        OUTER_R,
        OUTER_L
    }
    public AreaType areaType;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            ReadMouseClick();
    }
    #endregion

    #region --- CUSTOM METHODS ---
    void ReadMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            CallInputManager();
        }

    }

    public void CallInputManager()
    {
        switch (areaType)
        {
            case AreaType.INNER_R:
                if (!GameManager.instance.inputManager.CheckInnerRightTaiko() /*&& !CheckInnerSustain(true)*/ && !GameManager.instance.inputManager.CheckInnerTaiko())
                {
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                    {
                        GameManager.instance.scoreManager.IncreaseFail("innerRight");
                    }
                    else
                    {
                        GameManager.instance.scoreManager.IncreaseFail("inner");
                    }
                    GameManager.instance.inputManager.SetHitAnimation("FALLO", RED);

                }
                //GameManager.instance.inputManager.CheckInnerRightTaiko();
                //Debug.Log("inner right -- not implemented yet");
                break;
            case AreaType.INNER_L:
                if (!GameManager.instance.inputManager.CheckInnerLeftTaiko() /*&& !CheckInnerSustain(true)*/ && !GameManager.instance.inputManager.CheckInnerTaiko())
                {
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                    {
                        GameManager.instance.scoreManager.IncreaseFail("innerLeft");
                    }
                    else
                    {
                        GameManager.instance.scoreManager.IncreaseFail("inner");
                    }
                    GameManager.instance.inputManager.SetHitAnimation("FALLO", RED);
                }
                //GameManager.instance.inputManager.CheckInnerLeftTaiko();
                //Debug.Log("inner left -- not implemented yet");
                break;
            case AreaType.OUTER_R:
                if (!GameManager.instance.inputManager.CheckOuterRightTaiko() /*&& !CheckOuterSustain(true)*/ && !GameManager.instance.inputManager.CheckOuterTaiko())
                {
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                    {
                        GameManager.instance.scoreManager.IncreaseFail("outerRight");
                    }
                    else
                    {
                        GameManager.instance.scoreManager.IncreaseFail("outer");
                    }
                   GameManager.instance.inputManager.SetHitAnimation("FALLO", RED);

                }
                //GameManager.instance.inputManager.CheckOuterRightTaiko();
                //Debug.Log("outer right -- not implemented yet");
                break;
            case AreaType.OUTER_L:
                if (!GameManager.instance.inputManager.CheckOuterLeftTaiko() /*&& !CheckOuterSustain(true)*/ && !GameManager.instance.inputManager.CheckOuterTaiko())
                {
                    if (GameManager.instance.info.mutators.Contains("Dificil"))
                    {
                        GameManager.instance.scoreManager.IncreaseFail("outerLeft");
                    }
                    else
                    {
                        GameManager.instance.scoreManager.IncreaseFail("outer");
                    }
                    GameManager.instance.inputManager.SetHitAnimation("FALLO", RED);

                }
                //GameManager.instance.inputManager.CheckOuterLeftTaiko();
                //Debug.Log("outer left -- not implemented yet");
                break;
            default:
                break;
        }
    }

    #endregion
    #endregion
};
