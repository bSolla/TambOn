using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseButtonBehavior : MonoBehaviour
{
    #region ---- VARIABLES ----

    #region --- PRIVATE ---
    private SpriteRenderer rend;
    private Color transparent;

    private bool isPaused = false;
    #endregion
    #region --- PROTECTED ---
    protected RhythmManager rhythmManager;
    protected InputManager inputManager;

    protected const float LEFT_TRANSLATION = -0.1f;
    protected Vector3 speedDirection = new Vector3(-1f, 0f, 0f);
    protected float speed;

    protected Vector3 originPoint, destinationPoint;

    [SerializeField] protected ParticleLifetime stars;
    #endregion
    #region --- PUBLIC ---
    #endregion

    #endregion


    #region ---- METHODS ----
    #region --- UNITY METHODS ---
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region --- CUSTOM METHODS ---
    protected void Initialize()
    {
        rend = GetComponent<SpriteRenderer>();

        foreach (string mut in GameManager.instance.info.mutators)
        {
            if (mut == "Ciego")
            {
                transparent = rend.color;
                transparent.a = 0F;
                rend.color = transparent;
            }
        }

        rhythmManager = GameManager.instance.rhythmManager;
        inputManager = GameManager.instance.inputManager;

        originPoint = GameObject.FindGameObjectWithTag("Spawner").transform.position;
        destinationPoint = GameObject.FindGameObjectWithTag("Perfect").transform.position;

        float timeToReachGoal = rhythmManager.timeToReachGoal;

        speed = (Vector2.Distance(destinationPoint, originPoint) / timeToReachGoal);
    }

    protected void MoveLeft()
    {
        if (!isPaused)
        {
            float step = speed * Time.deltaTime;
            transform.position = new Vector3(
            transform.position.x + speedDirection.x * step,
            transform.position.y + speedDirection.y * step,
            transform.position.z);

            if (transform.position.x <= -2 && rend.color.a < 1F)
            {
                transparent = rend.color;
                transparent.a += 0.05F;
                if (transparent.a > 1F) transparent.a = 1F;
                rend.color = transparent;
            }
        }
    }

    public void DestroyButton()
    {
        Instantiate(stars, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity);
        Destroy(gameObject);
    }


    public void SetPause(bool pause)
    {
        isPaused = pause;
    }
    #endregion
    #endregion
}
