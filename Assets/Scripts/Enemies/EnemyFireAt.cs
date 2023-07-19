using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireAt : MonoBehaviour
{
    private Rigidbody enamRb;
    private GameObject player;
    public enum BotTypes
    {
        Beefy,
        Speedy,
        Hover
    }
    public enum AggressionLevel
    {
        Dummy,
        Default,
        Hellbent
    }
    [Header("Attacking Universal")]
    public BotTypes enemyBotType = BotTypes.Beefy;
    public AggressionLevel aggroLevel = AggressionLevel.Default;
    public float attackTimer, attackTimerMax;
    public float maxRange;
    public float damage;
    public bool canAttack, isAttacking;

    [Header("Attack: BEEF")]
    public GameObject beefProjectile;
    public Transform LGun, RGun;
    public float BeeffrequencyTimer;

    [Header("Attack: Hover")]
    public Transform hoverFirePoint;
    public float preFireTime;
    public float fireRateMax, fireRateTimer;
    public float fireDuration, firDurationMax;
    public GameObject hoverProjectile;

    [Header("Attack: Sniper")]
    public Transform sniperFirePoint, LSniper, RSniper;
    public float telegraphTime, fireDelay;
    public GameObject sniperProjectile,  sniperPoff;
    public Transform temp_aim, temp_point;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= 0.0f)
        {
            StopCoroutine(BeefFire_enem(.25f, 4));
            StopCoroutine(HoverFire_Enem());
            attackTimer -= 1 * Time.deltaTime;
        }
        else
        {
            if(canAttack)
            {
                int randChance = Random.Range(1, 3);

                if (randChance >= 2)
                {
                    Debug.Log("ATTACK");
                    switch (enemyBotType)
                    {
                        case BotTypes.Beefy:
                            StartCoroutine(BeefFire_enem(.25f, 4));
                            attackTimer = attackTimerMax;
                            break;
                        case BotTypes.Speedy:
                            SniperTelegraph_Enem();
                            //play telegraph SFX
                            break;
                        case BotTypes.Hover:
                            attackTimer = attackTimerMax + fireDuration + fireDelay;

                            StartCoroutine(HoverFire_Enem());

                            break;
                    }
                }
            }

        }


    }

    public IEnumerator BeefFire_enem(float fireInterval, int howManyRockets)
    {
        WaitForSeconds wait = new WaitForSeconds(fireInterval);

        for (int j = 0; j < howManyRockets; j++)
        {
            Debug.Log("Hiver Fire");

            int whichSide = Random.Range(0, 1);

            Vector3 targetPos = player.transform.position - transform.position;
            if (whichSide == 0)
            {
                GameObject beefRocket = Instantiate(beefProjectile, LGun.position, Quaternion.LookRotation(targetPos));
                beefRocket.GetComponent<homingRocket>().target = player;
            }
            else
            {
                GameObject beefRocket = Instantiate(beefProjectile, RGun.position, Quaternion.LookRotation(targetPos));
                beefRocket.GetComponent<homingRocket>().target = player;
            }
            yield return wait;
        }
        yield return null;
    }

    public void SniperTelegraph_Enem()
    {
        Invoke("SniperDelay_Enem", telegraphTime);
        attackTimer = attackTimerMax + telegraphTime + fireDelay;
    }
    public void SniperDelay_Enem()
    {
        sniperFirePoint = player.transform;
        temp_aim = sniperFirePoint;
        temp_point = LSniper;
        //play alert SFX
        Invoke("SniperFire_Enem", fireDelay);
    }
    public void SniperFire_Enem()
    {
        Vector3 storedPoint = temp_point.position - temp_aim.position;
        RaycastHit hit;
        if (Physics.Raycast(temp_aim.position, storedPoint, out hit, maxRange))
        {
            if (hit.transform.tag == "Player")
            {
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
            }

            
            GameObject hitParticle = Instantiate(sniperPoff, hit.point, Quaternion.identity);
            Destroy(hitParticle, .75f);
            

            Vector3 gunToPoint = LSniper.position - temp_aim.position;

            GameObject fireParticle = Instantiate(sniperProjectile, LSniper.position, Quaternion.LookRotation(gunToPoint));
            Destroy(fireParticle, .75f);
        }
        else if (hit.transform == null)
        {
            Vector3 gunToPoint = LSniper.position - temp_aim.position;

            GameObject fireParticle = Instantiate(sniperProjectile, LSniper.position, Quaternion.LookRotation(gunToPoint));
            Destroy(fireParticle, .75f);
        }
    }


    public IEnumerator HoverFire_Enem()
    {
        WaitForSeconds wait = new WaitForSeconds(fireRateMax);
        Debug.Log("We in that enumerator like uhhh");
        for (float j = 0; j < fireDuration; j++)
        {
            hoverFirePoint = player.transform;
            Transform prevFirePoint = hoverFirePoint;

            yield return wait;

            //Vector3 storedPoint = temp_aim.position - hoverFirePoint.position;
            RaycastHit hit;
            if (Physics.Raycast(prevFirePoint.position, prevFirePoint.forward, out hit, maxRange))
            {
                if (hit.transform.tag == "Player")
                {
                    player.GetComponent<PlayerHealth>().TakeDamage(damage);
                }

                Vector3 gunToPoint = LSniper.position - hoverFirePoint.transform.position;

                GameObject fireParticle = Instantiate(hoverProjectile, LSniper.position, Quaternion.LookRotation(gunToPoint));
                Destroy(fireParticle, .75f);
            }

            j -= Time.deltaTime;
        }

    }
}
