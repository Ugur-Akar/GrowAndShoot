using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBulletManager : MonoBehaviour
{
    //Settings
    public Vector3 vectorToPlayer;
    public float speed;
    public float timeLimit;
    // Connections

    // State Variables
    float timeSinceStart;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        timeSinceStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        if(timeSinceStart >= timeLimit)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
    
}
