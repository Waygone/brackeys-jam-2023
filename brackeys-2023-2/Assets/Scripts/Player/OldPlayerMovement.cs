using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OldNewPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float forceDamping = 1.2f;
    private Vector2 forceToApply;
    private Vector2 PlayerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    void FixedUpdate()
    {
        Vector2 moveForce = PlayerInput * moveSpeed;
        moveForce += forceToApply;
        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= moveSpeed && Mathf.Abs(forceToApply.y) <= moveSpeed)
        {
            forceToApply = Vector2.zero;
        }
        rb.velocity = moveForce;
    }
}