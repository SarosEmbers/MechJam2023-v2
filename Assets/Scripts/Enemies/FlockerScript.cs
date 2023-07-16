using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerScript : MonoBehaviour
{
    private Rigidbody enemBod;

    public Vector3 tempVar;

    [Header("Movement")]
    public float maxVel = 5;
    public float rotSpeed = 1;
    public float enemMoveSpeed = 25.0f;
    public float enemStrafeSpeed = 10.0f;

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

    public bool canAction = false;
    public Vector2 RandActionInterval;

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

    public bool takingAction = false; //if the enemy is taking a tactical action

    [Header("Awareness AI")]
    public bool AvoidHazards = true;
    public float distanceFromHazard = 4.0f;
    public float FOVRadius = 10; //Detect Range
    [Range(0, 360)]
    public float FOVAngle = 45;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool playerSpotted;
    public Vector2 ChaseRange;
    public Vector2 DesiredDistanceFromTarget;

    [Header("Other")]
    public Animator enemyMoveAnim;

    // Use this for initialization
    void Start ()
    {
        FlockingTarget = GameObject.FindGameObjectWithTag("Player");
        enemBod = GetComponent<Rigidbody>();
        StartCoroutine(FinderRoutine());
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
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, FOVRadius, targetMask);

        if (rangeCheck.Length != 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - enemyOrientation.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < FOVAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(enemyOrientation.position, target.position);

                if (!Physics.Raycast(enemyOrientation.position, directionToTarget, distanceToTarget, obstructionMask))
                    playerSpotted = true;
                else playerSpotted = false;
            }
            else
            {
                playerSpotted = false;
            }
        }
        else if (playerSpotted)
        {
            playerSpotted = false;
        }
    }

    private IEnumerator ActionTimer()
    {
        float randInterval = Random.Range(RandActionInterval.x, RandActionInterval.y);

        WaitForSeconds wait = new WaitForSeconds(randInterval);
        takingAction = false;

        yield return null;
    }
    // Update is called once per frame
    void Update ()
    {
        Vector3 desiredDirection = new Vector3();

        Vector3 vectorToTarget = FlockingTarget.transform.position - transform.position;
        float distanceToTarget = vectorToTarget.magnitude;

        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        if (playerSpotted)
        {
            CurrentFlockingMode = FlockingMode.EngageTarget;
        }

        if (canAction == false)
            StopCoroutine(ActionTimer());

        if (CurrentAttackState == AttackState.circleAround)
            rotSpeed = 1000;
        else
            rotSpeed = 5;

        switch (CurrentFlockingMode)
        {
            case FlockingMode.Idle:

                break;
            case FlockingMode.EngageTarget:

                StartCoroutine(ActionTimer());
                //======================================================================================================================================================

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

                            if(distanceToTarget >= DesiredDistanceFromTarget.y)
                            {
                                ForwardThrust(enemMoveSpeed);
                            }
                            else if(distanceToTarget <= DesiredDistanceFromTarget.x)
                            {
                                ForwardThrust(-enemMoveSpeed);
                            }

                            break;
                        case AttackState.circleAround:

                            StrafeThrust(enemStrafeSpeed);

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
        
        if(AvoidHazards)
        {

            HazardScript[] hazards = FindObjectsOfType<HazardScript>();

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

            if(avoidanceVector != Vector3.zero)
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
        Vector3 force = enemyOrientation.right * enemStrafeSpeed;
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

    private void Jump(float strafeDirection, float jumpAmount)
    {
        enemyMoveAnim.SetTrigger("JumpAnim");

        Vector3 jumpForce = enemyOrientation.up * jumpAmount * 100;

        enemBod.AddForce(jumpForce);
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
