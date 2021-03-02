using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody thisRB;

    private bool onGround = true;

    public float jumpModifier = 50f;
    public float speed = 1f;
    public float movementSmoothing = 0.1f;

    public float inputX, inputZ;

    private void Start()
    {
        thisRB = GetComponent<Rigidbody>();
    }

    public void MoveZ(float input) => AccelerateOnAxis(Vector3.forward * input);
    public void MoveX(float input) => AccelerateOnAxis(Vector3.right * input);

    public void AccelerateOnAxis(Vector3 axis)
    {
        thisRB.AddForce(axis * speed, ForceMode.Acceleration);
    }

    private void FixedUpdate()
    {
        thisRB.velocity =
            new Vector3(
                Mathf.Clamp(thisRB.velocity.x, -speed, speed),
                thisRB.velocity.y,
                Mathf.Clamp(thisRB.velocity.z, -speed, speed)
            );
    }

    public void Jump(float input)
    {
        if (onGround)
        {
            thisRB.AddForce(Vector3.up * jumpModifier, ForceMode.Impulse);
            onGround = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor" && !onGround)
            onGround = true;
    }
}
