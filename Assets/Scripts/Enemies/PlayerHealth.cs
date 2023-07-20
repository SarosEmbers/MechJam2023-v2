using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;
    public float maxHealth = 100;

    public GameManager gm;

    //public GameObject healthBar;
    AudioSource audioSource;
    public UIHandler UI;

    //Gamecontroller gameController;
    //gm

   

    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();

        audioSource = GetComponent<AudioSource>();
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float healthPercent = health / maxHealth;
        UI.UpdateHealthImg(healthPercent);
        //healthBar.transform.localScale = new Vector3(healthPercent, 1, 1);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        UpdateHealthBar();

        Debug.Log("PLAYER HEALTH: " + health);

        if (health <= 0)
        {
            OnHealthDepleted();
        }

        //audioSource.Play();
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
        Debug.Log("PLAYER DEAD");

        Debug.LogWarning("Player is dead");
        AudioManager._Instance.Play("MechDestroyed");
        Destroy(this.gameObject);

        //gameController = GameObject.Find("Game Controller").GetComponent<Gamecontroller>();
        //gameController.OnDeath();
    }
}
