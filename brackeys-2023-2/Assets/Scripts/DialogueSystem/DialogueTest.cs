using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    [SerializeField]
    private DialogueDB _DialogueDB;
    [SerializeField]
    private DialogueManager _DialogueManager;

    private void Start()
    {
        if (_DialogueManager.TrySetDialogue(_DialogueDB.GetDialogue("a")))
        {
            _DialogueManager.PlayDialogue();
            Debug.Log("Successfully started dialogue " + "a");
        }
        else
        {
            Debug.LogWarning("Unable to start dialogue " + "a");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _DialogueManager.SkipPassage();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_DialogueManager.TrySetDialogue(_DialogueDB.GetDialogue("b")))
            {
                _DialogueManager.PlayDialogue();
                Debug.Log("Successfully started dialogue " + "b");
            }
            else
            {
                Debug.LogWarning("Unable to start dialogue " + "b");
            }
        }
    }
}