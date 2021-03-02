using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Range(0,1)]
    public float smoothing;

    private Vector3 offset;

    private void Start()
    {
        offset = target.position - transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.position - offset,
            smoothing
        );
    }
}
