using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectParts : MonoBehaviour
{
    private GameObject player;
    private playerAimAttack paa;
    private ScroungeManager sM;
    private int scrapIndex = 0;
    public enum ScrapPart
    {
        Beefy,
        Speedy,
        Hover,
        none
    }
    public ScrapPart thisScrapPart = ScrapPart.none;
    public bool isLeg;
    // Start is called before the first frame update
    void Start()
    {
        switch (thisScrapPart)
        {
            case ScrapPart.Beefy:
                scrapIndex = 1;
                break;
            case ScrapPart.Speedy:
                scrapIndex = 2;
                break;
            case ScrapPart.Hover:
                scrapIndex = 3;
                break;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        paa = player.GetComponent<playerAimAttack>();
        sM = player.GetComponent<ScroungeManager>();
        //Destroy(this.gameObject, 7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(isLeg == false)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    switch (scrapIndex)
                    {
                        case 1:
                            paa.LeftEquppedArm = playerAimAttack.StolenArms.Beefy;
                            break;
                        case 2:
                            paa.LeftEquppedArm = playerAimAttack.StolenArms.Speedy;
                            break;
                        case 3:
                            paa.LeftEquppedArm = playerAimAttack.StolenArms.Hover;
                            break;
                    }
                    Destroy(this.gameObject);
                }
                else if (Input.GetButtonDown("Fire2"))
                {
                    switch (scrapIndex)
                    {
                        case 1:
                            paa.RightEquppedArm = playerAimAttack.StolenArms.Beefy;
                            break;
                        case 2:
                            paa.RightEquppedArm = playerAimAttack.StolenArms.Speedy;
                            break;
                        case 3:
                            paa.RightEquppedArm = playerAimAttack.StolenArms.Hover;
                            break;
                    }
                    sM.changePart("RArm", scrapIndex);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    sM.changePart("Legs", scrapIndex);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
