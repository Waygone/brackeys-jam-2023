using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControl : MonoBehaviour
{
    private DialogueDB dialogueDB;
    private NpcMovement ghostMover;

    private void Awake()
    {
        dialogueDB = GetComponent<DialogueDB>();
    }

    public void GetHint()
    {

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
