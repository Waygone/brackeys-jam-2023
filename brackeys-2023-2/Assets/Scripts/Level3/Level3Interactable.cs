using UnityEngine;

public class Level3Interactable : MonoBehaviour
{
    [SerializeField]
    private Level3Manager _Level3Manager;
    [SerializeField]
    private Level3Manager.Level3State _RequiredAtLeastState;

    public delegate void Level3InteractableChangeHandler(bool is_interactable);
    public event Level3InteractableChangeHandler OnLevel3InteractableChange;

    private void Start()
    {
        _Level3Manager.OnLevel3StateChange += Level3StateChangeHandler;
    }

    private void Level3StateChangeHandler(Level3Manager.Level3State state)
    {
        if (state >= _RequiredAtLeastState)
        {
            OnLevel3InteractableChange?.Invoke(true);
        }
        else
        {
            OnLevel3InteractableChange?.Invoke(false);
        }
    }
}