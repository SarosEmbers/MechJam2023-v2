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

                break;
            case StolenArms.Beefy:

                if(Input.GetButtonDown("Fire1"))
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
                    if(beefBarrageTimer <= 0)
                    {
                        Debug.Log("FIRE");
                    }
                    else
                    {
                        foreach(GameObject enemy in targettingEnemies)
                        {
                            enemy.GetComponent<enemySpotted>().deselectEnemy();
                        }
                        targettingEnemies.Clear();
                    }
                }

                break;
        }
    }

    public void FireArm()
    {

    }
    public void ArmChecker()
    {

    }
}
