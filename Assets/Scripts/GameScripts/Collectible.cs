using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collectible : MonoBehaviour
{
    //Settings
    private float score;
    public bool onWall;
    public float scoreCoef;
    // Connections
    CollectibleMoveToPlayer otherCollectibleScript;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections(){
        otherCollectibleScript = GetComponent<CollectibleMoveToPlayer>();
    }
    void InitState(){

        score = transform.localScale.x + transform.localScale.y + transform.localScale.z * scoreCoef;

        if (onWall)
        {
            otherCollectibleScript.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetScore()
    {
        return score;
    }
}
