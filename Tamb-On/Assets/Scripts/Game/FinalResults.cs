using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalResults : MonoBehaviour
{
    public GameObject score;
    public GameObject record;
    public GameObject perfect;
    public GameObject good;
    public GameObject fail;
    public GameObject character;
    public GameObject slider;
    public GameObject percentage;

    public Sprite floratoria;
    public Sprite topita;
    public Sprite mrpapa;
    public Sprite silvestre;
    public Sprite caxper;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    { 
        score.GetComponent<Text>().text = "Puntuacion: " + Math.Floor(GameManager.instance.scoreManager.score);
        record.GetComponent<Text>().text = newRecord(GameManager.instance.scoreManager.worldRecord,GameManager.instance.scoreManager.personalRecord);
        perfect.GetComponent<Text>().text = "Perfecto: " + GameManager.instance.scoreManager.totalPerfect;
        good.GetComponent<Text>().text = "Bien: " + GameManager.instance.scoreManager.totalGood;
        fail.GetComponent<Text>().text = "Fallo: " + GameManager.instance.scoreManager.totalFail;
        character.GetComponent<Image>().sprite = StringToSprite(GameManager.instance.info.character);
        slider.GetComponent<Slider>().value = (float)(GameManager.instance.scoreManager.totalGood + GameManager.instance.scoreManager.totalPerfect) / (float)GameManager.instance.rhythmManager.totalNotes;
        percentage.GetComponent<Text>().text = Math.Floor(((float)(GameManager.instance.scoreManager.totalGood + GameManager.instance.scoreManager.totalPerfect) / (float)GameManager.instance.rhythmManager.totalNotes) * 100) + "%";
    }

    private Sprite StringToSprite(string ch)
    {
        switch (ch)
        {
            case "Topita": return topita;
            case "Silvestre": return silvestre;
            case "Mr Papa": return mrpapa;
            case "Caxper": return caxper;
            case "Floratoria": return floratoria;

        }
        return null;
    }


    private string newRecord(bool world, bool personal)
    {
        if (world)
        {
            return "Nuevo record mundial";
        }
        else if (personal)
        {
            return "Nuevo record personal";
        }
        else return "";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
