using UnityEngine;
using System.Collections.Generic;

public class BookDB : MonoBehaviour
{
    [SerializeField]
    private BookScriptableObject[] _Books;

    private Dictionary<string, Book> _booksMap;

    private void Awake()
    {
        FillDialoguesMap();
    }

    // Fills the dialogues map to allow efficient lookups by dialogue's ID.
    private void FillDialoguesMap()
    {
        _booksMap = new Dictionary<string, Book>();
        for (int i = 0; i < _Books.Length; ++i)
        {
            _booksMap[_Books[i].Id] = _Books[i].ToBook();
        }
    }

    // Not recommended since it allocates memory at every call.
    public Book GetBook(int index)
    {
        return _Books[index].ToBook();
    }
    public Book GetBook(string id)
    {
        return _booksMap[id];
    }

    // Preferred way to retrieve book safely.
    public bool TryGetDialogue(string id, out Book result)
    {
        return _booksMap.TryGetValue(id, out result);
    }
}