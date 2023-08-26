using UnityEngine;

public class PianoKeyInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Level3Manager _Level3Manager;
    [SerializeField]
    private int _PianoKeyIndex;

    private bool _isInteractable = false;

    private void Start()
    {
        _Level3Manager.OnLevel3StateChange += Level3StateChangeHandler;
    }

    private void Level3StateChangeHandler(Level3Manager.Level3State state)
    {
        if (state == Level3Manager.Level3State.FOUND_DECIPHER_BOOK)
        {
            _isInteractable = true;
        }
    }

    string IInteractable.EnterInteract()
    {
        if (!_isInteractable)
        {
            return "";
        }
        return "Pickup [E]";
    }

    void IInteractable.ExitInteract()
    {

    }
    void IInteractable.ClickInteract()
    {
        if (!_isInteractable)
        {
            return;
        }
        _Level3Manager.SetFoundPianoKeys(_PianoKeyIndex);
        Destroy(gameObject);
    }
}
