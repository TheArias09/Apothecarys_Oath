using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimoirePotionNavigator : MonoBehaviour
{
    [SerializeField] GrimoirePotionPage firstPage;
    [SerializeField] GrimoirePotionPage lastPage;
    [SerializeField] GrimoirePotionPage middlePageFront;
    [SerializeField] GrimoirePotionPage middlePageBack;

    [SerializeField] GameObject firstPageGameObject;
    [SerializeField] GameObject lastPageGameObject;

    [SerializeField] List<GrimoirePotionPageData> grimoirePageDatas;

    private bool isLocked = false;
    private int currentIndex = 0;
    private int maxIndex => grimoirePageDatas.Count - 1;
    private int currentLeftPageIndex => currentIndex;

    private GrimoirePageAnimation grimoirePageAnimation;

    private void Awake()
    {
        grimoirePageAnimation = GetComponent<GrimoirePageAnimation>();

        firstPage.Clear();
        firstPageGameObject.SetActive(false);
        middlePageFront.Clear();
        middlePageBack.Clear();
        lastPage.DisplayDataForRightPage(grimoirePageDatas[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryToGoToNextPage();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryToGoToPreviousPage();
        }
    }

    public void TryToGoToNextPage()
    {
        if (isLocked || currentIndex == maxIndex) return;
        StartCoroutine(NextPage());
    }

    public IEnumerator NextPage()
    {
        isLocked = true;

        middlePageFront.DisplayDataForRightPage(grimoirePageDatas[currentLeftPageIndex]);
        middlePageBack.DisplayDataForLeftPage(grimoirePageDatas[currentLeftPageIndex + 1]);

        if (currentIndex != maxIndex - 1)
        {
            lastPage.DisplayDataForRightPage(grimoirePageDatas[currentLeftPageIndex + 1]);
        }
        else
        {
            lastPageGameObject.SetActive(false);
            lastPage.Clear();
        }

        yield return StartCoroutine(grimoirePageAnimation.FromRightToLeft());

        currentIndex++;

        firstPage.DisplayDataForLeftPage(grimoirePageDatas[currentLeftPageIndex]);
        firstPageGameObject.SetActive(true);

        isLocked = false;
    }

    public void TryToGoToPreviousPage()
    {
        if (isLocked || currentIndex == 0) return;
        StartCoroutine(PreviousPage());
    }

    public IEnumerator PreviousPage()
    {
        isLocked = true;

        middlePageFront.DisplayDataForRightPage(grimoirePageDatas[currentLeftPageIndex - 1]);
        middlePageBack.DisplayDataForLeftPage(grimoirePageDatas[currentLeftPageIndex]);

        if (currentIndex != 1)
        {
            firstPage.DisplayDataForLeftPage(grimoirePageDatas[currentLeftPageIndex - 1]);
        }
        else
        {
            firstPageGameObject.SetActive(false);
            firstPage.Clear();
        }

        yield return StartCoroutine(grimoirePageAnimation.FromLeftToRight());

        currentIndex--;

        lastPage.DisplayDataForRightPage(grimoirePageDatas[currentLeftPageIndex]);
        lastPageGameObject.SetActive(true);

        isLocked = false;
    }
}
