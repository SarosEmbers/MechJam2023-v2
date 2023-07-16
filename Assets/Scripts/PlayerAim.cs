using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform reticle;
    public float maxRange = 10;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(reticle.position, reticle.forward, out hit, maxRange))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<enemySpotted>().enemySelected();
                Debug.Log("ENEMY SPOTTED: " + hit.transform.name);
            }
        }
        //Debug.DrawRay(transform.position, transform.forward * maxRange);
    }
}
