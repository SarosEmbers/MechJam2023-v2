using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSound3d : MonoBehaviour
{
    public AudioSource audioSource;
    public bool started = false;
    // Update is called once per frame
    void Update()
    {
        if (started && audioSource.isPlaying == false) Destroy(this.gameObject);
    }
}
