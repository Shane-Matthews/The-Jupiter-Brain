using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

    NotificationsManager Notifications = null;

    //component variables
    private CharacterController pController;
    private SpriteRenderer playerSprite;
    public GameObject bulletPrefab;
    private BulletManager bulletManager;
    SoundManager soundManager;
    private GameObject[] enemies;
    private healthBar hBar;

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

    private bool canBeHit;

    public bool isGrounded;

    private float hurtForce = 3.0f;

    public int currentHealth;
    private int maxHealth = 100;
    Vector4 color;


    // Use this for initialization
    void Start () {
        pController = GetComponent<CharacterController>();
        playerSprite = GetComponent<SpriteRenderer>();
        soundManager = SoundManager.Instance;
        currentHealth = maxHealth;
        Notifications = GameObject.FindWithTag("EventManager").GetComponent<NotificationsManager>();
        hBar = GetComponent<healthBar>();
        color = playerSprite.color;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        canBeHit = true;

        foreach(GameObject enemy in enemies)
        {
            Physics.IgnoreCollision(enemy.GetComponent<CharacterController>(), pController);
        }
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
            moveDirection.x = Input.GetAxis("Horizontal") * speed / 1.5f;
        }


        if (Input.GetButtonDown("Jump"))
        {
            if (numJumps > 0)
            {
                if (numJumps > 1)
                {
                    soundManager.PlaySound(5);
                }
                else
                {
                    soundManager.PlaySound(6);
                }
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
            soundManager.PlaySound(7);
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
        float barAmount = amount;
        currentHealth += amount;
        hBar.Add(barAmount);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void loseHealth(int amount)
    {
        if (canBeHit)
        {
            soundManager.PlaySound(4);
            StartCoroutine(flashDamage());
            float barAmount = amount;
            currentHealth -= amount;
            hBar.Subtract(barAmount);
            moveDirection.x -= hurtForce * Time.deltaTime;
            pController.Move(moveDirection * Time.deltaTime);

            if (currentHealth <= 0)
            {
                death();
            }
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
        StartCoroutine(invincibility());
        while (!canBeHit)
        {
            if (color.x < 1)
            {
                playerSprite.color = new Color(color.x++, color.y, color.z, color.w);
            }
            else if (color.x > 0)
            {
                playerSprite.color = new Color(color.x--, color.y, color.z, color.w);
            }
            yield return new WaitForSecondsRealtime(0.1f);
            playerSprite.color = new Color(color.x, color.y, color.z, color.w);
        }
    }

    IEnumerator invincibility()
    {
        canBeHit = false;
        yield return new WaitForSecondsRealtime(1.1f);
        canBeHit = true;
    }
}
