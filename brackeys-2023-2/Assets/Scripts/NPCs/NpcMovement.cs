using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Grid movementGrid;
    private Coroutine movementCoroutine = null;

    private NpcMovePath queuedNpcMovePath = null;

    private void OnEnable()
    {
        movementGrid = GameObject.FindGameObjectWithTag("NpcMoveGrid").GetComponent<Grid>();
    }

    public void Move(NpcMovePath movePath)
    {
        if (movementCoroutine == null)
        {
            movementCoroutine = StartCoroutine(Movement(movePath));
        } else
        {
            queuedNpcMovePath = movePath;
        }
    }

    private IEnumerator Movement(NpcMovePath movePath)
    {
        var loop = movePath.Loop;

        do
        {
            var startPosition = new Vector3(movePath.StartPosition.startPosition.x, movePath.StartPosition.startPosition.y);
            if (movePath.StartPosition.hasStartPosition && transform.position != startPosition)
            {
                transform.position = startPosition;
            }
            MovementDirection? preMoveDir = null;
            foreach (MovementDirection moveDir in movePath.PathDirections)
            {
                if (preMoveDir != null && moveDir != preMoveDir)
                {
                    yield return new WaitForSeconds(movePath.TimeWaitAtEachTurn);
                }

                var moveVector = GetMoveVector(moveDir);
                var targetLocation = transform.position + moveVector;

                while (transform.position != targetLocation)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetLocation, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(movePath.TimeWaitAtEachPosition);
                preMoveDir = moveDir;
            }
        } while (queuedNpcMovePath == null && loop);

        movementCoroutine = null;

        if (queuedNpcMovePath != null)
        {
            Move(queuedNpcMovePath);
        }
    }

    private Vector3 GetMoveVector(MovementDirection moveDir)
    {
        var squareSize = movementGrid.cellSize;

        return moveDir switch
        {
            MovementDirection.NORTH => new Vector2(0, squareSize.y),
            MovementDirection.SOUTH => new Vector2(0, -squareSize.y),
            MovementDirection.WEST => new Vector2(-squareSize.x, 0),
            MovementDirection.EAST => new Vector2(squareSize.x, 0),
            _ => Vector2.zero,
        };
    }
}

public enum MovementDirection
{
    NORTH,
    SOUTH,
    WEST,
    EAST
}
