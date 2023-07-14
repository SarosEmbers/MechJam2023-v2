using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdPersonController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 10f;
    private float gravityValue = -9.81f;
    private Animator animator;

    public Transform cam;
    public Transform spawner;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;  // meters per second
    public string damageObjectWithTag = "Enemy";

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            FireProjectile();
        }

        float forwardMovement = Input.GetAxis("Vertical");
        //animator.SetFloat("ForwardMovement", forwardMovement);

        float sideMovement = Input.GetAxis("Horizontal");
        //animator.SetFloat("SideMovement", sideMovement);

        Vector3 camForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        Vector3 moveForward = camForward * forwardMovement;
        Vector3 moveSide = cam.transform.right * sideMovement;
        Vector3 moveDir = Vector3.Normalize(moveForward + moveSide);

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
        }

        Vector3 move = moveDir * playerSpeed * Time.deltaTime;
        playerVelocity.x = move.x;
        playerVelocity.z = move.z;

        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity);
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawner.position, spawner.rotation);
        projectile.GetComponent<Rigidbody>().velocity = spawner.forward * projectileSpeed;
    }

}
