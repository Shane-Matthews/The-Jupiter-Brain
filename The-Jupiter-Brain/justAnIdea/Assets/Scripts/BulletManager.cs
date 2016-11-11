using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

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
        bulletCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        OnTriggerEnter(bulletCollider);
        Debug.Log(hitSound.enabled);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log(hitSound.enabled);
            hitSound.Play();
            healthManager = collider.gameObject.GetComponent<EnemyManager>();
            Debug.Log("Bullet hit an enemy!");
            healthManager.takeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
