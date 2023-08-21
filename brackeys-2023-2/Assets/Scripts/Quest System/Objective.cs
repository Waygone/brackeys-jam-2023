using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
    [Header("Objective details")]
    [Tooltip("Objective description to display under Quest in-game.")]
    public string description;

    [Tooltip("What trigger to QuestManager will complete this objective.")]
    public string trigger;

    [HideInInspector] public bool objComplete = false;
    [HideInInspector] public event Action ObjectiveComplete;

    public void Initialize()
    {
        QuestManager.QuestTrigger += ObjTriggered;
    }

    public void ObjTriggered(string questTrigger)
    {
        if (questTrigger == trigger && !objComplete)
        {
            objComplete = true;
            ObjectiveComplete.Invoke();
        }
    }
}
