using UnityEngine;

public class BookToogle : MonoBehaviour
{
    public GameObject closedBook;
    public GameObject openBook;

    public void ToggleBook()
    {
        bool open = openBook.activeSelf;
        openBook.SetActive(!open);
        closedBook.SetActive(open);
    }
}
