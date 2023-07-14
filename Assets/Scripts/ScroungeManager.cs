using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScroungeManager : MonoBehaviour
{
    public MeshFilter Torso, ArmL, ArmR, LegL, LegR;

    [Header("Body Parts")]
    public Mesh[] TorsoMesh, ArmLMesh, ArmRMesh, LegLMesh, LegRMesh;

    [Header("Other")]
    public GameObject placeholderItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            randomizeParts();
        }
    }

    public void randomizeParts()
    {
        int a = Random.Range(0, 3);
        Torso.mesh = TorsoMesh[a];
        int b = Random.Range(0, 3);
        ArmL.mesh = ArmLMesh[b];
        int c = Random.Range(0, 3);
        ArmR.mesh = ArmRMesh[c];
        int i = Random.Range(0, 3);
        LegL.mesh = LegLMesh[i];
        LegR.mesh = LegRMesh[i];
    }

    public void changePart(string partToChange)
    {

    }
}
