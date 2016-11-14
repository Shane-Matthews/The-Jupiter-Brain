using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

    NotificationsManager Notifications = null;

    //component variables
    private CharacterController pController;
    private SpriteRenderer playerSprite;
    public GameObject bulletPrefab;
    private BulletManager bulletManager;
    private AudioSource fireSound;
    private AudioSource jumpSound1;
    private AudioSource jumpSound2;
    private AudioSource hurtSound;
    private AudioSource[] soundList;

    //movement variables
    private float speed = 10f;
    private float jumpSpeed = 10;
    private float gravity = 30.0f;
    private Vector2 moveDirection = Vector2.zero;

    //jump and shoot variables
    public int shotsInScene = 0;
    public int maxShots = 3;
    public int numJumps;
    public int maxJumps = 2;
    private bool canShoot;
    private bool canJump;

    public bool isGrounded;

    public int currentHealth;
    private int maxHealth = 100;
    Vector4 color;


    // Use this for initialization
    void Start () {
        pController = GetComponent<CharacterController>();
        playerSprite = GetComponent<SpriteRenderer>();
        soundList = GetComponents<AudioSource>();
        fireSound = soundList[0];
        jumpSound1 = soundList[1];
        jumpSound2 = soundList[2];
        hurtSound = soundList[3];
        currentHealth = maxHealth;
        Notifications = GameObject.FindWithTag("EventManager").GetComponent<NotificationsManager>();
        color = playerSprite.color;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        playerControls();
        flipSprite();
        Debug.DrawRay(transform.position, transform.right);
    }

    void playerControls()
    {
        if (pController.isGrounded)
        {
            isGrounded = true;
            numJumps = maxJumps;
            moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }
        else
        {
            isGrounded = false;
        }
        

            if (Input.GetButtonDown("Jump"))
            {
                if (numJumps > 0)
                {
                jumpSound1.Play();
                    moveDirection.y = jumpSpeed;
                    numJumps--;
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                shoot();
            }

            moveDirection.y -= gravity * Time.deltaTime;
            pController.Move(moveDirection * Time.deltaTime);
    }

    void flipSprite()
    {
        if (moveDirection.x > 0)
        {
            playerSprite.flipX = false;
        }
        if (moveDirection.x < 0)
        {
            playerSprite.flipX = true;
        }
    }
    
    void shoot()
    {
        if (shotsInScene < maxShots)
        {
            fireSound.Play();
            //Clone of the bullet
            GameObject clone;

            //spawning the bullet at position
            clone = (Instantiate(bulletPrefab, transform.position, transform.rotation)) as GameObject;
            shotsInScene++;
            if (!playerSprite.flipX)
            {
                clone.GetComponent<Rigidbody>().AddForce(1000, 0, 0);
            }
            else
            {
                clone.GetComponent<Rigidbody>().AddForce(-1000, 0, 0);
            }

            StartCoroutine(destroyBulletAfterTime(clone));
        }
    }

    //delete the bullets after a few seconds
    IEnumerator destroyBulletAfterTime(GameObject bullet)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(bullet);
        shotsInScene--;
    }

    public void gainHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void loseHealth(int amount)
    {
        hurtSound.Play();
        StartCoroutine(flashDamage());
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            death();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillBox") && Notifications != null)
        {
            Notifications.PostNotification(this, "playerDeath");
        }
    }

    public void death()
    {
        Notifications.PostNotification(this, "playerDeath");
    }

    IEnumerator flashDamage()
    {
        playerSprite.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSecondsRealtime(0.05f);
        playerSprite.color = new Color(color.x, color.y, color.z, color.w);
    }
}
