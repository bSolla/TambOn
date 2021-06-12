using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenuOps : MonoBehaviour
{
    public void RefreshCharacter(string ch)
    {
        GameManager.instance.info.character = ch;
        GameManager.instance.characterMenu.SetActive(false);
        GameManager.instance.mutatorsMenu.SetActive(true);
        
        GameManager.instance.mutatorsMenu.GetComponent<MutatorMenuOps>().Init();

    }
}
