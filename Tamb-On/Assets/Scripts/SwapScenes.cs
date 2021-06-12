using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapScenes : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject singleButton;
    void Start()
    {
        if (GameManager.instance.CameFromPlay)
        {
            singleButton.GetComponent<Button>().onClick.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
