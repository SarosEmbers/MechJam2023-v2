using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthHUD : MonoBehaviour
{

    //Referencing For Health Grid on HUD (Without Using Arrays or Lists, may want to fix this later)
    public Image[] torso, leftarm, leftleg, rightarm, rightleg;
    /* public Image healthTorso;
     public Image healthLeftArm;
     public Image healthLeftLeg;
     public Image healthRightArm;
     public Image healthRightLeg;*/

    private GameObject player;
    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.health >= playerHealth.maxHealth)
        {

        }
    }
}
