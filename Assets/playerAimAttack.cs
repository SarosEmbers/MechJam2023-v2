using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAimAttack : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Aiming")]
    public Transform orientation, reticlePos;
    public float maxLockOnDistance = 25;
    public float maxFiringRange = 10;
    public LayerMask obstructionMask;
    public LayerMask enemyMask;
    public List<GameObject> targettingEnemies;
    public GameObject PLArm, PRArm;
    public Transform armAimTarget;
    public GameObject enemyToAimAt;

    [Header("Attacking")]
    public bool isAttacking = false;
    public bool isBarrageTime = false;
    public float beefBarrageTimer, beefBarrageTimerMax, beefBarrageCooldown, beefBarrageCooldownMax;
    public GameObject beefProjectile, hoverProjectile, speedyProjectile;
    public Transform LbeefPoint, LhoverPoint, LspeedyPoint;
    public Transform RbeefPoint, RhoverPoint, RspeedyPoint;
    public float hoverFireRate, hoverFiretimer, hoverFireTimerMax, hoverFireRange;
    public enum StolenArms
    {
        Beefy,
        Speedy,
        Hover,
        Player36
    }

    [Header("Scrounge")]
    private ScroungeManager sM;
    public StolenArms LeftEquppedArm = StolenArms.Player36;
    public StolenArms RightEquppedArm = StolenArms.Player36;

    // Start is called before the first frame update
    void Start()
    {
        sM = GetComponent<ScroungeManager>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targettingEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
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

                break;
            case StolenArms.Beefy:

                sM.changePart("LArm", 1);

                if(enemyToAimAt != null)
                {
                    Quaternion armRotStore = Quaternion.LookRotation(enemyToAimAt.transform.position);
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
                    isBarrageTime = true;
                    beefBarrageTimer = beefBarrageTimerMax;
                    Debug.Log("FIRE: CLICK");
                }

                if (Input.GetButton("Fire1"))
                {
                    Debug.Log("FIRE: HOLDING");

                    if (beefBarrageCooldown <= 0)
                    {
                        Debug.Log("FIRE: NO COOLDOWN");

                        if (beefBarrageTimer > 0)
                        {
                            beefBarrageTimer -= Time.deltaTime;
                        }

                        RaycastHit hit;
                        if(Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
                        {
                            Debug.Log("FIRE: Raycast SENT");
                            if (hit.transform.tag == "Enemy")
                            {
                                GameObject foundEnemy = GameObject.Find(hit.transform.name);

                                if (!targettingEnemies.Contains(foundEnemy))
                                {
                                    targettingEnemies.Add(foundEnemy);
                                    foundEnemy.GetComponent<enemySpotted>().enemySelected();
                                    Debug.Log("ENEMY SPOTTED: " + hit.transform.name);
                                    enemyToAimAt = foundEnemy;
                                }
                            }
                        }
                    }
                }

                if(Input.GetButtonUp("Fire1"))
                {
                    StartCoroutine(BeefFireRockets(targettingEnemies.Count, 0.1f, 3, LbeefPoint));
                    foreach (GameObject enemy in targettingEnemies)
                    {
                        enemy.GetComponent<enemySpotted>().deselectEnemy();
                    }

                    /*
                    if (beefBarrageTimer <= 0)
                    {

                    }
                    else
                    {
                        foreach(GameObject enemy in targettingEnemies)
                        {
                            enemy.GetComponent<enemySpotted>().deselectEnemy();
                        }
                        targettingEnemies.Clear();
                    }
                    */
                }

                break;
            case StolenArms.Speedy:

                sM.changePart("LArm", 2);

                break;
            case StolenArms.Hover:

                if(Input.GetButton("Fire1"))
                {
                    hoverFire(LhoverPoint);
                }

                sM.changePart("LArm", 3);

                break;
        }
    }
    public void hoverFire(Transform fireFrom)
    {
        if (hoverFiretimer > 0)
        {
            hoverFiretimer -= Time.deltaTime;
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
                    //yoink their health
                }
            }

            hoverFiretimer = hoverFireRate;
        }
    }
    public IEnumerator BeefFireRockets(int numberOfTargets, float fireInterval, int rocketsPerEnemy, Transform fireFrom)
    {
        WaitForSeconds wait = new WaitForSeconds(fireInterval);

        for (int i = 0; i < numberOfTargets; i++)
        {
            for (int j = 0; j < rocketsPerEnemy; j++)
            {
                Debug.Log("Firing Rockets: " + i + " || " + targettingEnemies[i] + " || " + targettingEnemies.Count);
                if(targettingEnemies[i] != null)
                {
                    Vector3 targetPos = targettingEnemies[i].transform.position - transform.position;
                    GameObject beefRocket = Instantiate(beefProjectile, fireFrom.position, Quaternion.LookRotation(targetPos));
                    beefRocket.GetComponent<homingRocket>().target = targettingEnemies[i];
                }
                else
                {
                    Vector3 emptyPos = Camera.main.transform.position - reticlePos.transform.position;
                    GameObject beefRocket = Instantiate(beefProjectile, fireFrom.position, Quaternion.LookRotation(emptyPos));
                    beefRocket.GetComponent<homingRocket>().target = null;
                }
                yield return wait;
            }
        }
        targettingEnemies.Clear();
        yield return null;
    }
    public void FireArm()
    {

    }
    public void ArmChecker()
    {

    }
}
