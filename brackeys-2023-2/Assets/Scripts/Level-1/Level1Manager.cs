using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Quest quest1;
    [SerializeField] private Quest quest2;
    [SerializeField] private Level1Dolly level1Dolly;
    [SerializeField] private GameObject forbiddenBook;
    [SerializeField] private GameObject ghost;

    [SerializeField]
    private AudioClip bookFallSound;
    [SerializeField]
    private AudioSource soundEffectAudio;

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
        bookManager.OnBookOpen += BookOpenHandler;
        bookManager.OnBookClose += BookCloseHandler;

        if (!(GlobalData.State == GlobalData.GameState.LEVEL_1_END_GAME))
        {
            StartCoroutine(Quest1Opening());
        } else
        {
            playerController.playerAnimator.SetFloat("Dir", 0);
            StartCoroutine(EndgameOpening());
        }
        
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bookManager.CloseBook();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                bookManager.TryFlipPage(BookManager.FlipDirection.LEFT);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                bookManager.TryFlipPage(BookManager.FlipDirection.RIGHT);
            }
        }

    public void AddToInventory(Level1Item item)
    {
        inventory.Add(item.itemId, item);

        Debug.Log("Picked up");
        if (QuestManager.instance.currentQuest.QuestId == "Level1_Book")
        {
            if (inventory.ContainsKey("forbidden2") && inventory.ContainsKey("forbidden3"))
            {
                PlayDialogue("quest2_foundbooks");
                QuestManager.instance.TriggerQuestObj("Forbidden2");
            }
        }
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
            var speaker = ghost.activeSelf ? "Ghost" : "Librarian";
            var hintDialogue = CreateNewDialogue(speaker, hintText, 0.05f);

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

    private void BookOpenHandler(Book book)
    {
        playerController.TogglePlayerControls(false);
    }

    private void PlayDialogue(string dialogueId)
    {
        if (dialogueManager.TrySetDialogue(dialogueDb.GetDialogue(dialogueId)))
        {
            dialogueManager.PlayDialogue();
        }
    }

    private IEnumerator Quest1Opening()
    {
        yield return new WaitForSeconds(1);

        PlayDialogue("quest1_start");
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
            case "quest2_summoninghost":
                StartCoroutine(Quest2_SummoningGhost());
                break;
            case "quest2_ghostsummoned":
                QuestManager.instance.TriggerQuestObj("Forbidden3");
                break;
            case "quest2_hiddenstaircase":
                SceneManager.LoadScene(2);
                break;
        }
    }

    private void BookCloseHandler(Book book)
    {
        playerController.TogglePlayerControls(true);
        switch (book.Id)
        {
            case "forbiddenbook1":
                Debug.Log("You are here");
                PlayDialogue("quest2_gofind");
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

        PlayDialogue("quest1_help");
    }

    private IEnumerator Quest2()
    {
        yield return new WaitForSeconds(2);

        QuestManager.instance.TriggerQuestObj("Patron4");

        playerController.TogglePlayerControls(false);

        soundEffectAudio.volume = GlobalData.MainVolume;
        soundEffectAudio.PlayOneShot(bookFallSound);

        forbiddenBook.SetActive(true);

        yield return new WaitForSeconds(1);

        level1Dolly.PlayDolly();

        if (dialogueManager.TrySetDialogue(dialogueDb.GetDialogue("quest2_start")))
        {
            dialogueManager.PlayDialogue();
        }

        yield return new WaitForSeconds(12);

        QuestManager.instance.BeginQuest(quest2);
    }

    private IEnumerator Quest2_SummoningGhost()
    {
        playerController.TogglePlayerControls(false);

        yield return new WaitForSeconds(2f);

        ghost.SetActive(true);

        PlayDialogue("quest2_ghostsummoned");
    }

    private IEnumerator EndgameOpening()
    {

        PlayerController.TogglePlayerControls(true);
        yield return null;


    }
}
