using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAimControll : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;
    public float rotationOffset;

    public Transform behindHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //rotate orientation
        Vector3 viewdir = player.position - new Vector3(behindHolder.position.x, player.position.y, behindHolder.position.z);
        orientation.forward = viewdir.normalized;

        //Player Look
        Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(behindHolder.position.x, combatLookAt.position.y, behindHolder.position.z);
        orientation.forward = dirToCombatLookAt.normalized;

        playerObj.forward = dirToCombatLookAt.normalized;
    }
}
