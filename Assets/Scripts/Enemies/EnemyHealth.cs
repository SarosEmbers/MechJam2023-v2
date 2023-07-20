using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    public float emmissionRate = 100;
    private EnemyFireAt efa;
    public GameObject arm_beefy, arm_speedy, arm_hover, legs_beefy, legs_speedy, legs_hover;
    private Transform storedPos;
    //public GameObject healthBar;
    //public GameObject wholeHealthMeter;
    public ParticleSystem enemHealthIndic, deathBoom;
    Transform player;
    //Transform canvas;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //canvas = healthBar.transform.parent.parent;

        health = maxHealth;
        UpdateHealthBar();

        efa = GetComponent<EnemyFireAt>();
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
        //Debug.Log("ENEMY HEALTH: " + damagePEmission.rateOverTime + " || " + inverseHealth);
        //healthBar.transform.localScale = new Vector3(healthPercent, 1, 1);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        UpdateHealthBar();

        if (health <= 0)
        {
            float isArm = Random.Range(0, 2);
            Debug.Log("ENEMY DEAD: " + this.gameObject.name + isArm);
            if (isArm < 3)
            {
                switch (efa.enemyBotType)
                {
                    case EnemyFireAt.BotTypes.Beefy:
                        Instantiate(arm_beefy, efa.gameObject.transform.position, Quaternion.identity);
                        break;
                    case EnemyFireAt.BotTypes.Speedy:
                        Instantiate(arm_speedy, efa.gameObject.transform.position, Quaternion.identity);
                        break;
                    case EnemyFireAt.BotTypes.Hover:
                        Instantiate(arm_hover, efa.gameObject.transform.position, Quaternion.identity);
                        break;
                }
            }
            else if (isArm == 2)
            {
                switch (efa.enemyBotType)
                {
                    case EnemyFireAt.BotTypes.Beefy:
                        Instantiate(legs_beefy, efa.gameObject.transform.position, Quaternion.identity);
                        break;
                    case EnemyFireAt.BotTypes.Speedy:
                        Instantiate(legs_speedy, efa.gameObject.transform.position, Quaternion.identity);
                        break;
                    case EnemyFireAt.BotTypes.Hover:
                        Instantiate(legs_hover, efa.gameObject.transform.position, Quaternion.identity);
                        break;
                }
            }
            Invoke("OnHealthDepleted", .3f);
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
        AudioManager._Instance.PlayRandPitch("MechDestroyed", 0.85f, 1.25f);
        Destroy(this.gameObject);
    }
}
