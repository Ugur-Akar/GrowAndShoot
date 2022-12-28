using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToonyColorsPro.Runtime;
using ToonyColorsPro;

public class ColorChanger : MonoBehaviour
{
    //Settings
    public float timeLimit = 1;
    // Connections
    public Renderer thisRenderer;
    
    // State Variables
    bool colorChanging = false;
    Color newColor;
    float time = 0;
    Vector3 newColorRGB;
    Vector3 startingColorRGB;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
            startingColorRGB = new Vector3(thisRenderer.material.color.r, thisRenderer.material.color.g, thisRenderer.material.color.b);
    }

    // Update is called once per frame
    void Update()
    {
        if (colorChanging && time <= timeLimit)
        {           
            Vector3 currentColorRGB = Vector3.Lerp(startingColorRGB, newColorRGB, time);
            thisRenderer.material.color = new Color(currentColorRGB.x,currentColorRGB.y,currentColorRGB.z,thisRenderer.material.color.a);
            time += Time.deltaTime/timeLimit;
        }
    }

    public void ChangeColor(Vector3 rgbValues, float alphaValue = 1)
    {
        if (!colorChanging)
        {
            newColor = new Color(rgbValues.x, rgbValues.y, rgbValues.z, alphaValue);

            colorChanging = true;
            newColorRGB = new Vector3(newColor.r, newColor.g, newColor.b);
            thisRenderer.material.SetFloat(3, 0);
        }   
        
    }

}
