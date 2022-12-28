using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrowShrinkEffect : MonoBehaviour
{
    //Settings

    // Connections
    public Transform footTip;
    public Transform[] scaleableBodyParts;
    // State Variables
    float footTipInitialHeight;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        footTipInitialHeight = footTip.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetScale(0.5f); 
        }
    }

    public void SetScale(float scale)
    {
        for (int i = 0; i < scaleableBodyParts.Length; i++)
        {
            Vector3 currentScale = scaleableBodyParts[i].localScale;
            currentScale.y = scale;
            scaleableBodyParts[i].localScale = currentScale;
        }

        float currentFootTipHeight = footTip.position.y;
        transform.position += Vector3.down * (currentFootTipHeight - footTipInitialHeight);

    }
}

