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

    [Header("Attack: BEEF")]
    public GameObject beefProjectile;
    public Transform LGun, RGun;
    public float BeeffrequencyTimer;

    [Header("Attack: Hover")]
    public Transform hoverFirePoint;
    public float preFireTime;

    [Header("Attack: Sniper")]
    public Transform sniperFirePoint, LSniper, RSniper;
    public float telegraphTime, fireDelay;
    public GameObject sniperProjectile;


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
            attackTimer -= 1 * Time.deltaTime;
        }
        else
        {
            int randChance = Random.Range(1, 3);

            if(randChance >= 2)
            {
                switch (enemyBotType)
                {
                    case BotTypes.Beefy:

                    break;
                    case BotTypes.Speedy:

                        break;
                    case BotTypes.Hover:

                        break;
                }
            }
        }


    }

    public IEnumerator BeefFire_enem(float fireInterval, int howManyRockets)
    {
        WaitForSeconds wait = new WaitForSeconds(fireInterval);

        for (int j = 0; j < howManyRockets; j++)
        {
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
        Invoke("SniperFire_Enem", fireDelay);
    }
    public void SniperFire_Enem()
    {
        Vector3 storedPoint = LSniper.position - sniperFirePoint.position;
        RaycastHit hit;
        if (Physics.Raycast(LSniper.position, LSniper.forward, out hit, maxRange))
        {
            if (hit.transform.tag == "Enemy")
            {
                GameObject foundEnemy = GameObject.Find(hit.transform.name);
                foundEnemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }

            /*
            GameObject hitParticle = Instantiate(sniperParticle, hit.point, Quaternion.identity);
            Destroy(hitParticle, .75f);
            */

            Vector3 gunToPoint = sniperFirePoint.position - hit.point;

            GameObject fireParticle = Instantiate(sniperProjectile, sniperFirePoint.position, Quaternion.LookRotation(gunToPoint));
            Destroy(fireParticle, .75f);
        }
        else if (hit.transform == null)
        {
            GameObject fireParticle = Instantiate(sniperProjectile, sniperFirePoint.position, Quaternion.LookRotation(transform.forward));
            Destroy(fireParticle, .75f);
        }
    }
}
