using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSystemTest : MonoBehaviour
{
    public Quest questToRun;

    private void Start()
    {
        QuestManager.instance.BeginQuest(questToRun);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            QuestManager.instance.TriggerQuestObj("Space");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            QuestManager.instance.TriggerQuestObj("Arrow");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            QuestManager.instance.TriggerQuestObj("P");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            QuestManager.instance.TriggerQuestObj("T");
        }
    }
}
