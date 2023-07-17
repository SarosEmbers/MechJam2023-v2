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

    [Header("Attacking")]
    public bool isAttacking = false;
    public float beefBarrageTimer, beefBarrageTimerMax, beefBarrageCooldown, beefBarrageCooldownMax;
    public GameObject beefProjectile, hoverProjectile, speedyProjectile;
    public Transform LbeefPoint, LhoverPoint, LspeedyPoint;
    public Transform RbeefPoint, RhoverPoint, RspeedyPoint;
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
    void Update()
    {

        Debug.DrawRay(reticlePos.position, reticlePos.forward, Color.red);

        switch (LeftEquppedArm)
        {
            case StolenArms.Player36:

                sM.changePart("LArm", 0);

                break;
            case StolenArms.Beefy:

                sM.changePart("LArm", 1);

                if (Input.GetButtonDown("Fire1"))
                {
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
