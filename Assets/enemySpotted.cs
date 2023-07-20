using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemySpotted : MonoBehaviour
{
    public GameObject targettedPrefab;
    protected Image targetHighlight;
    private GameObject playerHUD;
    public Transform playerReticle;
    private bool isTargetted = false;
    // Start is called before the first frame update
    void Start()
    {
        playerHUD = GameObject.Find("HUD Canvas");
        playerReticle = GameObject.Find("Player Reticle").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorToEnemy = Camera.main.transform.position - transform.position;
        Vector3 vectorToHUD = Camera.main.transform.position - playerReticle.transform.position;
        Vector3 finalEnemyCam = Camera.main.transform.position - Vector3.ClampMagnitude(vectorToEnemy, vectorToHUD.magnitude);

        //Debug.DrawRay(Camera.main.transform.position, finalEnemyCam, Color.red);
        if(isTargetted)
        {
            targetHighlight.transform.position = finalEnemyCam;
        }
    }

    public void enemySelected()
    {
        if(!isTargetted)
        {
            targetHighlight = Instantiate(targettedPrefab, GameObject.Find("HUD Canvas").transform).GetComponent<Image>();
            isTargetted = true;
            Destroy(targetHighlight, 5);
        }
    }

    public void deselectEnemy()
    {
        Destroy(targetHighlight);
        isTargetted = false;
    }
}
