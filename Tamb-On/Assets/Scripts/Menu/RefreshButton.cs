using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RefreshButton : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    bool switched = false;

    [SerializeField] TextMeshProUGUI mutatorName;
    [SerializeField] MutatorType mutatorType;

    [SerializeField] Image imageComponent;
    [SerializeField] Sprite[] mutatorSprites;

    // these are in the same order as the sprites in the sprite array, for easier accesss
    private enum MutatorIndex { Facil, Dificil, Blind, Combo, Precision, Mortal, Inmortal};
    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public enum MutatorType { MortalBaby, ComboPrecision, DificilFacil, Blind};
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---

    #endregion

    #region --- CUSTOM METHODS ---

    public void ProcessClick()
    {
        switch(mutatorType)
        {
            case MutatorType.MortalBaby:
                MortalBaby();
                break;
            case MutatorType.ComboPrecision:
                ComboPrecision();
                break;
            case MutatorType.DificilFacil:
                DificilFacil();
                break;
            case MutatorType.Blind:
                Blind();
                break;
            default:
                break;
        }
    }

    void MortalBaby()
    {
        if (!switched)
        {
            mutatorName.text = MutatorIndex.Inmortal.ToString();
            imageComponent.sprite = mutatorSprites[(int)MutatorIndex.Inmortal];
            switched = !switched;
        }
        else
        {
            mutatorName.text = MutatorIndex.Mortal.ToString();
            imageComponent.sprite = mutatorSprites[(int)MutatorIndex.Mortal];
            switched = !switched;
        }
    }

    void ComboPrecision()
    {
        //if (GameManager.instance.info.character != "Fantasma")
        //{
            if (!switched)
            {
                mutatorName.text = MutatorIndex.Precision.ToString();
                imageComponent.sprite = mutatorSprites[(int)MutatorIndex.Precision];
                switched = !switched;
            }
            else
            {
                mutatorName.text = MutatorIndex.Combo.ToString();
                imageComponent.sprite = mutatorSprites[(int)MutatorIndex.Combo];
                switched = !switched;
            }
        /*}
        else
        {
            mutatorName.text = MutatorIndex.Combo.ToString();
            imageComponent.sprite = mutatorSprites[(int)MutatorIndex.Combo];
            switched = false;
        }*/
    }

    void DificilFacil()
    {
        if (!switched)
        {
            mutatorName.text = MutatorIndex.Dificil.ToString();
            imageComponent.sprite = mutatorSprites[(int)MutatorIndex.Dificil];
            switched = !switched;
        }
        else
        {
            mutatorName.text = MutatorIndex.Facil.ToString();
            imageComponent.sprite = mutatorSprites[(int)MutatorIndex.Facil];
            switched = !switched;
        }
    }

    void Blind()
    {
        if (GameManager.instance.info.mutators.Contains("Ciego"))
        {
            gameObject.GetComponentInChildren<Text>().text = "Activado";
        }
        else
        {
            gameObject.GetComponentInChildren<Text>().text = "Desactivado";
        }
    }
    #endregion
    #endregion

}
