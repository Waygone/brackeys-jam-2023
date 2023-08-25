using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NewPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float forceDamping = 1.2f;
    private Vector2 forceToApply;
    private Vector2 PlayerInput;
    private bool _arePlayerControlsEnabled = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_arePlayerControlsEnabled)
        {
            return;
        }
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        PlayerInput = Vector2.zero;

        if (x != 0f)
        {
            PlayerInput.x = x;
        }
        else if (y != 0f)
        {
            PlayerInput.y = y;
        }
    }
    private void FixedUpdate()
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

    public void EnableMoveBox(Vector2 boxPosition, Vector2 boxScale, BoxPuzzle.CanMove canMove, out Utils.Direction chosenDirection)
    {
        _arePlayerControlsEnabled = false;
        int minDistanceIndex = 0;
        float minDistance = float.MaxValue;
        for (int i = 0; i < Utils.Directions.Length; ++i)
        {
            if ((i == 0 || i == 2) && canMove == BoxPuzzle.CanMove.LR)
            {
                continue;
            }
            if ((i == 1 || i == 3) && canMove == BoxPuzzle.CanMove.UD)
            {
                continue;
            }

            Vector3 offset = boxPosition + new Vector3(Utils.Directions[i][0], Utils.Directions[i][1], 0f) * boxScale;
            float sqrDistance = (transform.position - offset).sqrMagnitude;
            if (sqrDistance < minDistance)
            {
                minDistanceIndex = i;
                minDistance = sqrDistance;
            }
        }

        transform.position = boxPosition + new Vector3(Utils.Directions[minDistanceIndex][0], Utils.Directions[minDistanceIndex][1], 0f) * boxScale;
        chosenDirection = (Utils.Direction)minDistanceIndex;
    }
    public void DisableMoveBox()
    {
        _arePlayerControlsEnabled = true;
    }
}