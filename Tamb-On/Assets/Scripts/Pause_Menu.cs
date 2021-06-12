using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void backToMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
        GameManager.instance.CameFromPlayScene();
    }
    public void restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void resume(GameObject pauseButton)
    {
        GameManager.instance.rhythmManager.SetPause(false);
        this.gameObject.SetActive(false);
        pauseButton.SetActive(true);
    }
    public void pause(GameObject pauseButton)
    {
        GameManager.instance.rhythmManager.SetPause(true);
        pauseButton.SetActive(false);

    }
}
