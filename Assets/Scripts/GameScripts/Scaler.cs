using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scaler : MonoBehaviour
{
    //Settings
    public float minScore, maxScore;
    public bool isHead;
    public bool isBody;
    float minmaxDifference;
    // Connections

    // State Variables
    public float score;
    float percentage;
    float rate;
    Vector3 defaultScale;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        minmaxDifference = maxScore - minScore;
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckScale(score);
    }

    void CheckScale(float _score)
    {
        if(_score >= maxScore)
        {
            SetScale(1);
        }
        else if(_score < maxScore && _score >= minScore)
        {
            float difference = _score - minScore;
            rate = difference / minmaxDifference;
            if(isHead || isBody)
            {
                if(rate <= 0.1f)
                {
                    SetScale(0.1f);
                }
                else
                {
                    SetScale(rate);
                }
            }
            else
            {
                SetScale(rate);
            }
        }
        else
        {
            if (isBody  || isHead)
            {
                SetScale(0.1f);
            }
            else
            {
                SetScale(0);
            }
        }
    }

    void SetScale(float scale)
    {
        if (isHead)
        {
            scale = 1 / scale;
            scale *= 0.7f;
        }
        transform.localScale = defaultScale * scale;
    }
}
