using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAimAttack : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject playerFireDefault, playerPoofDefault;
    public float playerDamage;
    public Transform headPoint;

    [Header("Aiming")]
    public Transform orientation, reticlePos;
    public float maxLockOnDistance = 25;
    public float maxFiringRange = 10;
    public LayerMask obstructionMask;
    public LayerMask enemyMask;
    public List<GameObject> targettingEnemies_Left;
    public List<GameObject> targettingEnemies_Right;
    public GameObject PLArm, PRArm, headHolder;
    public Transform armAimTarget;
    public GameObject enemyToAimAt_Left;
    public GameObject enemyToAimAt_Right;

    [Header("Attacking Universal")]
    public bool isAttacking = false;
    public GameObject beefProjectile, hoverProjectile, speedyProjectile;
    public GameObject beefParticle, hoverParticle, speedyParticle;
    public float beefDamage, speedyDamage, hoverDamage;
    public enum StolenArms
    {
        Beefy,
        Speedy,
        Hover,
        Player36
    }

    private ScroungeManager sM;

    [Header("LEFT ARM")]
    public StolenArms LeftEquppedArm = StolenArms.Player36;
    public Transform LbeefPoint, LhoverPoint, LspeedyPoint;
    public bool isBarrageTime_Left = false;
    public float beefBarrageTimer_Left, beefBarrageTimerMax_Left, beefBarrageCooldown_Left, beefBarrageCooldownMax_Left;
    public float hoverFireRate_Left, hoverFiretimer_Left, hoverFireTimerMax_Left, hoverFireRange_Left;
    public float sniperFireRate_Left, sniperFiretimer_Left;


    [Header("RIGHT ARM")]
    public StolenArms RightEquppedArm = StolenArms.Player36;
    public Transform RbeefPoint, RhoverPoint, RspeedyPoint;
    public bool isBarrageTime_Right = false;
    public float beefBarrageTimer_Right, beefBarrageTimerMax_Right, beefBarrageCooldown_Right, beefBarrageCooldownMax_Right;
    public float hoverFireRate_Right, hoverFiretimer_Right, hoverFireTimerMax_Right, hoverFireRange_Right;
    public float sniperFireRate_Right, sniperFiretimer_Right;


    // Start is called before the first frame update
    void Start()
    {
        sM = GetComponent<ScroungeManager>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targettingEnemies_Left = new List<GameObject>();
        targettingEnemies_Right = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.DrawRay(reticlePos.position, reticlePos.forward, Color.red, 100);
        /*
        RaycastHit farHit;
        if(!isAttacking && Physics.Raycast(reticlePos.position, reticlePos.forward, out farHit, maxLockOnDistance))
        {
            Quaternion armRot = Quaternion.LookRotation(farHit.transform.position);
            PLArm.transform.rotation = Quaternion.Lerp(PLArm.transform.rotation, armRot, 15 * Time.deltaTime);
            PRArm.transform.rotation = Quaternion.Lerp(PRArm.transform.rotation, armRot, 15 * Time.deltaTime);
        }
        */

        switch (LeftEquppedArm)
        {
            case StolenArms.Player36:

                sM.changePart("LArm", 0);

                if (Input.GetButtonDown("Fire1"))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
                    {
                        if (hit.transform.tag == "Enemy")
                        {
                            GameObject foundEnemy = GameObject.Find(hit.transform.name);
                            foundEnemy.GetComponent<EnemyHealth>().TakeDamage(playerDamage);
                        }
                        Vector3 gunToPoint = headPoint.transform.position - hit.point;

                        GameObject fireParticle = Instantiate(playerFireDefault, headPoint.transform.position, Quaternion.LookRotation(gunToPoint));
                        GameObject firePoof = Instantiate(playerPoofDefault, headPoint.transform.position, Quaternion.LookRotation(gunToPoint));
                        Destroy(fireParticle, 1.75f);
                        Destroy(firePoof, 1.75f);
                    }
                }

                break;
            case StolenArms.Beefy:

                sM.changePart("LArm", 1);

                if(enemyToAimAt_Left != null)
                {
                    Quaternion armRotStore = Quaternion.LookRotation(enemyToAimAt_Left.transform.position);
                    PLArm.transform.rotation = Quaternion.Lerp(PLArm.transform.rotation, armRotStore, 15 * Time.deltaTime);
                }
                else
                {
                    Quaternion simplyForward = Quaternion.LookRotation(orientation.forward);
                    PLArm.transform.rotation = simplyForward;
                    //PLArm.transform.rotation = Quaternion.Lerp(PLArm.transform.rotation, simplyForward, 15 * Time.deltaTime);
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    isBarrageTime_Left = true;
                    beefBarrageTimer_Left = beefBarrageTimerMax_Left;
                }

                if (Input.GetButton("Fire1"))
                {
                    if (beefBarrageCooldown_Left <= 0)
                    {
                        if (beefBarrageTimer_Left > 0)
                        {
                            beefBarrageTimer_Left -= Time.deltaTime;
                        }

                        RaycastHit hit;
                        if(Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
                        {
                            if (hit.transform.tag == "Enemy")
                            {
                                GameObject foundEnemy = GameObject.Find(hit.transform.name);

                                if (!targettingEnemies_Left.Contains(foundEnemy))
                                {
                                    targettingEnemies_Left.Add(foundEnemy);
                                    foundEnemy.GetComponent<enemySpotted>().enemySelected();
                                    enemyToAimAt_Left = foundEnemy;
                                }
                            }
                        }
                    }
                }

                if(Input.GetButtonUp("Fire1"))
                {
                    StartCoroutine(BeefFireRocketsLeft(targettingEnemies_Left.Count, 0.1f, 3));
                    foreach (GameObject enemy in targettingEnemies_Left)
                    {
                        enemy.GetComponent<enemySpotted>().deselectEnemy();
                    }
                    targettingEnemies_Right.Clear();
                }

                break;
            case StolenArms.Speedy:

                sM.changePart("LArm", 2);

                if (sniperFiretimer_Left > 0)
                {
                    sniperFiretimer_Left -= Time.deltaTime;
                }
                else
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        sniperFireLeft();
                    }
                }

                break;
            case StolenArms.Hover:

                sM.changePart("LArm", 3);

                if (Input.GetButton("Fire1"))
                {
                    hoverFireLeft();
                }

                break;
        }

        switch (RightEquppedArm)
        {
            case StolenArms.Player36:

                sM.changePart("RArm", 0);

                if (Input.GetButtonDown("Fire2"))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
                    {
                        if (hit.transform.tag == "Enemy")
                        {
                            GameObject foundEnemy = GameObject.Find(hit.transform.name);
                            foundEnemy.GetComponent<EnemyHealth>().TakeDamage(playerDamage);
                        }
                        Vector3 gunToPoint = headPoint.transform.position - hit.point;

                        GameObject fireParticle = Instantiate(playerFireDefault, headPoint.transform.position, Quaternion.LookRotation(gunToPoint));
                        GameObject firePoof = Instantiate(playerPoofDefault, headPoint.transform.position, Quaternion.LookRotation(gunToPoint));
                        Destroy(fireParticle, 1.75f);
                        Destroy(firePoof, 1.75f);
                    }
                }
                break;
            case StolenArms.Beefy:

                sM.changePart("RArm", 1);

                if (enemyToAimAt_Right != null)
                {
                    Quaternion armRotStore = Quaternion.LookRotation(enemyToAimAt_Right.transform.position);
                    PRArm.transform.rotation = Quaternion.Lerp(PRArm.transform.rotation, armRotStore, 15 * Time.deltaTime);
                }
                else
                {
                    Quaternion simplyForward = Quaternion.LookRotation(orientation.forward);
                    PRArm.transform.rotation = simplyForward;
                    //PLArm.transform.rotation = Quaternion.Lerp(PLArm.transform.rotation, simplyForward, 15 * Time.deltaTime);
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    targettingEnemies_Right.Clear();
                    isBarrageTime_Right = true;
                    beefBarrageTimer_Right = beefBarrageTimerMax_Right;
                }

                if (Input.GetButton("Fire2"))
                {
                    if (beefBarrageCooldown_Right <= 0)
                    {
                        if (beefBarrageTimer_Right > 0)
                        {
                            beefBarrageTimer_Right -= Time.deltaTime;
                        }

                        RaycastHit hit;
                        if (Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
                        {
                            if (hit.transform.tag == "Enemy")
                            {
                                GameObject foundEnemy = GameObject.Find(hit.transform.name);

                                if (!targettingEnemies_Right.Contains(foundEnemy))
                                {
                                    targettingEnemies_Right.Add(foundEnemy);
                                    foundEnemy.GetComponent<enemySpotted>().enemySelected();
                                    enemyToAimAt_Right = foundEnemy;
                                }
                            }
                        }
                    }
                }

                if (Input.GetButtonUp("Fire2"))
                {
                    StartCoroutine(BeefFireRocketsRight(targettingEnemies_Right.Count, 0.1f, 3));
                    foreach (GameObject enemy in targettingEnemies_Right)
                    {
                        enemy.GetComponent<enemySpotted>().deselectEnemy();
                    }

                    if (beefBarrageTimer_Right <= 0)
                    {

                    }
                    else
                    {
                        foreach (GameObject enemy in targettingEnemies_Right)
                        {
                            enemy.GetComponent<enemySpotted>().deselectEnemy();
                        }
                        targettingEnemies_Right.Clear();
                    }
                }

                break;
            case StolenArms.Speedy:

                sM.changePart("RArm", 2);

                if (sniperFiretimer_Right > 0)
                {
                    sniperFiretimer_Right -= Time.deltaTime;
                }
                else
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        sniperFireRight();
                    }
                }

                break;
            case StolenArms.Hover:

                sM.changePart("RArm", 3);

                if (Input.GetButton("Fire2"))
                {
                    hoverFireRight();
                }

                break;
        }
    }
    public void hoverFireRight()
    {
        if (hoverFiretimer_Right > 0)
        {
            hoverFiretimer_Right -= Time.deltaTime;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
            {
                Debug.DrawRay(reticlePos.transform.position, reticlePos.forward, Color.magenta);

                if (hit.transform.tag == "Enemy")
                {
                    GameObject foundEnemy = GameObject.Find(hit.transform.name);
                    foundEnemy.GetComponent<EnemyHealth>().TakeDamage(hoverDamage);
                }
                GameObject hitParticle = Instantiate(hoverParticle, hit.point, Quaternion.identity);
                Destroy(hitParticle, .75f);

                Vector3 gunToPoint = RhoverPoint.position - hit.point;

                GameObject fireParticle = Instantiate(hoverProjectile, RhoverPoint.position, Quaternion.LookRotation(gunToPoint));
                Destroy(fireParticle, .75f);
            }

            hoverFiretimer_Right = hoverFireRate_Right;
        }
    }

    public void sniperFireRight()
    {
        Debug.Log("R SNIPE");

        RaycastHit hit;
        if (Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
        {
            if (hit.transform.tag == "Enemy")
            {
                GameObject foundEnemy = GameObject.Find(hit.transform.name);
                foundEnemy.GetComponent<EnemyHealth>().TakeDamage(speedyDamage);
            }

            GameObject hitParticle = Instantiate(speedyParticle, hit.point, Quaternion.identity);
            Destroy(hitParticle, .75f);

            Vector3 gunToPoint = RspeedyPoint.position - hit.point;

            GameObject fireParticle = Instantiate(speedyProjectile, RspeedyPoint.position, Quaternion.LookRotation(gunToPoint));
            Destroy(fireParticle, .75f);
        }
        else if (hit.transform == null)
        {
            GameObject fireParticle = Instantiate(speedyProjectile, RspeedyPoint.position, Quaternion.LookRotation(transform.forward));
            Destroy(fireParticle, .75f);
        }

        sniperFiretimer_Right = sniperFireRate_Right;
    }

    public IEnumerator BeefFireRocketsRight(int numberOfTargets, float fireInterval, int rocketsPerEnemy)
    {
        WaitForSeconds wait = new WaitForSeconds(fireInterval);

        for (int i = 0; i < numberOfTargets; i++)
        {
            for (int j = 0; j < rocketsPerEnemy; j++)
            {
                if (targettingEnemies_Right[i] != null)
                {
                    Vector3 targetPos = targettingEnemies_Right[i].transform.position - transform.position;
                    GameObject beefRocket = Instantiate(beefProjectile, RbeefPoint.position, Quaternion.LookRotation(targetPos));
                    beefRocket.GetComponent<homingRocket>().target = targettingEnemies_Right[i];
                }
                else
                {
                    Vector3 emptyPos = Camera.main.transform.position - reticlePos.transform.position;
                    GameObject beefRocket = Instantiate(beefProjectile, RbeefPoint.position, Quaternion.LookRotation(emptyPos));
                    beefRocket.GetComponent<homingRocket>().target = null;
                }
                yield return wait;
            }
        }

        foreach (GameObject enemy in targettingEnemies_Right)
        {
            enemy.GetComponent<enemySpotted>().deselectEnemy();
        }
        targettingEnemies_Right.Clear();
        yield return null;
    }


    public void hoverFireLeft()
    {
        if (hoverFiretimer_Left > 0)
        {
            hoverFiretimer_Left -= Time.deltaTime;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
            {
                Debug.DrawRay(reticlePos.transform.position, reticlePos.forward, Color.magenta);

                if (hit.transform.tag == "Enemy")
                {
                    GameObject foundEnemy = GameObject.Find(hit.transform.name);
                    foundEnemy.GetComponent<EnemyHealth>().TakeDamage(hoverDamage);
                }
                GameObject hitParticle = Instantiate(hoverParticle, hit.point, Quaternion.identity);
                Destroy(hitParticle, .75f);

                Vector3 gunToPoint = LhoverPoint.position - hit.point;

                GameObject fireParticle = Instantiate(hoverProjectile, LhoverPoint.position, Quaternion.LookRotation(gunToPoint));
                Destroy(fireParticle, .75f);
            }

            hoverFiretimer_Left = hoverFireRate_Left;
        }
    }

    public void sniperFireLeft()
    {
        RaycastHit hit;
        if (Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
        {

            if (hit.transform.tag == "Enemy")
            {
                GameObject foundEnemy = GameObject.Find(hit.transform.name);
                foundEnemy.GetComponent<EnemyHealth>().TakeDamage(speedyDamage);
            }
            GameObject hitParticle = Instantiate(speedyParticle, hit.point, Quaternion.identity);
            Destroy(hitParticle, .75f);

            Vector3 gunToPoint = LspeedyPoint.position - hit.point;

            GameObject fireParticle = Instantiate(speedyProjectile, LspeedyPoint.position, Quaternion.LookRotation(gunToPoint));
            Destroy(fireParticle, .75f);
        }
        else if (hit.transform == null)
        {
            GameObject fireParticle = Instantiate(speedyProjectile, LspeedyPoint.position, Quaternion.identity);
            Destroy(fireParticle, .75f);
        }
        sniperFiretimer_Left = sniperFireRate_Left;
    }

    public IEnumerator BeefFireRocketsLeft(int numberOfTargets, float fireInterval, int rocketsPerEnemy)
    {
        WaitForSeconds wait = new WaitForSeconds(fireInterval);

        for (int i = 0; i < numberOfTargets; i++)
        {
            for (int j = 0; j < rocketsPerEnemy; j++)
            {
                if (targettingEnemies_Left[i] != null)
                {
                    Vector3 targetPos = targettingEnemies_Left[i].transform.position - transform.position;
                    GameObject beefRocket = Instantiate(beefProjectile, LbeefPoint.position, Quaternion.LookRotation(targetPos));
                    beefRocket.GetComponent<homingRocket>().target = targettingEnemies_Left[i];
                }
                else
                {
                    Vector3 emptyPos = Camera.main.transform.position - reticlePos.transform.position;
                    GameObject beefRocket = Instantiate(beefProjectile, LbeefPoint.position, Quaternion.LookRotation(emptyPos));
                    beefRocket.GetComponent<homingRocket>().target = null;
                }
                yield return wait;
            }
        }

        foreach (GameObject enemy in targettingEnemies_Left)
        {
            enemy.GetComponent<enemySpotted>().deselectEnemy();
        }
        targettingEnemies_Left.Clear();
        yield return null;
    }
}
