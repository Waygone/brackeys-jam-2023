using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoxPuzzle : MonoBehaviour, IInteractable
{
    public enum CanMove
    {
        NONE,
        UD,
        LR,
        ALL,
    }

    [SerializeField]
    private PlayerController _PlayerController;
    [SerializeField]
    private CanMove _CanMove;
    [SerializeField]
    private Tilemap _CollisionTilemap;

    private Utils.Direction _playerDirection;
    private bool _isMoving = false;
    private bool _isControlled = false;

    private Tile _tile;

    private void Start()
    {
        _tile = ScriptableObject.CreateInstance<Tile>();
        _tile.name = "BoxPuzzle";
        _CollisionTilemap.SetTile(_CollisionTilemap.WorldToCell(transform.position), _tile);
    }

    private void Update()
    {
        if (_isMoving || !_isControlled)
        {
            return;
        }

        switch (_playerDirection)
        {
            case Utils.Direction.NORTH:
                {
                    if (_CanMove == CanMove.NONE || _CanMove == CanMove.LR)
                    {
                        break;
                    }

                    float y = Input.GetAxisRaw("Vertical");
                    if (y == 1)
                    {
                        Move(Utils.Direction.SOUTH);
                    }
                    else if (y == -1)
                    {
                        Move(Utils.Direction.NORTH);
                    }
                    break;
                }
            case Utils.Direction.SOUTH:
                {
                    if (_CanMove == CanMove.NONE || _CanMove == CanMove.LR)
                    {
                        break;
                    }

                    float y = Input.GetAxisRaw("Vertical");
                    if (y == 1)
                    {
                        Move(Utils.Direction.NORTH);
                    }
                    else if (y == -1)
                    {
                        Move(Utils.Direction.SOUTH);
                    }
                    break;
                }
            case Utils.Direction.EAST:
                {
                    if (_CanMove == CanMove.NONE || _CanMove == CanMove.UD)
                    {
                        break;
                    }

                    float x = Input.GetAxisRaw("Horizontal");
                    if (x == 1)
                    {
                        Move(Utils.Direction.EAST);
                    }
                    else if (x == -1)
                    {
                        Move(Utils.Direction.WEST);
                    }
                    break;
                }
            case Utils.Direction.WEST:
                {
                    if (_CanMove == CanMove.NONE || _CanMove == CanMove.UD)
                    {
                        break;
                    }

                    float x = Input.GetAxisRaw("Horizontal");
                    if (x == 1)
                    {
                        Move(Utils.Direction.EAST);
                    }
                    else if (x == -1)
                    {
                        Move(Utils.Direction.WEST);
                    }
                    break;
                }
        }
    }

    private void Move(Utils.Direction direction)
    {
        Vector3 dir = new(Utils.Directions[(int)direction][0], Utils.Directions[(int)direction][1], 0f);
        if (!CanMoveTo(dir))
        {
            return;
        }

        StartCoroutine(MoveImpl(dir));
    }
    private IEnumerator MoveImpl(Vector3 direction)
    {
        _isMoving = true;

        _CollisionTilemap.SetTile(_CollisionTilemap.WorldToCell(transform.position), null);

        float t = 0;
        Vector3 startBoxPosition = transform.position;
        Vector3 startPlayerPosition = _PlayerController.transform.position;

        Vector3 targeBoxPosition = transform.position + direction;
        Vector3 targetPlayerPosition = _PlayerController.transform.position + direction;
        while (t < 1)
        {
            t = Math.Clamp(t + Time.deltaTime, 0f, 1f);
            Vector3 newBoxPosition = Vector3.Lerp(startBoxPosition, targeBoxPosition, t);
            Vector3 newPlayerPosition = Vector3.Lerp(startPlayerPosition, targetPlayerPosition, t);

            transform.position = newBoxPosition;
            _PlayerController.transform.position = newPlayerPosition;

            yield return null;
        }

        _CollisionTilemap.SetTile(_CollisionTilemap.WorldToCell(transform.position), _tile);
        _isMoving = false;
    }


    public string EnterInteract()
    {
        if (_CanMove == CanMove.NONE)
        {
            return "";
        }
        return "Interact / Leave [E]";
    }
    public void ExitInteract()
    {

    }
    public void ClickInteract()
    {
        if (_CanMove == CanMove.NONE)
        {
            return;
        }

        if (_isControlled)
        {
            _PlayerController.DisableMoveBox(transform.position, _playerDirection);
            _isControlled = false;
        }
        else
        {
            _PlayerController.EnableMoveBox(transform.position, _CanMove, out _playerDirection);
            _isControlled = true;
        }
    }

    private bool CanMoveTo(Vector3 direction)
    {
        Vector3Int gridPos = _CollisionTilemap.WorldToCell(transform.position + direction);
        if (_CollisionTilemap.HasTile(gridPos))
        {
            return false;
        }

        // For player collision.
        Vector3Int gridPos2 = _CollisionTilemap.WorldToCell(transform.position + direction * 2);
        if (_CollisionTilemap.HasTile(gridPos2))
        {
            return false;
        }
        return true;
    }
}
