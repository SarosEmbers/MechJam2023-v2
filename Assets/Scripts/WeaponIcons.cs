using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIcons : MonoBehaviour
{
    public GameObject LSniper;
    public GameObject RSniper;
    public GameObject LRocket;
    public GameObject RRocket;
    public GameObject LPlasma;
    public GameObject RPlasma;


    playerAimAttack playerAttack;
    ScroungeManager sm;
    // Start is called before the first frame update
    void Start()
    {
        playerAttack = playerAttack.GetComponent<playerAimAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}
