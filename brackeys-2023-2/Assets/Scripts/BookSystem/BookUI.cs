using TMPro;
using UnityEngine;

public class BookUI : MonoBehaviour
{
    [SerializeField]
    private BookManager _BookManager;
    [SerializeField]
    private Canvas _BookCanvas;
    [SerializeField]
    private TextMeshProUGUI[] _Pages;

    private void Start()
    {
        _BookManager.OnBookOpen += BookOpenHandler;
        _BookManager.OnBookPageFlip += BookPageFlipHandler;
        _BookManager.OnBookClose += BookCloseHandler;
    }

    public void FlipLeftHandler()
    {
        _BookManager.TryFlipPage(BookManager.FlipDirection.LEFT);
    }
    public void FlipRightHandler()
    {
        _BookManager.TryFlipPage(BookManager.FlipDirection.RIGHT);
    }

    private void BookOpenHandler(Book book)
    {
        _BookCanvas.enabled = true;
        UpdatePages(book, 0);
    }
    private void BookPageFlipHandler(Book book, int pageIndex)
    {
        UpdatePages(book, pageIndex);
    }
    private void BookCloseHandler(Book book)
    {
        _BookCanvas.enabled = false;
    }
    private void UpdatePages(Book book, int pageIndex)
    {
        if (pageIndex == 0)
        {
            _Pages[0].text = "";
            _Pages[1].text = book.Pages[pageIndex].Text;
        }
        else if (pageIndex + 1 == book.Pages.Length)
        {
            _Pages[0].text = book.Pages[pageIndex].Text;
            _Pages[1].text = "";
        }
        else
        {
            _Pages[0].text = book.Pages[pageIndex - 1].Text;
            _Pages[1].text = book.Pages[pageIndex].Text;
        }
    }
}