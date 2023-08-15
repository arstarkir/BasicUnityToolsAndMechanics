using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    Rigidbody rb;
    float speed;
    public Transform cam;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
            if (Input.GetKey(KeyCode.LeftShift))
                speed = 16f;
            else if (Input.GetKey(KeyCode.LeftControl))
                speed = 8f;
            else
                speed = 12f;

            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
            SpeedControl();      
    }
    void FixedUpdate()
    {
            Move();
    }
    private void IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.5f))
            isGrounded = true;
        else
            isGrounded = false;
    }
    private void Move()
    {
        IsGrounded();
        Vector3 moveDirection = cam.forward * Input.GetAxisRaw("Vertical") + cam.right * Input.GetAxisRaw("Horizontal");
        if (isGrounded)
            rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
        else if (!isGrounded)
            rb.AddForce(moveDirection.normalized * speed * 0.4f, ForceMode.Force);
    }
    private void Jump()
    {
        IsGrounded();
        if (isGrounded)
        rb.AddForce(Vector3.up * 6, ForceMode.Impulse);
    }

    private void SpeedControl()
    {
        Vector3 maxVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (maxVel.magnitude > speed)
        {
            Vector3 limitedVel = maxVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
