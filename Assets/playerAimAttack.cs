using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAimAttack : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Aiming")]
    public Transform orientation, reticlePos;
    public float maxLockOnDistance = 25;
    public LayerMask obstructionMask;

    [Header("Attacking")]
    public bool isAttacking = false;
    public enum StolenArms
    {
        Beefy,
        Speedy,
        Hover,
        Player36
    }

    [Header("Scrounge")]

    public StolenArms LeftEquppedArm = StolenArms.Player36;
    public StolenArms RightEquppedArm = StolenArms.Player36;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(reticlePos.position, reticlePos.forward, out hit, maxLockOnDistance))
        {
            Debug.Log(hit.transform.name);
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("SMACKED: " + hit.transform.name);

            }
        }
    }

    public void FireArm()
    {

    }
}
