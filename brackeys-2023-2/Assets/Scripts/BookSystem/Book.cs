[System.Serializable]
public class Book
{
    public string Id;
    // Page 0      = Book cover title (single front page)
    // Page 1..n-2 = Book pages (if normal book, PagesPerView is 2, since you can see two pages at the same time, left and right).
    // Page n-1    = Book end cover (single back page)
    // This is the conventional book representation, but a book with only 1 page 
    // (a piece of paper), or a book with 7 pages per view is possible. You just have
    // to tweak your UI accordingly.
    public Page[] Pages;
    public int PagesPerView;
}