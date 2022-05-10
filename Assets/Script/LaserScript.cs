using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public Sprite laserOnSprite;
    public Sprite laserOffSprite;

    public float toggleInterval = 0.5f;
    public float rotationSpeed = 0.0f;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;

    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;

    // Start is called before the first frame update
    void Start()
    {
        timeUntilNextToggle = toggleInterval;

        laserCollider = GetComponent<Collider2D>();
        laserRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilNextToggle -= Time.deltaTime;

        if(timeUntilNextToggle < 0)
        {
            isLaserOn =! isLaserOn;//switch state of the laser
            laserCollider.enabled = isLaserOn; //switch state of the laser's collider

            if (isLaserOn)
            {
                laserRenderer.sprite = laserOnSprite;
            }
            else
            {
                laserRenderer.sprite = laserOffSprite;
            }
            //switch sprite of the laser

            timeUntilNextToggle = toggleInterval; //reset toggle time
        }

        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
