using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwellEffect : MonoBehaviour
{
    //Settings
    public float swellTime = 1.0f;
    public float shrinkTime = 0.3f;
    public float initialSize = 0.0f;
    public float endSize = 1.0f;
    // Connections

    // State Variables
    Vector3 initialScale;
    Tween swellTween;
    bool isOscillating;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        initialScale = transform.localScale;
        transform.localScale = initialScale * initialSize;
        isOscillating = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyEffect(bool shrinkBack = false)
    {
        if(swellTween != null)
        {

            swellTween.Kill();
            transform.localScale = initialSize * initialScale;
        }
        swellTween = transform.DOScale(endSize * initialScale, swellTime).SetEase(Ease.OutElastic);
        if (shrinkBack)
        {
            swellTween.OnComplete(
                () => transform.DOScale(initialSize * initialScale, shrinkTime));
        }
    }

    public void Oscillate()
    {
        StartCoroutine(OscillateCoroutine());
    }

    IEnumerator OscillateCoroutine()
    {
        while (isOscillating)
        {
            yield return new WaitForSeconds(swellTime + shrinkTime);
            ApplyEffect(shrinkBack: true);
        }
    }
}

