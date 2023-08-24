using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    private Dictionary<string, Level1Item> inventory = new Dictionary<string, Level1Item>();

    private BookDB bookDb;
    private DialogueManager dialogueManager;
    private BookManager bookManager;

    public static Level1Manager Instance { get; private set; }

    public BookDB BookDb
    {
        get => bookDb;
        set => bookDb = value;
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
        dialogueManager = GetComponent<DialogueManager>();
        bookManager = GetComponent<BookManager>();
    }

    public void AddToInventory(Level1Item item)
    {
        inventory.Add(item.itemId, item);
    }

    public bool IsItemInInventory(string itemId)
    {
        return inventory.ContainsKey(itemId);
    }
}

