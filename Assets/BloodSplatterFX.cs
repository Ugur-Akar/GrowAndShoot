using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BloodSplatterFX : MonoBehaviour
{
    const int DEFAULT_N_JUMPS = 1;
    //Settings
    public float minJumpHeight;
    public float maxJumpHeight;
    public float dropDuration;
    // Connections
    public Transform bloodDropTransform;
    public GameObject splashPrefab;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SplashBlood(Vector3 position)
    {
        float jumpPower = Random.Range(minJumpHeight, maxJumpHeight);
        bloodDropTransform.DOJump(position, jumpPower, DEFAULT_N_JUMPS, dropDuration)
            .OnComplete(()=>CreateSplatter(position));
        
    }

    void CreateSplatter(Vector3 position)
    {
        GameObject splashGO = Instantiate(splashPrefab, position, Quaternion.identity);
        SplashEffect splashEffect = splashGO.GetComponent<SplashEffect>();
        splashEffect.Randomize();
    }
}

