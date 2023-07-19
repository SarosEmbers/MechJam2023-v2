using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBar : MonoBehaviour
{
    GameObject player;
    ShipMovement shipMOvement;
    public Image fillBar;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<ShipMovement>().jumped == true)
        {
            Debug.Log("Reading");

        }
        else if (player.GetComponent<ShipMovement>().jumped != true)
        {
          //  fillBar.rectTransform.sizeDelta = Vector2.Lerp(, fillBar.sprite.rect.height - 0.5f * Time.deltaTime);

        }
    }
}
