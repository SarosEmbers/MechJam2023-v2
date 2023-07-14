using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    Vector3 offset;
    public Transform target;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (target == null) return;

        transform.position = target.position + offset;
    }
}
