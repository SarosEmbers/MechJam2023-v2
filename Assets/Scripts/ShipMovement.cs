using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform orientation;

    public float maxVelocity = 5;
    public float accelRateForward = 5;
    public float accelRateStrafe = 5;

    public float rotationSpeed = 3;

    public float jumpAmount = 3;
    public float fallSpeed = 3;
    public float maxFallSpeed = 3;
    public bool jumped = false;

    public Animator playerMoveAnim;

    #region Monobehavior API

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // These Axis values are not accurate whatsoever lmao
    void FixedUpdate()
    {
        float zAxis = Input.GetAxisRaw("Vertical");
        float xAxis = Input.GetAxisRaw("Horizontal");

        ThrustForward(zAxis);
        ThrustStrafe(xAxis);

        int horizInt = Mathf.RoundToInt(xAxis);
        int vertInt = Mathf.RoundToInt(zAxis);

        playerMoveAnim.SetInteger("HorizontalAnim", horizInt);
        playerMoveAnim.SetInteger("VerticalAnim", vertInt);

        //Rotate(transform, xAxis * rotationSpeed);
        /*
        if (INPUT CAMO BUTTON)
        {
            camo();
        }
        */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumped = true;
            Debug.Log("JUMP");
            Jump(0, jumpAmount);
        }

        if (jumped)
        {
            //Vector3 force = fallSpeed;

            if (rb.velocity.y < 5)
            {
                if (rb.velocity.y > maxFallSpeed)
                {
                    rb.velocity = new Vector3(rb.velocity.x, maxFallSpeed, rb.velocity.z);
                }
                else
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - fallSpeed, rb.velocity.z);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION: " + collision.gameObject.name);

        if (collision.gameObject.tag == "Ground" && jumped == true)
        {
            jumped = false;
        }
    }

    #endregion

    #region Maneuvering API

    private void ClampVelocity()
    {
        float z = Mathf.Clamp(rb.velocity.z, -maxVelocity, maxVelocity);
        float x = Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity);

        rb.velocity = new Vector3(x, rb.velocity.y, z);

    }

    private void ThrustForward(float amount)
    {
        Vector3 force = orientation.forward * maxVelocity * amount * accelRateForward;

        rb.AddForce(force);
    }

    private void ThrustStrafe(float amount)
    {
        Vector3 force = orientation.right * maxVelocity * amount * accelRateStrafe;
        rb.AddForce(force);
    }

    private void Rotate(Transform t, float amound)
    {
        t.Rotate(0, amound, 0);
    }
    public void CallRotL()
    {
        float xAxis = 25.0f;
        Rotate(transform, xAxis * rotationSpeed);
    }
    public void CallRotR()
    {
        float xAxis = -25.0f;
        Rotate(transform, xAxis * rotationSpeed);
    }
    private void Jump(float strafeDirection, float jumpAmount)
    {
        Vector3 jumpForce = orientation.up * jumpAmount * 100;

        rb.AddForce(jumpForce);
    }
    #endregion
}
