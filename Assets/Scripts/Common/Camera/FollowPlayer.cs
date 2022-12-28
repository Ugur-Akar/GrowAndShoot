using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool excludeY = false;
    public float cameraChangeLerpFactor = 0.2f;
    // Connections
    public Transform target;
    // State variables
    Vector3 defaultOffset;
    float currentOffsetMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        defaultOffset = transform.position - target.position;
        currentOffsetMultiplier = 1.0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = target.position + currentOffsetMultiplier * defaultOffset;
        if (excludeY)
            targetPosition.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraChangeLerpFactor * Time.deltaTime);
    }

    public void SetOffsetMultiplier(float multiplier)
    {
        currentOffsetMultiplier = multiplier;
    }

}