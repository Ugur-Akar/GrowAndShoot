using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class PlayerMovement : MonoBehaviour, SimpleUserTapInputListener
{
    public float speed = 10;
    public float roadBorder = 2;
    bool canMove = true;
    bool canSwerve = true;
    SplineFollower follower;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SimpleUserTapInput>().SetListener(this);
        follower = GetComponent<SplineFollower>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnDirectiveTap(Vector2 direction)
    {
        if (canSwerve)
        {
            direction.y = 0;
            if (Mathf.Abs(follower.motion.offset.x + direction.x) < roadBorder)
                follower.motion.offset += (direction);
        }
    }

    public void Disable()
    {
        canMove = false;
        canSwerve = false;
    }

    public void Enable()
    {
        canMove = true;
        canSwerve = true;
    }
}