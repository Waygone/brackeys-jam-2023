using UnityEngine;

public class BookTest : MonoBehaviour
{
    [SerializeField]
    private BookDB _BookDB;
    [SerializeField]
    private BookManager _BookManager;

    private int _bookIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _BookManager.CloseBook();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _BookManager.TryFlipPage(BookManager.FlipDirection.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _BookManager.TryFlipPage(BookManager.FlipDirection.RIGHT);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_BookManager.TrySetBook(_BookDB.GetBook(_bookIndex)))
            {
                _BookManager.OpenBook();
                Debug.Log("Successfully opened book");

                if (_bookIndex + 1 < 2)
                {
                    ++_bookIndex;
                }
            }
            else
            {
                Debug.LogWarning("Unable to open book");
            }
        }
    }
}