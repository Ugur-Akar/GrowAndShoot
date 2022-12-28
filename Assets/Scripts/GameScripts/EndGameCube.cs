using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EndGameCube : MonoBehaviour
{
    //Settings
    public float health;
    // Connections
    public GameObject explodingPart;
    SwellEffect swell;
    public TextMeshPro label;
    public event Action<GameObject> RemoveCube;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections(){
        swell = GetComponent<SwellEffect>();
    }
    void InitState(){
        int cubeIndex = transform.GetSiblingIndex();
        label.text = "x" + (cubeIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            //Explode();
            RemoveCube(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        health--;
        swell.ApplyEffect(true);
        Destroy(other.gameObject);
    }

    void Explode()
    {
        Instantiate(explodingPart, transform.position, Quaternion.identity);
    }
}
