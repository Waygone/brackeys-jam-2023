using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Grid movementGrid;
    private Coroutine movementCoroutine = null;

    private void Start()
    {
        movementGrid = GameObject.FindGameObjectWithTag("NpcMoveGrid").GetComponent<Grid>();
    }

    public void Move(MovementDirection moveDir, int squaresToMove)
    {
        var squareSize = movementGrid.cellSize;

        var moveVector = moveDir switch
        {
            MovementDirection.NORTH => new Vector2(0, squareSize.y),
            MovementDirection.SOUTH => new Vector2(0, -squareSize.y),
            MovementDirection.WEST => new Vector2(-squareSize.x, 0),
            MovementDirection.EAST => new Vector2(squareSize.x, 0),
            _ => Vector2.zero,
        };

        movementCoroutine = StartCoroutine(Movement(moveVector, squaresToMove));
    }

    private IEnumerator Movement(Vector3 moveVector, int squaresToMove)
    {
        for (int i = 0; i < squaresToMove; i++)
        {
            var targetLocation = transform.position + moveVector;

            while (transform.position != targetLocation)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetLocation, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        movementCoroutine = null;
    }
}

public enum MovementDirection
{
    NORTH,
    SOUTH,
    WEST,
    EAST
}
