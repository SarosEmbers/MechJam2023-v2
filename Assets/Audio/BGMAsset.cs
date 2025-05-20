using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAsset : ScriptableObject
{
    public bool customLoop;
    public float loopEndPoint;
    public float loopLength;
    public AudioClip audioClip;
    public bool canLoop;
}
