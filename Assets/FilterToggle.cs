using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterToggle : MonoBehaviour
{
    public GameObject mainCam, renderedCam;
    public RenderTexture pixelatedFilter;
    public bool isOldTyme = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void renderTextureMode(bool onOrOff)
    {
        if(onOrOff == false)
        {
            renderedCam.SetActive(false);
            mainCam.GetComponent<Camera>().targetTexture = null;
        }
        else
        {
            renderedCam.SetActive(true);
            mainCam.GetComponent<Camera>().targetTexture = pixelatedFilter;
        }
    }
}
