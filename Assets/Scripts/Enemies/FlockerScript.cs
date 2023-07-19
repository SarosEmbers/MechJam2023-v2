using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerScript : MonoBehaviour
{
    private Rigidbody enemBod;
    private EnemyFireAt efa;

    public Vector3 tempVar;

    [Header("Movement")]
    public float maxVel = 5;
    public float rotSpeed = 1;
    private float storeRotSpeed;
    public float enemMoveSpeed = 25.0f;
    public float enemStrafeSpeed = 10.0f;
    public float dashStrength = 5;
    public float enemyJumpAmount = 15;
    public bool enemHasJumped = false;

    public Transform enemyOrientation;
    public GameObject LArm, RArm;

    public enum FlockingMode
    {
        Idle,
        EngageTarget,
        TurretTarget
    }

    [Header("Attack AI")]
    public FlockingMode CurrentFlockingMode = FlockingMode.Idle;
    public GameObject FlockingTarget;

    public float aimSpeed = 15.0f;

    private bool leftOrRight = true;



    public enum AttackState
    {
        Sentry, //Stationary and simply firing upon the player
        Approach, //Moving Towards the player
        backOff, //Moving Away from the player
        circleAround, //Circle Around the player at a set distance
        maintainDistance, //Stay a set distance away from player
        Idle
    }
    public AttackState CurrentAttackState = AttackState.Idle;
    public Vector2 RandStateInterval;
    public float stateChangeTimer = 5;
    public bool randomizeStates = false;
    public Vector2 RandActionInterval;
    public float actionTimer = 5;
    public bool canAction = false;
    public bool randomizeAction = false;
    public bool takingAction = false; //if the enemy is taking a tactical action

    [Header("Awareness AI")]
    public Transform headPoint;
    public bool AvoidHazards = true;
    public float distanceFromHazard = 4.0f;
    public float FOVRadius = 10; //Detect Range
    [Range(0, 360)]
    public float FOVAngle = 45;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool playerSpotted, lookingFor;
    public float lookingForTimer, lookingForTimerMax;
    public Vector2 ChaseRange;
    public Vector2 DesiredDistanceFromTarget;

    [Header("Other")]
    public Animator enemyMoveAnim;

    // Use this for initialization
    void Start()
    {
        FlockingTarget = GameObject.FindGameObjectWithTag("Player");
        enemBod = GetComponent<Rigidbody>();
        StartCoroutine(FinderRoutine());
        storeRotSpeed = rotSpeed;

        efa = GetComponent<EnemyFireAt>();
    }
    /*
    private void OnEnable()
    {
        StartCoroutine(FinderRoutine());
    }
    */

    private IEnumerator FinderRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewcheck();
        }
    }

    private void FieldOfViewcheck()
    {
        Collider[] rangeCheck = Physics.OverlapSphere(headPoint.position, FOVRadius, targetMask);

        if (rangeCheck.Length != 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - enemyOrientation.position).normalized;

            if (Vector3.Angle(headPoint.forward, directionToTarget) < FOVAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(enemyOrientation.position, target.position);

                RaycastHit findPlayer;
                if (!Physics.Raycast(enemyOrientation.position, directionToTarget, out findPlayer, distanceToTarget, obstructionMask))
                {
                    if(findPlayer.transform.gameObject.tag == "Player")
                    {
                        playerSpotted = true;
                        efa.canAttack = true;
                        lookingFor = false;
                    }
                }
                else
                {
                    lookingFor = true;
                    efa.canAttack = false;
                    lookingForTimer = lookingForTimerMax;
                }
            }
            else
            {
                lookingFor = true;
                efa.canAttack = false;
                lookingForTimer = lookingForTimerMax;
            }
        }
        else if (playerSpotted)
        {
            lookingFor = true;
            efa.canAttack = false;
            lookingForTimer = lookingForTimerMax;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredDirection = new Vector3();

        Vector3 vectorToTarget = FlockingTarget.transform.position - transform.position;
        float distanceToTarget = vectorToTarget.magnitude;

        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        float zAxis = enemBod.velocity.x;
        float xAxis = enemBod.velocity.y;

        int horizInt = Mathf.RoundToInt(xAxis);
        int vertInt = Mathf.RoundToInt(zAxis);

        enemyMoveAnim.SetInteger("VerticalAnim", vertInt);
        enemyMoveAnim.SetInteger("HorizontalAnim", horizInt);

        if(lookingFor == true)
        {
            if(lookingForTimer >= 0f)
            {
                lookingForTimer -= Time.deltaTime;
            }
            else
            {
                playerSpotted = false;
                efa.canAttack = false;
                enemBod.angularVelocity = Vector3.zero;
                lookingFor = false;
            }
        }

        if (playerSpotted)
        {
            CurrentFlockingMode = FlockingMode.EngageTarget;
        }
        else
        {
            CurrentFlockingMode = FlockingMode.Idle;
        }

        if (CurrentAttackState == AttackState.circleAround)
            rotSpeed = 1000;
        else
            rotSpeed = storeRotSpeed;

        if (enemHasJumped)
        {
            //Vector3 force = fallSpeed;

            if (enemBod.velocity.y < 5)
            {
                if (enemBod.velocity.y > 1)
                {
                    enemBod.velocity = new Vector3(enemBod.velocity.x, 1, enemBod.velocity.z);
                }
                else
                {
                    enemBod.velocity = new Vector3(enemBod.velocity.x, enemBod.velocity.y - 1, enemBod.velocity.z);
                }
            }
        }

        switch (CurrentFlockingMode)
        {
            case FlockingMode.Idle:

                break;
            case FlockingMode.EngageTarget:

                //======================================================================================================================================================

                if (!takingAction)
                {
                    if (distanceToTarget <= ChaseRange.y)
                    {
                        RotateTowardsPlayer();

                        switch (CurrentAttackState)
                        {
                            case AttackState.Approach:

                                if (distanceToTarget >= ChaseRange.x)
                                {
                                    ForwardThrust(enemMoveSpeed);
                                }

                                break;
                            case AttackState.backOff:

                                if (distanceToTarget >= DesiredDistanceFromTarget.x)
                                {
                                    ForwardThrust(-enemMoveSpeed);
                                }

                                break;
                            case AttackState.maintainDistance:

                                if (distanceToTarget >= DesiredDistanceFromTarget.y)
                                {
                                    ForwardThrust(enemMoveSpeed);
                                }
                                else if (distanceToTarget <= DesiredDistanceFromTarget.x)
                                {
                                    ForwardThrust(-enemMoveSpeed);
                                }

                                break;
                            case AttackState.circleAround:

                                if (leftOrRight == true) StrafeThrust(enemStrafeSpeed);
                                else StrafeThrust(-enemStrafeSpeed);

                                if (distanceToTarget >= DesiredDistanceFromTarget.y)
                                {
                                    ForwardThrust(enemStrafeSpeed);
                                }
                                else if (distanceToTarget <= DesiredDistanceFromTarget.x)
                                {
                                    ForwardThrust(-enemStrafeSpeed);
                                }

                                break;

                            case AttackState.Sentry:

                                break;
                        }

                        /*
                        if (distanceToTarget <= ChaseRange.y)
                        {
                            ForwardThrust(enemMoveSpeed);
                        }
                        */
                    }
                    else if (distanceToTarget >= ChaseRange.y)
                    {
                        playerSpotted = false;
                    }
                }

                if(randomizeAction)
                {
                    if (!canAction && !takingAction)
                    {
                        if (actionTimer > 0)
                        {
                            actionTimer -= Time.deltaTime;
                        }
                        else
                        {
                            Debug.Log("ACTION: Action Ready");
                            actionTimer = Random.Range(RandActionInterval.x, RandActionInterval.y);
                            canAction = true;

                            if (leftOrRight == true) leftOrRight = false;
                            else if (leftOrRight == false) leftOrRight = true;
                        }
                    }
                    else if (canAction)
                    {
                        int randChance = Random.Range(0, 2);
                        //Debug.Log("ACTION RAND: " + randChance);

                        switch (randChance)
                        {
                            case 0:
                                int randDir = Random.Range(0, 3);
                                EnemyDash(randDir);
                                takingAction = true;
                                break;
                            case 1:
                                EnemyJump(enemyJumpAmount);
                                break;
                        }


                        canAction = false;
                    }
                }

                if (randomizeStates)
                {
                    if (stateChangeTimer > 0)
                    {
                        stateChangeTimer -= Time.deltaTime;
                    }
                    else
                    {
                        //Debug.Log("STATE: MIX IT UP");

                        int randState = Random.Range(0, 5);

                        switch (randState)
                        {
                            case 0:
                                CurrentAttackState = AttackState.Approach;
                                break;
                            case 1:
                                CurrentAttackState = AttackState.backOff;
                                break;
                            case 2:
                                CurrentAttackState = AttackState.maintainDistance;
                                break;
                            case 3:
                                CurrentAttackState = AttackState.circleAround;
                                break;
                            case 4:
                                CurrentAttackState = AttackState.Sentry;
                                break;
                            case 5:
                                CurrentAttackState = CurrentAttackState;
                                break;
                        }

                        stateChangeTimer = Random.Range(RandStateInterval.x, RandStateInterval.y);
                    }
                }


                //======================================================================================================================================================

                break;
            case FlockingMode.TurretTarget:

                if (distanceToTarget <= ChaseRange.y)
                {
                    RotateTowardsPlayer();
                }
                else if (distanceToTarget >= ChaseRange.y)
                {
                    playerSpotted = false;
                }

                break;
        }

        if (AvoidHazards)
        {
            HazardScript[] hazards = FindObjectsOfType<HazardScript>();
            GameObject[] enemyMechs = GameObject.FindGameObjectsWithTag("Enemy");
            Vector3 avoidanceVector = Vector3.zero;
            for (int i = 0; i < hazards.Length; ++i)
            {
                var collider = hazards[i].GetComponent<Collider>();

                if (!collider)
                {
                    return; // nothing to do without a collider
                }

                Vector3 closestPoint = collider.ClosestPoint(transform.position);
                tempVar = closestPoint;

                Vector3 vectorToHazard = closestPoint - transform.position;
                if (vectorToHazard.magnitude < distanceFromHazard)
                {
                    Vector3 vectorAwayFromHazard = -vectorToHazard;

                    avoidanceVector += vectorAwayFromHazard;
                }
            }
            for (int i = 0; i < enemyMechs.Length; ++i)
            {
                var collider = enemyMechs[i].GetComponent<Collider>();

                if (!collider)
                {
                    return; // nothing to do without a collider
                }

                Vector3 closestPoint = collider.ClosestPoint(transform.position);
                tempVar = closestPoint;

                Vector3 vectorToHazard = closestPoint - transform.position;
                if (vectorToHazard.magnitude < distanceFromHazard)
                {
                    Vector3 vectorAwayFromHazard = -vectorToHazard;

                    avoidanceVector += vectorAwayFromHazard;
                }
            }

            if (avoidanceVector != Vector3.zero)
            {
                desiredDirection.Normalize();
                avoidanceVector.Normalize();

                desiredDirection = desiredDirection * 0.5f + avoidanceVector * 0.5f;

                AvoidThrust(avoidanceVector);
            }
        }

        //desiredDirection.Normalize();
        //transform.position += desiredDirection * SpeedPerSecond * Time.deltaTime;
    }



    private void ForwardThrust(float amount)
    {
        Vector3 force = transform.forward * amount;

        enemBod.AddForce(force);
    }

    private void StrafeThrust(float amount)
    {
        Vector3 force = enemyOrientation.right * amount;
        enemBod.AddForce(force);
    }

    private void AvoidThrust(Vector3 awayFromHazard)
    {
        Vector3 force = awayFromHazard * enemMoveSpeed * 1.5f;

        enemBod.AddForce(force);
    }

    private void ClampVelocity()
    {
        float z = Mathf.Clamp(enemBod.velocity.z, -maxVel, maxVel);
        float x = Mathf.Clamp(enemBod.velocity.x, -maxVel, maxVel);

        enemBod.velocity = new Vector3(x, enemBod.velocity.y, z);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerPos = FlockingTarget.transform.position - transform.position;

        Vector3 bodyTurnTo = new Vector3(playerPos.x, 0.0f, playerPos.z);
        //Vector3 armsAimAt = new Vector3(playerPos.x, 0.0f, 0.0f);

        Quaternion bodRot = Quaternion.LookRotation(bodyTurnTo);
        Quaternion armRot = Quaternion.LookRotation(playerPos);

        transform.rotation = Quaternion.Lerp(transform.rotation, bodRot, rotSpeed * Time.deltaTime);
        LArm.transform.rotation = Quaternion.Lerp(LArm.transform.rotation, armRot, aimSpeed * Time.deltaTime);
        RArm.transform.rotation = Quaternion.Lerp(RArm.transform.rotation, armRot, aimSpeed * Time.deltaTime);

        //t.Rotate(0, amound, 0);
    }
    private void Rotate(Transform t, float amound)
    {
        t.Rotate(0, amound, 0);
    }

    private void EnemyJump(float jumpAmount)
    {
        enemyMoveAnim.SetTrigger("JumpAnim");

        Vector3 jumpForce = enemyOrientation.up * jumpAmount * 100;

        enemBod.AddForce(jumpForce);

        takingAction = false;
        canAction = false;
    }

    void EnemyDash(int dDirectionIndex)
    {
        Vector3 force = Vector3.zero;

        switch (dDirectionIndex)
        {
            case 0:
                Vector3 dF = transform.forward * dashStrength * 100;
                force = dF;
                break;
            case 1:
                Vector3 dB = -transform.forward * dashStrength * 100;
                force = dB;
                break;
            case 2:
                Vector3 dL = -transform.right * dashStrength * 100;
                force = dL;
                break;
            case 3:
                Vector3 dR = transform.right * dashStrength * 100;
                force = dR;
                break;
        }
        enemBod.AddForce(force);
        Invoke("slowDown", 0.4f);
    }

    public void slowDown()
    {
        float z = Mathf.Clamp(enemBod.velocity.z, -5, 5);
        float x = Mathf.Clamp(enemBod.velocity.x, -5, 5);

        enemBod.velocity = new Vector3(x, 0.0f, z);
        //rb.velocity =
        canAction = false;
        takingAction = false;
    }

    private void AttackAction(string actionToTake)
    {

    }

    public void reTargetPlayer()
    {
        FlockingTarget = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, distanceFromHazard);
        Gizmos.DrawWireSphere(tempVar, distanceFromHazard);
    }
}
