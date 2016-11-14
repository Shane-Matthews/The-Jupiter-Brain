using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

    BoxCollider bulletCollider;
    private EnemyManager healthManager;
    private playerController playerHealthManager;
    private AudioSource hitSound;

    int damageToEnemy = 1;
    int damageToPlayer = 5;

    void Awake()
    {
        hitSound = GetComponent<AudioSource>();
        hitSound.enabled = true;
    }

	// Use this for initialization
	void Start () {
        bulletCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        OnTriggerEnter(bulletCollider);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" && this.gameObject.tag == "playerBullet")
        {
            //hitSound.Play();
            healthManager = collider.gameObject.GetComponent<EnemyManager>();
            healthManager.takeDamage(damageToEnemy);
            Destroy(this.gameObject);
        }
        else if (collider.gameObject.tag == "Player" && this.gameObject.tag == "enemyBullet")
        {
            playerHealthManager = collider.gameObject.GetComponent<playerController>();
            playerHealthManager.loseHealth(damageToPlayer);
            Destroy(this.gameObject);
        }
    }
}
