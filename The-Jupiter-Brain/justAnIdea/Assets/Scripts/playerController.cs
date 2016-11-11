using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

    NotificationsManager Notifications = null;

    //component variables
    private CharacterController pController;
    private SpriteRenderer playerSprite;
    public GameObject bulletPrefab;
    private GameObject bulletSpawn;
    private BulletManager bulletManager;
    private AudioSource fireSound;
    private AudioSource jumpSound1;
    private AudioSource jumpSound2;
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

    // Use this for initialization
    void Start () {
        pController = GetComponent<CharacterController>();
        playerSprite = GetComponent<SpriteRenderer>();
        bulletSpawn = GameObject.FindWithTag("bulletSpawn");
        soundList = GetComponents<AudioSource>();
        fireSound = soundList[0];
        jumpSound1 = soundList[1];
        jumpSound2 = soundList[2];
        Notifications = GameObject.FindWithTag("EventManager").GetComponent<NotificationsManager>();
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
            clone = (Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation)) as GameObject;
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
        yield return new WaitForSecondsRealtime(0.7f);
        Destroy(bullet);
        shotsInScene--;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillBox") && Notifications != null)
        {
            Notifications.PostNotification(this, "KillBarrierDeath");
        }
    }

    public Vector2 getMoveDirection()
    {
        return moveDirection;
    }
}
