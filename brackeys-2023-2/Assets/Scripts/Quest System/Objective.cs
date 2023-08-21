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
        objComplete = false;
        QuestManager.ObjectiveTrigger += ObjTriggered;
    }

    public void ObjTriggered(string objectiveTrigger)
    {
        if (objectiveTrigger == trigger && !objComplete)
        {
            objComplete = true;
            ObjectiveComplete?.Invoke();
        }
    }

    public override string ToString()
    {
        return description;
    }
}
