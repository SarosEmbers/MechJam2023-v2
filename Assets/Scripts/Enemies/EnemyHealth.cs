using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    public float emmissionRate = 100;

    //public GameObject healthBar;
    //public GameObject wholeHealthMeter;
    public ParticleSystem enemHealthIndic;
    Transform player;
    //Transform canvas;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //canvas = healthBar.transform.parent.parent;

        health = maxHealth;
        UpdateHealthBar();

        //wholeHealthMeter.SetActive(false);
    }

    private void Update()
    {
        //Vector3 lookDirection = player.position - canvas.position;
        //lookDirection.y = 0;
        //canvas.rotation = Quaternion.LookRotation(lookDirection);
    }

    private void UpdateHealthBar()
    {
        if (health < maxHealth)
        {
            enemHealthIndic.gameObject.SetActive(true);
        }
        float healthPercent = health / maxHealth;
        float inverseHealth = (1 - healthPercent) * 100;

        var damagePEmission = enemHealthIndic.emission;
        damagePEmission.rateOverTime = emmissionRate;
        damagePEmission.rateOverTimeMultiplier = inverseHealth;
        Debug.Log("ENEMY HEALTH: " + damagePEmission.rateOverTime + " || " + inverseHealth);
        //healthBar.transform.localScale = new Vector3(healthPercent, 1, 1);
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
        //Destroy(this.gameObject);
        Debug.Log("ENEMY DEAD: " + this.gameObject.name);
    }
}
