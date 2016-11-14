using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    CharacterController eController;
    BoxCollider eBox;
    EnemyController cEnemy;
    public GameObject healthPowerupPrefab;
    ParticleSystem particles;
    AudioSource deathSound;
    public SpriteRenderer spriteRenderer;

    public int currentHealth;
    int maxHealth = 8;

    Vector4 color;

    bool isFade;
    float alpha;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        eController = GetComponent<CharacterController>();
        eBox = GetComponent<BoxCollider>();
        cEnemy = GetComponent<EnemyController>();
        particles = GetComponent<ParticleSystem>();
        deathSound = GetComponent<AudioSource>(); 
        currentHealth = maxHealth;
        isFade = false;
        alpha = 1;
        color = spriteRenderer.color;
    }

    void Update()
    {
        if (isFade)
        {
            alpha -= 0.01f;
            fadeAway();
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(flashDamage());
        //Debug.Log("Enemy's health is now: " + currentHealth);
        if (currentHealth == 0)
        {
            death();
        }
    }

    void death()
    {
        deathSound.Play();
        particles.Play();
        fadeAway();
        StartCoroutine(deathDelay());
    }

    IEnumerator deathDelay()
    {
        disableComponents();
        isFade = true;
        yield return new WaitForSecondsRealtime(1.5f);
        dropPowerup();
        Destroy(this.gameObject);
    }

    void fadeAway()
    {
        cEnemy.spriteRenderer.color = new Color(0f, 0f, 1f, alpha);
    }

    //simple method that will disable components of the enemy before death, for proper aniimation
    void disableComponents()
    {
        eController.enabled = false;
        eBox.enabled = false;
    }

    IEnumerator flashDamage()
    {
        cEnemy.spriteRenderer.color = new Color(0f, 0f, 1f, 1f);
        yield return new WaitForSecondsRealtime(0.05f);
        cEnemy.spriteRenderer.color = new Color(color.x, color.y, color.z, color.w);
    }

    //generates a number between 0 and 1
    float generateNumber()
    {
        float randomNumber = Random.value;
        return randomNumber;
    }

    void dropPowerup()
    {
        //check chance to drop
        if (generateNumber() < 0.4f)
        {
            //drop health
            if (generateNumber() < 0.5f)
            {

                //Clone of the bullet
                GameObject clone;

                //spawning the bullet at position
                clone = (Instantiate(healthPowerupPrefab, transform.position, transform.rotation)) as GameObject;
            }
            //drop ammo (NOT YET IMPLIMENTED)
            else
            {

            }
        }
    }
}
