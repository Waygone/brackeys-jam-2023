using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
    [Tooltip("ObjectiveID needs to be unique.")]
    [SerializeField] private string objectiveId;

    [Header("Objective details")]
    [Tooltip("Objective description to display under Quest in-game.")]
    [SerializeField] private string description;

    [Tooltip("What trigger to QuestManager will complete this objective.")]
    [SerializeField] private string trigger;

    [Tooltip("What to display if the player gets stuck")]
    [SerializeField] private string hintText;

    [HideInInspector] public bool objComplete = false;
    [HideInInspector] public event Action ObjectiveComplete;

    public string ObjectiveID
    {
        get => objectiveId;
        set => objectiveId = value;
    }

    public string Description
    {
        get => description;
        private set => description = value;
    }

    public string HintText
    {
        get => hintText;
        private set => hintText = value;
    }

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
