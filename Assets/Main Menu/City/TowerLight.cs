using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLight : MonoBehaviour
{
    public Light towerLight;
    void Update()
    {
        towerLight.intensity = Mathf.PingPong(3, 50 * Time.deltaTime);
    }
}
