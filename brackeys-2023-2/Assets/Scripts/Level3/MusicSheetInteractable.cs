using UnityEngine;

public class MusicSheetInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Level3Manager _Level3Manager;
    [SerializeField]
    private int _MusicSheetIndex;

    string IInteractable.EnterInteract()
    {
        return "Pickup [E]";
    }

    void IInteractable.ExitInteract()
    {

    }
    void IInteractable.ClickInteract()
    {
        _Level3Manager.SetFoundMusicSheet(_MusicSheetIndex);
        Destroy(gameObject);
    }
}
