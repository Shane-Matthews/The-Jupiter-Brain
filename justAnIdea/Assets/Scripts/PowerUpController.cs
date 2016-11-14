using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

    private CharacterController pUpC;
    private playerController playerHealthRef;
    private BoxCollider pickupBox;
    private SpriteRenderer sprite;
    private AudioSource pickupSound;
    private Vector3 velocity;

    private int amountToRestore = 30;
    float alpha = 1;
    bool isFade = false;

    // Use this for initialization
    void Start () {
        velocity.y += 400.0f * Time.deltaTime;
        pUpC = GetComponent<CharacterController>();
        pickupBox = GetComponent<BoxCollider>();
        sprite = GetComponent<SpriteRenderer>();
        pickupSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        velocity.y -= 15.0f * Time.deltaTime;
        pUpC.Move(velocity  * Time.deltaTime);
        OnTriggerEnter(pickupBox);
        if (isFade)
        {
            alpha -= 0.07f;
            fadeAway();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerHealthRef = collider.gameObject.GetComponent<playerController>();
            playerHealthRef.gainHealth(amountToRestore);
            pickupSound.Play();
            StartCoroutine(destroyDelay());

        }
    }

    IEnumerator destroyDelay()
    {
        disableComponents();
        isFade = true;
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(this.gameObject);
    }

    void disableComponents()
    {
        pUpC.enabled = false;
        pickupBox.enabled = false;
    }

    void fadeAway()
    {
        sprite.color = new Color(1f, 1f, 1f, alpha);
    }
}
