using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifetime : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    const float CUT_TIME = 0.5f;
    #endregion
    #region --- PROTECTED ---

    #endregion
    #region --- PUBLIC ---
    public ParticleSystem particles;
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (particles.isPlaying && particles.time >= CUT_TIME)
        {
            particles.Stop();
        }
        else if (!particles.IsAlive())
        {
            Destroy(gameObject);
        }
        
    }
    #endregion

    #region --- CUSTOM METHODS ---

    #endregion
    #endregion
}
