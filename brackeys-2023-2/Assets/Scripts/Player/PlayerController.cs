using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerMoveSpeed = 1f;

    [SerializeField] private Tilemap movementTilemap;
    [SerializeField] private Tilemap colliderTilemap;

    private Coroutine movementCoroutine = null;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            Move(Utils.Direction.NORTH);

        if (Input.GetKey(KeyCode.A))
            Move(Utils.Direction.WEST);

        if (Input.GetKey(KeyCode.S))
            Move(Utils.Direction.SOUTH);

        if (Input.GetKey(KeyCode.D))
            Move(Utils.Direction.EAST);
    }

    private void Move(Utils.Direction direction)
    {
        Vector3 directionOffset = new Vector2(Utils.Directions[(int)direction][0], Utils.Directions[(int)direction][1]);
        if (movementCoroutine == null && CanMove(directionOffset))
        {
            Vector3 targetCell = movementTilemap.WorldToCell(transform.position + directionOffset);
            var targetPosition = targetCell + new Vector3(0.5f, 0.5f);
            movementCoroutine = StartCoroutine(Movement(targetPosition));
        }
    }

    private IEnumerator Movement(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, playerMoveSpeed * Time.deltaTime);
            yield return null;
        }
        movementCoroutine = null;
    }

    private bool CanMove(Vector3 directionOffset)
    {
        Vector3Int gridPos = movementTilemap.WorldToCell(transform.position + directionOffset);
        if (!movementTilemap.HasTile(gridPos) || colliderTilemap.HasTile(gridPos))
        {
            return false;
        }
        return true;
    }
}

