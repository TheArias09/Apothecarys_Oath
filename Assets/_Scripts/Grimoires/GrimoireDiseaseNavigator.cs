using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimoireDiseaseNavigator : MonoBehaviour
{
    [SerializeField] GrimoireDiseasePage firstPage;
    [SerializeField] GrimoireDiseasePage lastPage;
    [SerializeField] GrimoireDiseasePage middlePageFront;
    [SerializeField] GrimoireDiseasePage middlePageBack;

    [SerializeField] GameObject firstPageGameObject;
    [SerializeField] GameObject lastPageGameObject;
    [SerializeField] GrimoirePageAnchor firstPageAnchor;
    [SerializeField] GrimoirePageAnchor lastPageAnchor;

    [SerializeField] List<GrimoireDiseasePageData> grimoirePageDatas;

    [SerializeField] bool isTutoGrimoire = false;

    private bool isLocked = false;
    private int currentIndex = 0;
    private int maxIndex => Mathf.CeilToInt((float)(grimoirePageDatas.Count - 1) / 2);
    private int currentLeftPageIndex => currentIndex * 2 - 1;

    private GrimoirePageAnimation grimoirePageAnimation;
    private AudioSource audioSource;

    private void Awake()
    {
        grimoirePageAnimation = GetComponent<GrimoirePageAnimation>();
        audioSource = GetComponent<AudioSource>();

        firstPage.Clear();
        if (!isTutoGrimoire)
        {
            firstPageGameObject.SetActive(false);
            firstPageAnchor.DeactivateAnchor();
            lastPage.DisplayData(grimoirePageDatas[0]);
        }
        else
        {
            currentIndex++;
            firstPage.DisplayData(grimoirePageDatas[1]);
            lastPage.DisplayData(grimoirePageDatas[2]);
        }
        middlePageFront.Clear();
        middlePageBack.Clear();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryToGoToNextPage();
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
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
        audioSource.Play();

        middlePageFront.DisplayData(grimoirePageDatas[currentLeftPageIndex + 1]);
        middlePageBack.DisplayData(grimoirePageDatas[currentLeftPageIndex + 2]);

        if (currentIndex != maxIndex - 1)
        {
            lastPage.DisplayData(grimoirePageDatas[currentLeftPageIndex + 3]);
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
        audioSource.Play();

        middlePageFront.DisplayData(grimoirePageDatas[currentLeftPageIndex - 1]);
        middlePageBack.DisplayData(grimoirePageDatas[currentLeftPageIndex]);
        
        if(currentIndex != 1)
        {
            firstPage.DisplayData(grimoirePageDatas[currentLeftPageIndex - 2]);
        }
        else
        {
            firstPageAnchor.DeactivateAnchor();
            firstPageGameObject.SetActive(false);
            firstPage.Clear();
        }

        yield return StartCoroutine(grimoirePageAnimation.FromLeftToRight());

        currentIndex--;

        lastPage.DisplayData(grimoirePageDatas[currentLeftPageIndex + 1]);
        lastPageGameObject.SetActive(true);
        lastPageAnchor.ActivateAnchor();

        isLocked = false;
    }
}
