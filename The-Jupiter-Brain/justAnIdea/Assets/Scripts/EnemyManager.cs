using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    CharacterController eController;
    BoxCollider eBox;
    EnemyController cEnemy;
    SpriteRenderer eSprite;
    ParticleSystem particles;
    AudioSource deathSound;

    int currentHealth;
    int maxHealth = 8;

    bool isFade;
    float alpha;

    // Use this for initialization
    void Start()
    {
        eController = GetComponent<CharacterController>();
        eBox = GetComponent<BoxCollider>();
        cEnemy = GetComponent<EnemyController>();
        particles = GetComponent<ParticleSystem>();
        deathSound = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        isFade = false;
        alpha = 1;
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
        Debug.Log("Enemy's health is now: " + currentHealth);
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
        cEnemy.spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
    }
}
