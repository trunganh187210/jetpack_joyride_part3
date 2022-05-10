using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouseController : MonoBehaviour
{

    public float jetpackForce = 75.0f;

    private Rigidbody2D playerRigidbody;

    public float ForwardMovementSpeed = 3.0f;

    public Transform groundCheckTransform; //tranform to check if the mouse is grounded

    private bool isGrounded;

    public LayerMask groundCheckLayerMask; //layer mask that defines the ground

    Animator mouseAnimator;

    public ParticleSystem jetpack;

    private bool isDead = false;

    private uint coin = 0;

    public Text coinCollectedLable;

    public Button restartButton;

    public AudioClip coinCollectSound;

    public AudioSource jetpackAudio;

    public AudioSource footstepAudio;

    public ParralaxScroll parralax;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        bool jetpackFire = Input.GetButton("Fire1");
        jetpackFire = jetpackFire && !isDead;

        if (jetpackFire)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }

        if (!isDead)
        {
            Vector2 newVelocity = playerRigidbody.velocity;
            newVelocity.x = ForwardMovementSpeed;
            playerRigidbody.velocity = newVelocity;
        }

        UpdateGroundedStatus();

        AdjustJetPack(jetpackFire);

        if(isDead && isGrounded)
        {
            restartButton.gameObject.SetActive(true);
        }

        AdjustFootstepsAndJetpackSound(jetpackFire);

        parralax.offset = transform.position.x;
    }

    void UpdateGroundedStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.2f, groundCheckLayerMask);
        //draw a cirle with the radius 0.2 at ground checking object, if it overlaps the objects
        //that has a layer that is groundCheckLayerMask then the mouse is grounded

        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

    void AdjustJetPack(bool jetpackFire)
    {
        var jetpackEmmision = jetpack.emission;
        if (jetpackFire)
        {
            jetpackEmmision.rateOverTime = 300.0f;
        }
        else
        {
            jetpackEmmision.rateOverTime = 75.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            CollectCoin(collision);
        }
        else
        {
            HitByLaser(collision);
        }
    }

    void HitByLaser(Collider2D laserCollider)
    {
        isDead = true;
        mouseAnimator.SetBool("isDead", true);
        if (isDead)
        {
            AudioSource laserZap = laserCollider.gameObject.GetComponent<AudioSource>();
            laserZap.Play();
        }
    }

    void CollectCoin(Collider2D coinCollider)
    {
        coin++;
        coinCollider.gameObject.SetActive(false);
        coinCollectedLable.text = coin.ToString();
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("RocketMouse");
    }

    void AdjustFootstepsAndJetpackSound(bool jetpackFire)
    {
        footstepAudio.enabled = !isDead && isGrounded;
        jetpackAudio.enabled = !isDead && !isGrounded;

        if (jetpackFire)
        {
            jetpackAudio.volume = 1.0f;
        }
        else
        {
            jetpackAudio.volume = 0.5f;
        }
    }
}
