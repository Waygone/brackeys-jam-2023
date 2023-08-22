using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance { get; private set; }

    public static event Action<string> ObjectiveTrigger;

    public Quest currentQuest { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private Transform questContainer;
    [SerializeField] private GameObject objectivePrefab;
    [SerializeField] private GameObject questComplete;

    private TextMeshProUGUI currentObjectiveText;
    private List<GameObject> spawnedObjectiveTexts = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        } else
        {
            instance = this;
        }
    }

    /// <summary>
    /// Calling this will trigger a quest objective.
    /// </summary>
    /// <param name="questTrigger">string name of Objective Trigger to activate. Must match up to trigger name on objective.</param>
    public void TriggerQuestObj(string objectiveTrigger)
    {
        ObjectiveTrigger.Invoke(objectiveTrigger);
    }

    /// <summary>
    /// Tells the Quest Manager to start a new given quest.
    /// </summary>
    /// <param name="questToBegin">Quest Scriptable Object for the quest to start.</param>
    public void BeginQuest(Quest questToBegin)
    {
        currentQuest = questToBegin;
        questText.text = questToBegin.Description;

        currentQuest.StartQuest();
    }

    public void UpdateObjective()
    {
        if (currentObjectiveText != null)
        {
            currentObjectiveText.text = "<s>" + currentObjectiveText.text + "</s>";
        }

        var currentObjective = Instantiate(objectivePrefab, questContainer);
        spawnedObjectiveTexts.Add(currentObjective);
        currentObjectiveText = currentObjective.GetComponent<TextMeshProUGUI>();
        currentObjectiveText.text = currentQuest.currentObjective.ToString();

        hintText.text = currentQuest.currentObjective.HintText;
    }

    public void FinishQuest()
    {
        questText.text = "No Active Quest";
        spawnedObjectiveTexts.ForEach(x => Destroy(x));
        spawnedObjectiveTexts.Clear();

        currentQuest = null;
        currentObjectiveText.text = "<s>" + currentObjectiveText.text + "</s>";
        questComplete.SetActive(true);
    }
}
