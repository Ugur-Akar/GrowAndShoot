using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    //Settings
    Vector3 offset;
    
    // Connections
    public GameObject player;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.LookAt(player.transform);
        Vector3 targetPosition = player.transform.position;
        targetPosition = player.transform.position - offset;
        targetPosition.y = transform.position.y;
        targetPosition.x = transform.position.x;
        transform.position = targetPosition;
    }
}
