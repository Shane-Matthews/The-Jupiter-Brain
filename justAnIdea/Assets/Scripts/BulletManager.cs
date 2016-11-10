using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

    playerController pCont;
    Rigidbody bulletRB;
    BoxCollider bulletCollider;
    private EnemyManager healthManager;
    private AudioSource hitSound;

    int damage = 1;

    void Awake()
    {
        hitSound = GetComponent<AudioSource>();
        hitSound.enabled = true;
    }

	// Use this for initialization
	void Start () {
        //pCont = GameObject.FindWithTag("Player").GetComponent<playerController>();
        bulletRB = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        OnTriggerEnter(bulletCollider);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            //hitSound.Play();
            healthManager = collider.gameObject.GetComponent<EnemyManager>();
            Debug.Log("Bullet hit an enemy!");
            healthManager.takeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
