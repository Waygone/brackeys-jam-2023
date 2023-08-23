using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/NewQuest")]
public class Quest : ScriptableObject
{
    [Header("Quest details")]
    [Tooltip("Needs to be unique.")]
    [SerializeField] private string uniqueName;

    [Tooltip("Quest description to display on players screen.")]
    [SerializeField] private string description;

    [Header("Objectives")]
    [Tooltip("Objective array needs to be in-order or wrong current objective could display.")]
    [SerializeField] private Objective[] objectives;

    [HideInInspector] public Objective currentObjective;

    private bool questComplete;

    public string UniqueName 
    {
        get => uniqueName; 
        private set => uniqueName = value;
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
