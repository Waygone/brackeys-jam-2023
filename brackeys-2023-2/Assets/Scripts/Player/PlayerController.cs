using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerMoveSpeed = 1f;

    [SerializeField] private Tilemap colliderTilemap;

    [SerializeField] private PlayerInteraction _PlayerInteraction;
    private bool _arePlayerControlsEnabled = true;

    private Coroutine movementCoroutine = null;

    private void Update()
    {
        if (!_arePlayerControlsEnabled)
        {
            return;
        }
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0f)
        {
            Utils.Direction dir = x == -1 ? Utils.Direction.WEST : Utils.Direction.EAST;
            Move(dir);
            _PlayerInteraction.SetInteractionOffset(dir);
        }
        else if (y != 0f)
        {
            Utils.Direction dir = y == -1 ? Utils.Direction.SOUTH : Utils.Direction.NORTH;
            Move(dir);
            _PlayerInteraction.SetInteractionOffset(dir);
        }
    }

    private void Move(Utils.Direction direction)
    {
        Vector3 directionOffset = new Vector2(Utils.Directions[(int)direction][0], Utils.Directions[(int)direction][1]) * (1f / 3f);
        if (movementCoroutine == null && CanMove(directionOffset))
        {
            Vector3 targetPosition = transform.position + directionOffset;
            movementCoroutine = StartCoroutine(Movement(targetPosition));
        }
    }

    private IEnumerator Movement(Vector3 targetPosition)
    {
        float t = 0f;
        Vector3 startPosition = transform.position;
        while (t < 1f)
        {
            t = Math.Clamp(t + Time.deltaTime * playerMoveSpeed, 0f, 1f);
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            transform.position = newPosition;
            yield return null;
        }

        movementCoroutine = null;
    }

    private bool CanMove(Vector3 directionOffset)
    {
        Vector3Int gridPos = colliderTilemap.WorldToCell(transform.position + directionOffset);
        if (colliderTilemap.HasTile(gridPos))
        {
            return false;
        }
        return true;
    }

    public void EnableMoveBox(Vector3 boxPosition, BoxPuzzle.CanMove canMove, out Utils.Direction chosenDirection)
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

            Vector3 offset = boxPosition + new Vector3(Utils.Directions[i][0], Utils.Directions[i][1], 0f) * 0.5f;
            float sqrDistance = (transform.position - offset).sqrMagnitude;
            if (sqrDistance < minDistance)
            {
                minDistanceIndex = i;
                minDistance = sqrDistance;
            }
        }

        transform.position = boxPosition + new Vector3(Utils.Directions[minDistanceIndex][0], Utils.Directions[minDistanceIndex][1], 0f) * 0.5f;
        chosenDirection = (Utils.Direction)minDistanceIndex;
    }
    public void DisableMoveBox(Vector3 boxPosition, Utils.Direction direction)
    {
        transform.position = boxPosition + (new Vector3(Utils.Directions[(int)direction][0], Utils.Directions[(int)direction][1], 0f) * 0.5f) + new Vector3(Utils.Directions[(int)direction][0], Utils.Directions[(int)direction][1]) * (1f / 6f);
        _arePlayerControlsEnabled = true;
    }
}
