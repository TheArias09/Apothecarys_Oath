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
    [SerializeField] GrimoirePageAnchor firstPageAnchor;
    [SerializeField] GrimoirePageAnchor lastPageAnchor;

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
        firstPageAnchor.DeactivateAnchor();
        middlePageFront.Clear();
        middlePageBack.Clear();
        lastPage.DisplayData(grimoirePageDatas[0]);
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

        middlePageFront.DisplayData(grimoirePageDatas[currentLeftPageIndex]);
        middlePageBack.DisplayData(grimoirePageDatas[currentLeftPageIndex + 1]);

        if (currentIndex != maxIndex - 1)
        {
            lastPage.DisplayData(grimoirePageDatas[currentLeftPageIndex + 1]);
        }
        else
        {
            lastPageAnchor.DeactivateAnchor();
            lastPageGameObject.SetActive(false);
            lastPage.Clear();
        }

        yield return StartCoroutine(grimoirePageAnimation.FromRightToLeft());

        currentIndex++;

        firstPage.DisplayData(grimoirePageDatas[currentLeftPageIndex]);
        firstPageGameObject.SetActive(true);
        firstPageAnchor.ActivateAnchor();

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

        middlePageFront.DisplayData(grimoirePageDatas[currentLeftPageIndex - 1]);
        middlePageBack.DisplayData(grimoirePageDatas[currentLeftPageIndex]);

        if (currentIndex != 1)
        {
            firstPage.DisplayData(grimoirePageDatas[currentLeftPageIndex - 1]);
        }
        else
        {
            firstPageAnchor.DeactivateAnchor();
            firstPageGameObject.SetActive(false);
            firstPage.Clear();
        }

        yield return StartCoroutine(grimoirePageAnimation.FromLeftToRight());

        currentIndex--;

        lastPage.DisplayData(grimoirePageDatas[currentLeftPageIndex]);
        lastPageGameObject.SetActive(true);
        lastPageAnchor.ActivateAnchor();

        isLocked = false;
    }
}
