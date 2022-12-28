using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RewardThrower : MonoBehaviour
{
    //Settings
    public Vector3 throwForce;
    // Connections
    public GameObject rewardGO;
    CubeObstacle obstacle;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitState();
    }
    void InitConnections(){
        obstacle = GetComponent<CubeObstacle>();
        obstacle.OnExploded += OnObstacleExploded;
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnObstacleExploded()
    {
        if(rewardGO != null)
            rewardGO.GetComponent<Rigidbody>().AddForce(throwForce);
      
    }
}

