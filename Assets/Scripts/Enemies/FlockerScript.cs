using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerScript : MonoBehaviour
{
    private Rigidbody enemBod;

    [Header("Movement")]
    public float maxVel = 5;
    public float rotSpeed = 1;
    public float ThrustSpeed = 1.0f;

    public Transform enemyOrientation;
    public GameObject LArm, RArm;

    public enum FlockingMode
    {
        ChaseTarget,
        TurretTarget,
        MaintainDistance,
        DoNothing,
        Idle
    }

    [Header("Attack AI")]
    public FlockingMode CurrentFlockingMode = FlockingMode.ChaseTarget;
    public GameObject FlockingTarget;
    public float DesiredDistanceFromTarget_Min = 10.0f;
    public float DesiredDistanceFromTarget_Max = 15.0f;

    public float aimSpeed = 15.0f;

    public float DetectRange = 25.0f;
    public bool spotted;
    public float ChaseRange = 45.0f;

    [Header("Navigation AI")]
    public bool AvoidHazards = true;

    [Header("Other")]
    public Animator enemyMoveAnim;

    // Use this for initialization
    void Start ()
    {
        FlockingTarget = GameObject.FindGameObjectWithTag("Player");
        enemBod = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 desiredDirection = new Vector3();

        Vector3 vectorToTarget = FlockingTarget.transform.position - transform.position;
        float distanceToTarget = vectorToTarget.magnitude;

        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        switch (CurrentFlockingMode)
        {
            case FlockingMode.ChaseTarget:


                if (distanceToTarget <= DetectRange)
                {

                    spotted = true;

                    RotateTowardsPlayer();

                    if (distanceToTarget <= ChaseRange)
                    {
                        ForwardThrust(ThrustSpeed);
                    }
                    /*
                    if (FlockingTarget.gameObject.GetComponent<WCamo>().isCloaked == true)
                    {
                        spotted = false;
                    }
                    else
                    {
 
                    }
                    */
                }
                else if (distanceToTarget >= DetectRange)
                {
                    spotted = false;
                }

                break;
            case FlockingMode.TurretTarget:

                if (distanceToTarget <= DetectRange)
                {
                    spotted = true;

                    RotateTowardsPlayer();
                    /*
                    if (FlockingTarget.gameObject.GetComponent<WCamo>().isCloaked == true)
                    {
                        spotted = false;
                    }
                    else
                    {

                    }
                    */
                }
                else if (distanceToTarget >= DetectRange)
                {
                    spotted = false;
                }

                break;
            case FlockingMode.MaintainDistance:
                {
                    if (distanceToTarget <= DetectRange)
                    {
                        spotted = true;

                        RotateTowardsPlayer();

                        if (distanceToTarget <= ChaseRange)
                        {
                            if (distanceToTarget <= DesiredDistanceFromTarget_Min)
                            {
                                ForwardThrust(-1.0f);
                            }
                            else if (distanceToTarget >= DesiredDistanceFromTarget_Max)
                            {
                                ForwardThrust(1.0f);
                            }
                        }
                        /*
                        if (FlockingTarget.gameObject.GetComponent<WCamo>().isCloaked == true)
                        {
                            spotted = false;
                        }
                        else
                        {

                        }
                        */
                    }
                    else if (distanceToTarget >= DetectRange)
                    {
                        spotted = false;
                    }

                }
                break;
            case FlockingMode.DoNothing:
                break;
            case FlockingMode.Idle:
                spotted = false;
                break;
        }

        if(AvoidHazards)
        {

            HazardScript[] hazards = FindObjectsOfType<HazardScript>();

            Vector3 avoidanceVector = Vector3.zero;
            for (int i = 0; i < hazards.Length; ++i)
            {
                Vector3 vectorToHazard = hazards[i].transform.position - transform.position;
                if (vectorToHazard.magnitude < 4.0f)
                {
                    Vector3 vectorAwayFromHazard = -vectorToHazard;
                    //LAB TASK #3: Implement hazard avoidance, part 1
                    //TODO: Accumulate vectors away from hazards in avoidance vector
                    //HINT: This loop runs once for every hazard in the level
                    //HINT: Try setting avoidanceVector to itself plus a vector pointing away from a hazard
                    avoidanceVector += vectorAwayFromHazard;
                }
            }

            if(avoidanceVector != Vector3.zero)
            {
                desiredDirection.Normalize();
                avoidanceVector.Normalize();

                //LAB TASK #4: Implement hazard avoidance, part 2
                //TODO: Set the value of desiredDirection to 50% desiredDirection and 50% avoidanceVector
                //HINT: Set desiredDirection = a mathmatical formula sums half of desiredDirection and half of avoidanceVector

                desiredDirection = desiredDirection * 0.5f + avoidanceVector * 0.5f;
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
}
