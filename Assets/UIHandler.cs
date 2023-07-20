using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth ph;
    private playerAimAttack paa;

    public GameObject Left_Equip, Right_Equip;
    public Image LSniper, LHover, LBeef, RSniper, RHover, RBeef;
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
    }
}
