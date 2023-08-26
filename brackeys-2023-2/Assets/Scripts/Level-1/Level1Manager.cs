using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Quest quest1;
    [SerializeField] private Quest quest2;

    private Dictionary<string, Level1Item> inventory = new Dictionary<string, Level1Item>();

    private BookDB bookDb;
    private DialogueDB dialogueDb;
    private DialogueManager dialogueManager;
    private BookManager bookManager;

    public static Level1Manager Instance { get; private set; }

    public BookDB BookDb
    {
        get => bookDb;
        set => bookDb = value;
    }

    public DialogueDB DialogueDb
    {
        get => dialogueDb;
        set => dialogueDb = value;
    }

    public DialogueManager DialogueManager
    {
        get => dialogueManager;
        set => dialogueManager = value;
    }

    public BookManager BookManager
    {
        get => bookManager; 
        set => bookManager = value;
    }

    public PlayerController PlayerController
    {
        get => playerController;
        set => playerController = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }

        bookDb = GetComponent<BookDB>();
        dialogueDb = GetComponent<DialogueDB>();
        dialogueManager = GetComponent<DialogueManager>();
        bookManager = GetComponent<BookManager>();
    }

    private void Start()
    {
        dialogueManager.OnDialogueEnd += DialogueEndHandler;
        dialogueManager.OnDialogueBegin += DialogueBeginHandler;

        StartCoroutine(Quest1Opening());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            dialogueManager.SkipPassage();
        }

        if (Input.GetKeyDown(KeyCode.H) && PlayerController.ArePlayerControlsEnabled)
        {
            GetHint();
        }
    }

    public void AddToInventory(Level1Item item)
    {
        inventory.Add(item.itemId, item);
    }

    public bool IsItemInInventory(string itemId)
    {
        return inventory.ContainsKey(itemId);
    }

    public void GetHint()
    {
        var hintText = QuestManager.instance.currentQuest.CurrentObjective.HintText;
        if (!string.IsNullOrEmpty(hintText))
        {
            var hintDialogue = CreateNewDialogue("Librarian", hintText, 0.05f);

            if (dialogueManager.TrySetDialogue(hintDialogue))
            {
                dialogueManager.PlayDialogue();
            }
        }
    }

    private Dialogue CreateNewDialogue(string title, string message, float secondsPerCharacter)
    {
        var dialoguePassage = new DialoguePassage
        {
            Title = title,
            Message = message,
            SecondsPerCharacter = secondsPerCharacter
        };

        return new Dialogue { Id = "na", DialoguePassages = new DialoguePassage[] { dialoguePassage } };
    }

    private void DialogueBeginHandler(Dialogue dialogue)
    {
        playerController.TogglePlayerControls(false);
    }

    private IEnumerator Quest1Opening()
    {
        yield return new WaitForSeconds(1);

        if (dialogueManager.TrySetDialogue(dialogueDb.GetDialogue("quest1_start")))
        {
            dialogueManager.PlayDialogue();
        }
    }

    private void DialogueEndHandler(Dialogue dialogue)
    {
        playerController.TogglePlayerControls(true);
        switch (dialogue.Id)
        {
            case "quest1_start":
                QuestManager.instance.BeginQuest(quest1);
                break;
            case "patron1":
                StartCoroutine(Quest1GoFindBook());
                break;
            case "patron3":
                StartCoroutine(Quest2());
                break;
        }
    }

    private IEnumerator Quest1GoFindBook()
    {
        yield return new WaitForSeconds(3f);

        while (!PlayerController.ArePlayerControlsEnabled)
        {
            yield return null;
        }

        if (dialogueManager.TrySetDialogue(dialogueDb.GetDialogue("quest1_help")))
        {
            dialogueManager.PlayDialogue();
        }
    }

    private IEnumerator Quest2()
    {
        yield return new WaitForSeconds(2);

        QuestManager.instance.TriggerQuestObj("Patron4");

        // Play sound here.



        yield return null;
    }
}
