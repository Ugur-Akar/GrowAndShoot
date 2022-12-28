using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplodingCube : MonoBehaviour
{
    //Settings
    public float forceCoef;
    public float forceMin, forceMax;
    public float timeLimit;
    // Connections
    public List<Rigidbody> parts;
    // State Variables
    float timeSinceStart;
    
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();

        Explode();
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
    }

    void Explode()
    {
        foreach(Rigidbody part in parts)
        {
            Vector3 explosionVector = new Vector3(Random.Range(forceMin, forceMax), Mathf.Abs(Random.Range(forceMin, forceMax*2)), Random.Range(forceMin, forceMax));
            part.AddForce(explosionVector * forceCoef, ForceMode.Impulse);
        }
    }
}
