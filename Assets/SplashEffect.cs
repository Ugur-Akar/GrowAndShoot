using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SplashEffect : MonoBehaviour
{
    //Settings

    public float minSize;
    public float maxSize;
    public Vector3 minOffset;
    public Vector3 maxOffset;
    public float minRotation;
    public float maxRotation;
    public int rotationAxisIndex=2; // Default rotation axis is Z
    public float minAlpha;
    public float maxAlpha;
    // Connections
    SpriteRenderer spriteRenderer;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitState();
    }
    void InitConnections(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Randomize()
    {
        float size = Random.Range(minSize, maxSize);
        Vector3 offset = GetRandomVector(Vector3.zero, Vector3.zero);
        float rotation = Random.Range(minRotation, maxRotation);
        transform.localScale *= size;
        transform.position += offset;
        Vector3 rotationEuler = Vector3.zero;
        rotationEuler[rotationAxisIndex] = rotation;
        transform.Rotate(rotationEuler);
        float alpha = Random.Range(minAlpha, maxAlpha);
        if (spriteRenderer == null) InitConnections();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }

    Vector3 GetRandomVector(Vector3 minVector, Vector3 maxVector)
    {
        Vector3 output = new Vector3(Random.Range(minVector.x, maxVector.x),
                                        Random.Range(minVector.y, maxVector.y),
                                        Random.Range(minVector.z, maxVector.z));
        return output;
    }

}

