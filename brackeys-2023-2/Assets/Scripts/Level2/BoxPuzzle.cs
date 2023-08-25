using System;
using System.Collections;
using UnityEngine;

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
    private NewPlayerMovement _PlayerMovement;
    [SerializeField]
    private Canvas _InteractionCanvas;
    [SerializeField]
    private CanMove _CanMove;

    private Utils.Direction _playerDirection;
    private bool _isMoving = false;
    private bool _isControlled = false;

    private void Update()
    {
        if (_isMoving || !_isControlled)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _InteractionCanvas.enabled = false;
            _PlayerMovement.DisableMoveBox();
            _isControlled = false;
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
                        Move(Utils.Direction.WEST);
                    }
                    else if (x == -1)
                    {
                        Move(Utils.Direction.EAST);
                    }
                    break;
                }
        }
    }

    private void Move(Utils.Direction direction)
    {
        Vector3 dir = new(Utils.Directions[(int)direction][0], Utils.Directions[(int)direction][1], 0f);
        StartCoroutine(MoveImpl(dir));
    }
    private IEnumerator MoveImpl(Vector3 direction)
    {
        _isMoving = true;
        float t = 0;
        Vector3 startBoxPosition = transform.position;
        Vector3 startPlayerPosition = _PlayerMovement.transform.position;

        Vector3 targeBoxPosition = transform.position + direction;
        Vector3 targetPlayerPosition = _PlayerMovement.transform.position + direction;
        while (t < 1)
        {
            t = Math.Clamp(t + Time.deltaTime, 0f, 1f);
            Vector3 newBoxPosition = Vector3.Lerp(startBoxPosition, targeBoxPosition, t);
            Vector3 newPlayerPosition = Vector3.Lerp(startPlayerPosition, targetPlayerPosition, t);

            transform.position = newBoxPosition;
            _PlayerMovement.transform.position = newPlayerPosition;

            yield return null;
        }

        _isMoving = false;
    }


    public string EnterInteract()
    {
        if (_CanMove == CanMove.NONE)
        {
            return "";
        }
        return "Interact [E]";
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

        _InteractionCanvas.enabled = true;
        _PlayerMovement.EnableMoveBox(transform.position, transform.localScale, _CanMove, out _playerDirection);
        _isControlled = true;
    }
}
