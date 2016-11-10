using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    ParticleSystem particles;
    AudioSource deathSound;

    int currentHealth;
    int maxHealth = 8;

    // Use this for initialization
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        deathSound = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
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
        StartCoroutine(deathDelay());
    }

    IEnumerator deathDelay()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(this.gameObject);
    }
}
