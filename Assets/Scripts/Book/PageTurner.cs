using UnityEngine;

public class PageTurner : MonoBehaviour
{
    [Header("All pages in order (Page 0, Page 1, Page 2, ...)")]
    [SerializeField] private GameObject[] pages;

    private int currentIndex = 0;

    private void Start()
    {
        ShowPage(currentIndex);
    }

    public void NextPage()
    {
        if (pages == null || pages.Length == 0) return;

        // go to next page but don't go past last
        if (currentIndex < pages.Length - 1)
        {
            currentIndex++;
            ShowPage(currentIndex);
        }
    }

    public void PreviousPage()
    {
        if (pages == null || pages.Length == 0) return;

        // go to previous page but don't go below 0
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowPage(currentIndex);
        }
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == index);
        }
    }
}
