using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GateManager : MonoBehaviour
{
    //Settings
    public float xOffset = 5.5f;
    public float openTime = 0.5f;
    // Connections
    public GameObject[] gates;
    // State Variables

    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSesame()
    {
        Debug.Log("Open Sesame!");
        
        gates[0].transform.DOMoveX(-xOffset, openTime);
        gates[1].transform.DOMoveX(xOffset, openTime);
        Invoke(nameof(DestroySplash), openTime);
    }

    void DestroySplash()
    {
        gates[0].GetComponent<GateCollider>().DestroySplash();
        gates[1].GetComponent<GateCollider>().DestroySplash();
    }
}
