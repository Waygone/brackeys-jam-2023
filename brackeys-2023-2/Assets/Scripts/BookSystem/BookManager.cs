using System;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public enum FlipDirection
    {
        LEFT = -1,
        RIGHT = 1,
    }

    public delegate void BookOpenHandler(Book book);
    public event BookOpenHandler OnBookOpen;

    public delegate void OnBookPageFlipHandler(Book book, int pageIndex);
    public event OnBookPageFlipHandler OnBookPageFlip;

    public delegate void BookCloseHandler(Book book);
    public event BookCloseHandler OnBookClose;

    [SerializeField]
    private AudioClip _PageFlipAudioClip;
    [SerializeField]
    private AudioSource _AudioSource;

    private Book _book = null;
    private bool _isBookOpen = false;
    private int _currentPageIndex = 0;

    public bool TrySetBook(Book book)
    {
        if (_isBookOpen)
        {
            return false;
        }

        _book = book;
        return true;
    }

    public void OpenBook()
    {
        if (_isBookOpen)
        {
            return;
        }

        _currentPageIndex = 0;
        _isBookOpen = true;
        OnBookOpen?.Invoke(_book);
    }
    public void CloseBook()
    {
        OnBookClose?.Invoke(_book);
        _isBookOpen = false;
    }

    public void TryFlipPage(FlipDirection direction)
    {
        // How much do we have to move?
        int pagesOffset = _book.PagesPerView * (int)direction;
        int newValue = Math.Clamp(_currentPageIndex + pagesOffset, 0, _book.Pages.Length - 1);

        // If something changed, then trigger the event.
        if (_currentPageIndex != newValue)
        {
            _AudioSource.volume = GlobalData.MainVolume / 100f;
            _AudioSource.PlayOneShot(_PageFlipAudioClip);
            _currentPageIndex = newValue;
            OnBookPageFlip?.Invoke(_book, _currentPageIndex);
        }
    }
}
