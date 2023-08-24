using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1ItemObject : ScriptableObject
{
    [SerializeField] private string itemId;

    [SerializeField] private string itemName;

    [SerializeField] private string bookId;

    public string BookId
    {
        get => bookId;
        private set => bookId = value;
    }

    public Level1Item ToItem()
    {
        return new Level1Item { itemId = itemId, itemName = itemName, bookId = bookId };
    }
}

public class Level1Item
{
    public string itemId;
    public string itemName;
    public string bookId;
}
