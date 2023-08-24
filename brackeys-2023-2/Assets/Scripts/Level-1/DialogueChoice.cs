using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueChoice
{
    public string dialogueId;
    public string questId;
    public string objectiveId;

    public bool ValidDialogue()
    {
        return CorrectQuest() && CorrectObjective();
    }

    private bool CorrectQuest() => !string.IsNullOrEmpty(questId) ? QuestManager.instance.currentQuest.QuestId == questId : true;
    private bool CorrectObjective() => !string.IsNullOrEmpty(objectiveId) ? QuestManager.instance.currentQuest.CurrentObjective.ObjectiveID == objectiveId : true;
}
