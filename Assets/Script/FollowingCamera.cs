using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{

    public GameObject targetObject;

    private float DistanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        DistanceToTarget = transform.position.x - targetObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float targetObjectX = targetObject.transform.position.x;
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectX + DistanceToTarget;
        transform.position = newCameraPosition;
    }
}
