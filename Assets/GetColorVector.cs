using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GetColorVector : MonoBehaviour
{
    //Settings

    // Connections
    public Material mat;
    public Shader shader;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitState();
    }
    void InitConnections(){
        mat = GetComponent<Renderer>().material;
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("" + mat.color);
    }
}
