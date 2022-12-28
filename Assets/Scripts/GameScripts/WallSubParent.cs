using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;


public class WallSubParent : MonoBehaviour
{
    //Settings
    public float bulletDelay = 0.5f;
    // Connections
    public List<ObstacleRevised> cubes;

    public event Action WallDestroyedByBullets;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitState();
    }
    void InitConnections(){
        for(int i = 0; i < cubes.Count; i++)
        {
            cubes[i].CubeLost += CubeLost;
        }
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            DecreaseHealthOfLastCube();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            DestroyAllCubes();
        }
    }

    void DecreaseHealthOfLastCube()
    {
        if(cubes.Count >= 1)
        cubes[0].DecreaseHealth();
        cubes[0].ColorChange();
        cubes[0].GetComponent<SwellEffect>().ApplyEffect(shrinkBack: true);
        
    }

    void CubeLost()
    {
        ObstacleRevised cubeToDelete;
        cubeToDelete = cubes[0];
        cubes[0].Explode();

        cubes.RemoveAt(0);
        Destroy(cubeToDelete.gameObject);
        foreach(Transform child in transform)
        {
        transform.DOMoveY(transform.position.y - 2, bulletDelay);
        }
        if(cubes.Count == 0)
        {
            WallDestroyedByBullets();
            Destroy(gameObject);
        }
    }

    void DestroyAllCubes()
    {
        if(cubes.Count > 0)
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].Explode();
            }
            Destroy(gameObject);
        }
    
    }
}
