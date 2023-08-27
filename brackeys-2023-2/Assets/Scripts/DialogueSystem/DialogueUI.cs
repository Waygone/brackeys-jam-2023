using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    private DialogueManager _DialogueManager;
    [SerializeField]
    private Canvas _DialogueCanvas;
    [SerializeField]
    private TextMeshProUGUI _TitleText;
    [SerializeField]
    private TextMeshProUGUI _MessageText;

    private void Start()
    {
        Debug.Log("Start");
        _DialogueManager.OnDialogueBegin += DialogueBeginHandler;
        _DialogueManager.OnPassageChange += PassageChangeHandler;
        _DialogueManager.OnDialogueMessageChange += DialogueMessageChangeHandler;
        _DialogueManager.OnDialogueEnd += DialogueEndHandler;
    }

    private void DialogueBeginHandler(Dialogue dialogue)
    {
        Debug.Log("Begin dialogue");
        _DialogueCanvas.enabled = true;
    }
    private void PassageChangeHandler(DialoguePassage passage)
    {
        _TitleText.text = passage.Title;
        _MessageText.text = "";
    }
    private void DialogueMessageChangeHandler(string message)
    {
        _MessageText.text = message;
    }
    private void DialogueEndHandler(Dialogue dialogue)
    {
        _DialogueCanvas.enabled = false;
    }
}