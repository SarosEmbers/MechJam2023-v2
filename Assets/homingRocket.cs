using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingRocket : MonoBehaviour
{
    private Rigidbody rb;
    public float velSpeed, accelRate, rockVel, maxRoxkVel;
    public float rotSpeed, rayRange;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float randX = Random.Range(-25, 25);
        float randy = Random.Range(0, 25);
        float randz = Random.Range(0, 25);

        rb.AddForce(new Vector3(randX, randy, randz));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 turnToTarget = Vector3.zero;
        if (target != null)
        {
            turnToTarget = target.transform.position - transform.position;
        }
        rockVel = rockVel + (accelRate * Time.deltaTime);

        if (rb.velocity.magnitude <= maxRoxkVel)
        {
            rb.velocity = transform.forward * rockVel;
        }
        RotateRocket(turnToTarget);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayRange))
        {
            if(hit.transform.tag == "Enemy")
            {
                Destroy(hit.transform.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    private void RotateRocket(Vector3 targetLoc)
    {
        Quaternion bodRot = Quaternion.LookRotation(targetLoc);
        transform.rotation = Quaternion.Lerp(transform.rotation, bodRot, rotSpeed * Time.deltaTime);

        Debug.DrawRay(transform.position, targetLoc, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player")
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy")
        {
            Debug.Log(rb.velocity.magnitude);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
