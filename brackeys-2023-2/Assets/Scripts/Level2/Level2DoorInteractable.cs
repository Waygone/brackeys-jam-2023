
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Level2Manager _Level2Manager;

    private bool _isInteractable = false;

    private void Start()
    {
        _Level2Manager.OnLevel2FoundAllKeys += Level2FoundAllKeysHandler;
    }

    private void Level2FoundAllKeysHandler()
    {
        _isInteractable = true;
    }

    string IInteractable.EnterInteract()
    {
        if (!_isInteractable)
        {
            return "";
        }
        return "Open [E]";
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
        QuestManager.instance.TriggerQuestObj("Keys2");
        SceneManager.LoadScene(3);
    }
}
