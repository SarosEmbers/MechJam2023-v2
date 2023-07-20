using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth ph;
    private playerAimAttack paa;

    [Header("Equipment UI")]
    public GameObject placeholder;
    public GameObject Left_Equip, Right_Equip;
    public Image LSniper, LHover, LBeef, RSniper, RHover, RBeef;
    [Header("Health UI")]
    public GameObject placeholder2;
    public GameObject[] healthUIElements;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ph = player.GetComponent<PlayerHealth>();
        paa = player.GetComponent<playerAimAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (paa.LeftEquppedArm)
        {
            case playerAimAttack.StolenArms.Player36:
                LSniper.gameObject.SetActive(false);
                LHover.gameObject.SetActive(false);
                LBeef.gameObject.SetActive(false);
                break;
            case playerAimAttack.StolenArms.Beefy:
                LSniper.gameObject.SetActive(false);
                LHover.gameObject.SetActive(false);
                LBeef.gameObject.SetActive(true);
                break;
            case playerAimAttack.StolenArms.Speedy:
                LSniper.gameObject.SetActive(true);
                LHover.gameObject.SetActive(false);
                LBeef.gameObject.SetActive(false);
                break;
            case playerAimAttack.StolenArms.Hover:
                LSniper.gameObject.SetActive(false);
                LHover.gameObject.SetActive(true);
                LBeef.gameObject.SetActive(false);
                break;
        }

        switch (paa.RightEquppedArm)
        {
            case playerAimAttack.StolenArms.Player36:
                RSniper.gameObject.SetActive(false);
                RHover.gameObject.SetActive(false);
                RBeef.gameObject.SetActive(false);
                break;
            case playerAimAttack.StolenArms.Beefy:
                RSniper.gameObject.SetActive(false);
                RHover.gameObject.SetActive(false);
                RBeef.gameObject.SetActive(true);
                break;
            case playerAimAttack.StolenArms.Speedy:
                RSniper.gameObject.SetActive(true);
                RHover.gameObject.SetActive(false);
                RBeef.gameObject.SetActive(false);
                break;
            case playerAimAttack.StolenArms.Hover:
                RSniper.gameObject.SetActive(false);
                RHover.gameObject.SetActive(true);
                RBeef.gameObject.SetActive(false);
                break;
        }

        //float healthPercent = ph.health / ph.maxHealth;
        //float inverseHealth = (1 - healthPercent) * 100;
    }

    public void UpdateHealthImg(float healthPercent)
    {
        if (healthPercent <= .9f)
        {
            healthUIElements[0].SetActive(true);
        }
        if (healthPercent <= .8f)
        {
            healthUIElements[1].SetActive(true);
            healthUIElements[0].SetActive(true);
        }
        if (healthPercent <= .7f)
        {
            healthUIElements[2].SetActive(true);
            healthUIElements[1].SetActive(true);
            healthUIElements[0].SetActive(true);

        }
        if (healthPercent <= .6f)
        {
            healthUIElements[3].SetActive(true);
            healthUIElements[2].SetActive(true);
            healthUIElements[1].SetActive(true);
            healthUIElements[0].SetActive(true);

        }
        if (healthPercent <= .5f)
        {
            healthUIElements[5].SetActive(true);
            healthUIElements[4].SetActive(true);
            healthUIElements[3].SetActive(true);
            healthUIElements[2].SetActive(true);
            healthUIElements[1].SetActive(true);
            healthUIElements[0].SetActive(true);

        }
        if (healthPercent <= .4f)
        {
            healthUIElements[6].SetActive(true);
            healthUIElements[7].SetActive(true);
            healthUIElements[5].SetActive(true);
            healthUIElements[4].SetActive(true);
            healthUIElements[3].SetActive(true);
            healthUIElements[2].SetActive(true);
            healthUIElements[1].SetActive(true);
            healthUIElements[0].SetActive(true);

        }
        if (healthPercent <= .3f)
        {
            healthUIElements[9].SetActive(true);
            healthUIElements[8].SetActive(true);
            healthUIElements[6].SetActive(true);
            healthUIElements[7].SetActive(true);
            healthUIElements[5].SetActive(true);
            healthUIElements[4].SetActive(true);
            healthUIElements[3].SetActive(true);
            healthUIElements[2].SetActive(true);
            healthUIElements[1].SetActive(true);
            healthUIElements[0].SetActive(true);
        }
    }
}
