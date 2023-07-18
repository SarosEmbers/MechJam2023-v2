using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private static float health = 100;
    public float maxHealth = 100;

    public GameObject healthBar;
    AudioSource audioSource;

    //Gamecontroller gameController;
    //gm

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercent = health / maxHealth;

        healthBar.transform.localScale = new Vector3(healthPercent, 1, 1);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        UpdateHealthBar();

        if (health <= 0)
        {
            OnHealthDepleted();
        }

        audioSource.Play();
    }

    public void SetHealth(float healAmount)
    {
        health = healAmount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdateHealthBar();
    }

    public float GetHealth()
    {
        return health;
    }

    private void OnHealthDepleted()
    {
        //gameController = GameObject.Find("Game Controller").GetComponent<Gamecontroller>();
        //gameController.OnDeath();
    }
}
