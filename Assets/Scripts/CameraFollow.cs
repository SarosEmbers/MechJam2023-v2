using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.25f;

    Vector3 currentVelocity;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            //transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, smoothTime);
            //transform.position = target.position + target.forward + offset;

            transform.position = Vector3.SmoothDamp(transform.position, target.position + target.forward + offset, ref currentVelocity, smoothTime);
            transform.forward = target.transform.position;
        }
    }
}
