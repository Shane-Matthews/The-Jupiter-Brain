using UnityEngine;
using System.Collections;

public class shootEnemyBehavior : MonoBehaviour {

    public GameObject bulletPrefab;
    private EnemyController eController;
    private EnemyManager eManager;
    private AudioSource fireSound;

    private AudioSource[] soundList;

    public int shotsInScene = 0;
    public int maxShots = 1;

    float shotDelayTime = 1.0f;


    bool isAlive;

    // Use this for initialization
    void Start () {
        eController = GetComponent<EnemyController>();
        eManager = GetComponent<EnemyManager>();
        soundList = GetComponents <AudioSource>();
        fireSound = soundList[1];
        isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {
        checkToFire();
	}

    void checkToFire()
    {
        if (eManager.currentHealth <= 0)
        {
            isAlive = false;
        }

        if (isAlive)
        {
            if (eController.getDistanceFromPlayer() < 15.0f)
            {
                shoot();
            }
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
            if (!eController.spriteRenderer.flipX)
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
        yield return new WaitForSecondsRealtime(1.3f);
        Destroy(bullet);
        shotsInScene--;
    }
}
