using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class bossEnemyScript : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject powerShotPrefab;
    private EnemyController eController;
    private EnemyManager eManager;
    SoundManager soundManager;

    public int shotsInScene = 0;
    private int maxShots = 6;

    float shotDelayTime;
    float powerShotDelayTime;
    float maxTime = 0.7f;
    float maxPowerTime = 1.4f;

    bool isAlive;

    public List<GameObject> clones = new List<GameObject>();

    bool canAttack;
    bool canShoot;
    bool canShootAlt;
    bool canCharge;
    bool canAltShoot;

    float attackTime = 10.0f;
    float currentAttackTime;

    // Use this for initialization
    void Start () {
        eController = GetComponent<EnemyController>();
        eManager = GetComponent<EnemyManager>();
        soundManager = SoundManager.Instance;
        isAlive = true;
        canAttack = false;
        canShoot = false;
        canShootAlt = false;
        canCharge = false;
        canAltShoot = false;
        shotDelayTime = maxTime;
        powerShotDelayTime = maxPowerTime;
        currentAttackTime = attackTime;

        eManager.maxHealth = 50;
        eManager.currentHealth = eManager.maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        if (isAlive && eController.getDistanceFromPlayer() < 15.0f)
        {
            determineAttack();
        }

        checkToFire();

        shotDelayTime -= Time.deltaTime;
        powerShotDelayTime -= Time.deltaTime;
        currentAttackTime -= Time.deltaTime;
        if (currentAttackTime <= 0)
        {
            canAttack = true;
            currentAttackTime = attackTime;
        }
	}

    void checkToFire()
    {
        if (eManager.currentHealth <= 0)
        {
            isAlive = false;
        }

        if (isAlive)
        {
            if (eController.getDistanceFromPlayer() < 15.0f && canShoot)
            {
                shoot();
            }
            else if (eController.getDistanceFromPlayer() < 15.0f && canShootAlt)
            {
                powerShot();
            }
        }
    }

    void shoot()
    {
        if (shotsInScene < maxShots && shotDelayTime <= 0 && canShoot)
        {
            shotDelayTime = maxTime;
            soundManager.PlaySound(8);
            //Clone of the bullet
            GameObject clone;
            //spawning the bullet at position
            clone = (Instantiate(powerShotPrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation)) as GameObject;
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
    }

    IEnumerator destroyPowerBulletAfterTime(GameObject bullet)
    {
        yield return new WaitForSecondsRealtime(10f);
        Destroy(bullet);
    }

    void powerShot()
    {
        if (shotsInScene < maxShots - 3 && powerShotDelayTime <= 0 && canShootAlt)
        {
            powerShotDelayTime = maxPowerTime;
            soundManager.PlaySound(8);
            //Clone of the bullet
            GameObject clone;
            //spawning the bullet at position
            clone = (Instantiate(powerShotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation)) as GameObject;
            shotsInScene++;
            if (!eController.spriteRenderer.flipX)
            {
                clone.GetComponent<Rigidbody>().AddForce(350, 0, 0);
            }
            else
            {
                clone.GetComponent<Rigidbody>().AddForce(-350, 0, 0);
            }

            StartCoroutine(destroyPowerBulletAfterTime(clone));
        }
    }

    IEnumerator chargeAttack()
    {
        float tmpSpeed = eController.speed;
        eController.speed *= 4;
        yield return new WaitForSecondsRealtime(5.0f);
        eController.speed = tmpSpeed;
    }

    void determineAttack()
    {
        if (canAttack)
        {
            float random = eManager.generateNumber();

            if (random <= 0.33 && random >= 0)
            {
                //perform basic shoot attack
                canShoot = true;
                if (shotsInScene == maxShots)
                {
                    canShoot = false;
                    shotsInScene = 0;
                    canAttack = false;
                }
            }
            else if (random >= 0.34 && random <= 0.66)
            {
                //perform charge attack
                StartCoroutine(chargeAttack());
            }
            else
            {
                //perform power shot attack
                canShootAlt = true;
                if (shotsInScene == maxShots)
                {
                    canShootAlt = false;
                    shotsInScene = 0;
                }
            }
        }
        canAttack = false;
    }
}
