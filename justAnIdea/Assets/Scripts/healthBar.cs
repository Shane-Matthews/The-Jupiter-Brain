using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {

    public Scrollbar hBar;

    public float health = 100f;

    public void Subtract(float value)
    {
        health -= value;
        hBar.size = health / 100f;
    }

    public void Add(float value)
    {
        health += value;
        if (health > 100)
        {
            health = 100;
        }
        hBar.size = health / 100f;
    }
}
