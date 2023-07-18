using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    float health;
    public float maxHealth = 100;

    public GameObject healthBar;
    public GameObject wholeHealthMeter;
    Transform player;
    Transform canvas;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = healthBar.transform.parent.parent;

        health = maxHealth;
        UpdateHealthBar();

        wholeHealthMeter.SetActive(false);
    }

    private void Update()
    {
        Vector3 lookDirection = player.position - canvas.position;
        lookDirection.y = 0;
        canvas.rotation = Quaternion.LookRotation(lookDirection);
    }

    private void UpdateHealthBar()
    {
        if (health < maxHealth)
        {
            wholeHealthMeter.SetActive(true);
        }

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
        Destroy(this.gameObject);
    }
}
