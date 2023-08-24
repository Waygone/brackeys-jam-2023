using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostControl : MonoBehaviour
{
    private DialogueDB dialogueDb;
    [SerializeField] private DialogueManager dialogueManager;

    [Tooltip("Which dialogue will display when trying to talk to the Ghost during quests")]
    [SerializeField] private DialogueChoice[] dialogueChoice;

    private void Awake()
    {
        dialogueDb = GetComponent<DialogueDB>();
    }

    public void TalkTo()
    {
        var dialogueCon = dialogueChoice.LastOrDefault(x => x.ValidDialogue());
        var dialogueId = dialogueCon.dialogueId;

        if (dialogueDb.TryGetDialogue(dialogueId, out Dialogue dialogue))
        {
            SendDialogue(dialogue);
        }
    }

    public void GetHint()
    {
        var hintText = QuestManager.instance.currentQuest.CurrentObjective.HintText;
        if (!string.IsNullOrEmpty(hintText))
        {
            var hintDialogue = CreateDialogue("Ghost", hintText, 0.1f);

            SendDialogue(hintDialogue);
        }
    }

    private void SendDialogue(Dialogue dialogue)
    {
        if (dialogueManager.TrySetDialogue(dialogue))
        {
            dialogueManager.PlayDialogue();
        }
    }

    private Dialogue CreateDialogue(string title, string message, float secondsPerCharacter)
    {
        var dialoguePassage = new DialoguePassage { 
            Title = title, 
            Message = message, 
            SecondsPerCharacter = secondsPerCharacter 
        };

        return new Dialogue { Id = "na", DialoguePassages = new DialoguePassage[] { dialoguePassage } };
    }
}

