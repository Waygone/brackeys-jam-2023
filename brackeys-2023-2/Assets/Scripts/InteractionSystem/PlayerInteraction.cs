using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Vector2 _InteractionSize;
    [SerializeField]
    private Rigidbody2D _Rigidbody2D;

    public Vector2 _interactionOffset;
    private readonly Vector2 _baseOffset = new(0, -0.1f);
    private IInteractable _focusedInteractable = null;

    private void Start()
    {
        SetInteractionOffset(Utils.Direction.NORTH);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _focusedInteractable?.ClickInteract();
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_Rigidbody2D.position + _interactionOffset, _InteractionSize, 0f);
        IInteractable newInteractabe = null;
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i].TryGetComponent(out IInteractable interactable))
            {
                newInteractabe = interactable;
                break;
            }
        }

        if (newInteractabe != _focusedInteractable)
        {
            _focusedInteractable?.ExitInteract();
            _focusedInteractable = newInteractabe;
            _focusedInteractable?.EnterInteract();
        }
    }

    public void SetInteractionOffset(Utils.Direction direction)
    {
        _interactionOffset.x = Utils.Directions[(int)direction][0] * 0.125f;
        _interactionOffset.y = Utils.Directions[(int)direction][1] * 0.125f;

        _interactionOffset += _baseOffset;
    }
}

