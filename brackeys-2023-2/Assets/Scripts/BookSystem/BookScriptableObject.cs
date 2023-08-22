using UnityEngine;

[CreateAssetMenu(fileName = "Book", menuName = "ScriptableObjects/Book", order = 1)]
public class BookScriptableObject : ScriptableObject
{
    public string Id;
    public Page[] Pages;
    public int PagesPerView;

    public Book ToBook()
    {
        return new Book { Id = Id, Pages = Pages, PagesPerView = PagesPerView };
    }
}