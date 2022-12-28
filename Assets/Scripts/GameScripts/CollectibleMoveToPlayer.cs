using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectibleMoveToPlayer : MonoBehaviour
{
    //Settings
    public float speed;
    // Connections
    public GameObject player;
    public GameObject wallSubParent;
    // State Variables
    bool isAllowedToMove;
    Vector3 dirVector;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitState();
    }
    void InitConnections(){
        wallSubParent.GetComponent<WallSubParent>().WallDestroyedByBullets += OnWallDestroyed;
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isAllowedToMove)
        {
            dirVector = player.transform.position - transform.position;
            dirVector /= dirVector.magnitude;
            transform.Translate(dirVector * speed * Time.deltaTime);
        }
    }


    void OnWallDestroyed()
    {
        isAllowedToMove = true;
    }

}
