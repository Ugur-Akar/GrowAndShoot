using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;


public class PlayerSplineControl : MonoBehaviour
{
    //Settings

    // Connections
    public SplineFollower follower;
    public SplineComputer computer;

    public event Action LevelEnd;
    // State Variables
    bool eventSent;
    private void Awake()
    {
        eventSent = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitConnections();
        //InitState();
        StopMoving();
    }
    void InitConnections(){
        follower = GetComponent<SplineFollower>();

        //TODO: computer game manager tarafindan atanacak
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        if(follower.result.percent == 1 && !eventSent)
        {
            LevelEnd();
            eventSent = true;
        }
    }


    public void SetSpeed(float speed)
    {
        follower.followSpeed = speed;
    }

    public void StartMoving()
    {
        if (follower == null) InitConnections();
        follower.follow = true;
    }
    public void StopMoving()
    {
        if (follower == null) InitConnections();
        follower.follow = false;
    }
}
