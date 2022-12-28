using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ObstacleRevised : MonoBehaviour
{
    //Settings
    public float health;
    public Vector3 onDamagedColor = new Vector3(0.2f, 0.2f, 0.2f);
    // Connections
    public GameObject explodingCube;
    ColorChanger colorChanger;
    

    public event Action CubeLost;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitState();
    }
    void InitConnections(){
        colorChanger = GetComponent<ColorChanger>();
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Explode();
            CubeLost();          
        }
    }

    public void Explode()
    {
        GameObject newCube = Instantiate(explodingCube, transform.position, Quaternion.identity);
    }

    public void ColorChange()
    {
        colorChanger.ChangeColor(new Vector3(1, 1, 1));
    }

    public void DecreaseHealth()
    {
        health--;
    }
}
