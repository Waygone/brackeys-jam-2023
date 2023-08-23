using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/NewQuest")]
public class Quest : ScriptableObject
{
    [Tooltip("QuestID needs to be unique.")]
    [SerializeField] private string questId;

    [Header("Quest details")]
    [Tooltip("Name of the quest. Keep it short.")]
    [SerializeField] private string questName;

    [Tooltip("Quest description to display on players screen.")]
    [SerializeField] private string description;

    [Header("Objectives")]
    [Tooltip("Objective array needs to be in-order or wrong current objective could display.")]
    [SerializeField] private Objective[] objectives;

    [HideInInspector] public Objective currentObjective;

    private bool questComplete;

    public string QuestId
    {
        get => questId; 
        set => questId = value;
    }

    public string QuestName 
    {
        get => questName; 
        private set => questName = value;
    }

    public string Description
    {
        get => description; 
        private set => description = value;
    }

    public Objective[] Objectives
    {
        get => objectives; 
        private set => objectives = value;
    }

    public Objective CurrentObjective
    {
        get => currentObjective;
        private set => currentObjective = value;
    }

    public void StartQuest()
    {
        foreach (var obj in objectives)
        {
            obj.Initialize();
        }

        NextObjective();
    }

    private void NextObjective()
    {
        if (currentObjective != null)
        {
            currentObjective.ObjectiveComplete -= NextObjective;
        }

        currentObjective = objectives.FirstOrDefault(x => !x.objComplete);

        if (currentObjective == null)
        {
            questComplete = true;
            QuestManager.instance.FinishQuest();
        } else
        {
            currentObjective.ObjectiveComplete += NextObjective;
            QuestManager.instance.UpdateObjective();
        }
    }
}
