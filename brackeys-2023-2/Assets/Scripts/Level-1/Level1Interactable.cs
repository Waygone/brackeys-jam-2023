using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Level1Interactable : MonoBehaviour
{
    [SerializeField] private Level1ItemObject item;

    private DialogueManager dialogueManager; // need to get somehow
    private BookManager bookManager; // Need to get somehow
    private BookDB bookDb;

    [Tooltip("Which dialogue will display when interacting with the interactable during which quest/objective")]
    [SerializeField] private DialogueChoice[] dialogueChoice;

    private DialogueDB dialogueDb;

    private void Awake()
    {
        dialogueDb = GetComponent<DialogueDB>();
    }

    private void Start()
    {
        dialogueManager = Level1Manager.Instance.DialogueManager;
        bookManager = Level1Manager.Instance.BookManager;
        bookDb = Level1Manager.Instance.BookDb;
    }

    public void Interact()
    {
        var dialogueCon = dialogueChoice.LastOrDefault(x => x.ValidDialogue());
        var dialogueId = dialogueCon.dialogueId;

        if (dialogueDb.TryGetDialogue(dialogueId, out Dialogue dialogue))
        {
            if (dialogueManager.TrySetDialogue(dialogue))
            {
                dialogueManager.PlayDialogue();

                if (item != null)
                {
                    dialogueManager.OnDialogueEnd += DialogueEndHandler;
                }
            }
        }
    }

    private void DialogueEndHandler(Dialogue dialogue)
    {
        dialogueManager.OnDialogueEnd -= DialogueEndHandler;
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
        } else
        {
            PickupItem();
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
    }
}
