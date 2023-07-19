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
    private playerAimAttack paa;

    // Start is called before the first frame update
    void Start()
    {
        paa = GetComponent<playerAimAttack>();
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
        int a = Random.Range(0, 4);
        Torso.mesh = TorsoMesh[a];
        int b = Random.Range(0, 4);
        ArmL.mesh = ArmLMesh[b];
        int c = Random.Range(0, 4);
        ArmR.mesh = ArmRMesh[c];
        int i = Random.Range(0, 4);
        LegL.mesh = LegLMesh[i];
        LegR.mesh = LegRMesh[i];
    }

    public void changePart(string partToChange, int changePartToIndex)
    {
        Debug.Log("SCROUNGE: " + partToChange + " || " + changePartToIndex);
        switch (partToChange)
        {
            case "LArm":
                ArmL.mesh = ArmLMesh[changePartToIndex];
                break;
            case "RArm":
                ArmR.mesh = ArmRMesh[changePartToIndex];
                break;
            case "Legs":
                LegL.mesh = LegLMesh[changePartToIndex];
                LegR.mesh = LegRMesh[changePartToIndex];
                break;
        }
        Debug.Log("SCROUNGE: " + paa.LeftEquppedArm + " || " + paa.RightEquppedArm);
    }
}
