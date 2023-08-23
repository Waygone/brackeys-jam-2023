using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement controls;

    [SerializeField] private Tilemap movementTilemap;
    [SerializeField] private Tilemap collTilemap;


    #region CALLBACK
    private void Awake()
    {
        controls = new PlayerMovement();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Main.Movement.performed += context => Move(context.ReadValue<Vector2>());
    }

    #endregion

    private void Move(Vector2 direction)
    {
        if(CanMove(direction))
        {
            transform.position += (Vector3)direction;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPos = movementTilemap.WorldToCell(transform.position + (Vector3) direction);
        if (!movementTilemap.HasTile(gridPos) || collTilemap.HasTile(gridPos))
        {
            return false;
        }
        return true;
    }
}
