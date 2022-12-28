using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;

public class LevelManager : MonoBehaviour
{
    //Settings

    // Connections
    public List<EnemyControl> enemies;
    public SplineComputer splineComputer;
    //public GameObject endGameCubesParent;
    public Transform endGameCubesParent;
    public List<Collider> endGameCubeColliders;
    public Action OnCubeRemoved;
    public GateManager gateManager;
    public GameObject collectibleParent;
    public Transform endTarget;
    public Transform enemyParent;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitState();

        
    }
    void InitConnections(){
        
        endGameCubeColliders  = new List<Collider>();
        for(int i=0; i< endGameCubesParent.childCount; i++)
        {
            GameObject endGameCubeGO = endGameCubesParent.GetChild(i).gameObject;
            endGameCubeGO.GetComponent<EndGameCube>().RemoveCube += RemoveCube;
            endGameCubeColliders.Add(endGameCubeGO.GetComponent<Collider>());
        }

        
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockEndGame()
    {
        foreach(Collider col in endGameCubeColliders)
        {
            col.enabled = true;
        }
    }

    void RemoveCube(GameObject cube)
    {
        endGameCubeColliders.Remove(cube.GetComponent<Collider>());
        OnCubeRemoved();
    }

    public void AssignPlayer(GameObject player)
    {
        for(int i = 0; i < collectibleParent.transform.childCount; i++)
        {
            collectibleParent.transform.GetChild(i).GetComponent<CollectibleMoveToPlayer>().player = player;
        }
    }


    public void AddEnemiesToList()
    {
        for (int i = 0; i < enemyParent.childCount; i++)
        {
            enemies.Add(enemyParent.GetChild(i).GetComponent<EnemyControl>());
        }
    }
}
