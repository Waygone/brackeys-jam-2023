using System.Collections;
using UnityEngine;
using static DialogueManager;

public class Level2Manager : MonoBehaviour
{
    public delegate void Level2FoundAllKeysHandler();
    public event Level2FoundAllKeysHandler OnLevel2FoundAllKeys;

    private readonly bool[] _hasFoundKeys = new bool[3];

    [SerializeField] private Quest quest;
    [SerializeField] private PlayerController playerController;

    private DialogueDB dialogueDb;
    private DialogueManager dialogueManager;

    private void Awake()
    {
        dialogueDb = GetComponent<DialogueDB>();
        dialogueManager = GetComponent<DialogueManager>();

        OnLevel2FoundAllKeys += FoundAllKeys;
    }

    private void Start()
    {
        dialogueManager.OnDialogueEnd += DialogueEndHandler;
        dialogueManager.OnDialogueBegin += DialogueBeginHandler;

        StartCoroutine(StartQuest());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            dialogueManager.SkipPassage();
        }
    }

    public void SetFoundKey(int index)
    {
        _hasFoundKeys[index] = true;
        UpdateState();
    }

    private void PlayDialogue(string dialogueId)
    {
        if (dialogueManager.TrySetDialogue(dialogueDb.GetDialogue(dialogueId)))
        {
            dialogueManager.PlayDialogue();
        }
    }

    private void UpdateState()
    {
        for (int i = 0; i < _hasFoundKeys.Length; ++i)
        {
            if (_hasFoundKeys[i] == false)
            {
                return;
            }
        }

        OnLevel2FoundAllKeys?.Invoke();
        Debug.LogWarning("Level2 completed!");
    }

    private void FoundAllKeys()
    {
        QuestManager.instance.TriggerQuestObj("Keys1");
        PlayDialogue("keys_2");
    }

    private void DialogueBeginHandler(Dialogue dialogue)
    {
        playerController.TogglePlayerControls(false);
    }

    private void DialogueEndHandler(Dialogue dialogue)
    {
        playerController.TogglePlayerControls(true);

        switch (dialogue.Id)
        {
            case "keys_1":
                QuestManager.instance.BeginQuest(quest);
                break;
        }
    }

    private IEnumerator StartQuest()
    {
        yield return null;
        PlayDialogue("keys_1");
    }
}
