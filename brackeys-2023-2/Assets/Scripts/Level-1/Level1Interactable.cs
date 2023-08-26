using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Level1Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private Level1ItemObject item;

    [Tooltip("Which dialogue will display when interacting with the interactable during which quest/objective")]
    [SerializeField] private DialogueChoice[] dialogueChoice;

    private DialogueManager dialogueManager;
    private BookManager bookManager;
    private BookDB bookDb;
    private DialogueDB dialogueDb;

    private DialogueChoice curDialogueChoice;

    private void Start()
    {
        dialogueManager = Level1Manager.Instance.DialogueManager;
        bookManager = Level1Manager.Instance.BookManager;
        bookDb = Level1Manager.Instance.BookDb;
        dialogueDb = Level1Manager.Instance.DialogueDb;
    }

    public void ClickInteract()
    {
        curDialogueChoice = dialogueChoice.LastOrDefault(x => x.ValidDialogue());
        var dialogueId = curDialogueChoice.dialogueId;

        if (!string.IsNullOrEmpty(dialogueId))
        {
            if (dialogueDb.TryGetDialogue(dialogueId, out Dialogue dialogue))
            {
                if (dialogueManager.TrySetDialogue(dialogue))
                {
                    Level1Manager.Instance.PlayerController.TogglePlayerControls(false);
                    dialogueManager.PlayDialogue();
                    dialogueManager.OnDialogueEnd += DialogueEndHandler;
                }
            }
        }
    }

    private void DialogueEndHandler(Dialogue dialogue)
    {
        dialogueManager.OnDialogueEnd -= DialogueEndHandler;

        if (item != null && curDialogueChoice.pickUpItem)
        {
            var bookId = item.BookId;
            if (!string.IsNullOrEmpty(bookId))
            {
                if (bookDb.TryGetDialogue(bookId, out Book book))
                {
                    if (bookManager.TrySetBook(book))
                    {
                        bookManager.OpenBook();

                        bookManager.OnBookClose += BookCloseHandler;
                    }
                }
            }
            else
            {
                PickupItem();
            }
        } else
        {
            ExitInteractable();
        }
    }

    private void BookCloseHandler(Book book)
    {
        bookManager.OnBookClose -= BookCloseHandler;

        PickupItem();
    }

    private void PickupItem()
    {
        var pickupItem = item.ToItem();
        if (!Level1Manager.Instance.IsItemInInventory(pickupItem.itemId))
        {
            Level1Manager.Instance.AddToInventory(item.ToItem());
        }

        ExitInteractable();
    }

    private void ExitInteractable()
    {
        if (!string.IsNullOrEmpty(curDialogueChoice.objectiveId) && curDialogueChoice.completeObjective)
            QuestManager.instance.TriggerQuestObj(curDialogueChoice.objectiveId);

        Level1Manager.Instance.PlayerController.TogglePlayerControls(true);
    }

    public string EnterInteract()
    {
        var validDialogue = false;
        foreach (DialogueChoice choice in dialogueChoice)
        {
            if (choice.ValidDialogue())
                validDialogue = true;
        }
        if (!validDialogue)
        {
            return "";
        }
        return "Interact [E]";
    }

    public void ExitInteract()
    {
        return;
    }
}
