using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance { get; private set; }

    public static event Action<string> ObjectiveTrigger;

    public UnityEvent QuestOrObjectiveUpdated;
    public UnityEvent<string> QuestFinished;

    public Quest currentQuest { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private Transform questContainer;
    //[SerializeField] private GameObject objectivePrefab;
    //[SerializeField] private GameObject questComplete;

    //private TextMeshProUGUI currentObjectiveText;
    //private List<GameObject> spawnedObjectiveTexts = new List<GameObject>();

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

        QuestOrObjectiveUpdated.Invoke();
    }

    public void UpdateObjective()
    {
        StartCoroutine(ObjectiveChangeAnimation(currentQuest.CurrentObjective.ToString()));

        hintText.text = currentQuest.CurrentObjective.HintText;

        QuestOrObjectiveUpdated.Invoke();
    }

    private IEnumerator ObjectiveChangeAnimation(string newObjective)
    {
        if (!string.IsNullOrEmpty(objectiveText.text))
        {
            objectiveText.text = "<s>" + objectiveText.text + "</s>";
            yield return new WaitForSeconds(2);
        }

        objectiveText.text = "- " + newObjective;
    }

    public void FinishQuest()
    {
        StartCoroutine(FinishQuestAnimation());
    }

    private IEnumerator FinishQuestAnimation()
    {
        if (!string.IsNullOrEmpty(objectiveText.text))
        {
            objectiveText.text = "<s>" + objectiveText.text + "</s>";
        }
        questText.text = "<s>" + questText.text + "</s>";

        yield return new WaitForSeconds(2);

        questText.text = "No Active Quest";
        objectiveText.text = "";

        var previousQuestId = currentQuest.QuestId;
        currentQuest = null;
        //questComplete.SetActive(true);

        QuestFinished.Invoke(previousQuestId);
    }
}
