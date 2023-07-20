using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
     GameObject player;
    public GameObject elevatorSpawn;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    

     void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;

        if (other.gameObject.tag == "Player")
        {
            if (sceneName == "Level1Scene")
            {
                SceneManager.LoadScene("Level2Scene");
            }
            else if (sceneName == "Level2Scene")
            {
                SceneManager.LoadScene("Level3Scene");
            }
            else if(sceneName == "Level3Scene")
            {
                player.transform.position = elevatorSpawn.transform.position;
                player.transform.rotation = elevatorSpawn.transform.rotation;

            }
        }


    }
}
