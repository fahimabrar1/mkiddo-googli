using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using DG.Tweening;

public class SwipeSlider : MonoBehaviour, IEndDragHandler
{

    public Scrollbar Scrollbar;

    public Transform PagesParent;

    public Button NextPageButton;
    public Button PreviousPageButton;
    public List<GameObject> Pages = new();


    // public GameObject DotPrefab;
    // public Transform DotParentLayout;

    // public Sprite ActiveDotSprite;
    // public Sprite InActiveDotSprite;

    [SerializeField]
    private int activeIndex;

    [SerializeField]
    private float transitionTime;

    // [SerializeField]
    // private List<Image> dotsImage = new List<Image>();




    void Awake()
    {
        for (int i = 0; i < PagesParent.childCount; i++)
        {
            Pages.Add(PagesParent.GetChild(i).gameObject);
        }
    }

    void Start()
    {
        activeIndex = 0;
        Scrollbar.value = activeIndex;
        PreviousPageButton.gameObject.SetActive(false);
        NextPageButton.gameObject.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float diff = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        Debug.Log($"DIF: {diff},Abs Diff: {Mathf.Abs(diff)}");

        if (Mathf.Abs(diff) > 0)
        {
            if (diff > 0 && activeIndex < Pages.Count - 1)
            {
                GoToNextPage();
            }
            else if (diff < 0 && activeIndex > 0)
            {
                GoToPreviousPage();
            }
        }
    }


    public void GoToNextPage()
    {
        PreviousPageButton.gameObject.SetActive(true);

        activeIndex++;
        if (activeIndex == Pages.Count - 1)
            NextPageButton.gameObject.SetActive(false);
        Debug.Log("Swiped Left. New Active Index: " + activeIndex);
        JumpToPage(activeIndex);
    }

    public void GoToPreviousPage()
    {
        NextPageButton.gameObject.SetActive(true);

        activeIndex--;
        if (activeIndex == 0)
            PreviousPageButton.gameObject.SetActive(false);
        Debug.Log("Swiped Right. New Active Index: " + activeIndex);
        JumpToPage(activeIndex);
    }

    public void JumpToPage(int pageIndex)
    {
        float targetValue = pageIndex * 1.0f / (Pages.Count - 1) * 1.0f;
        Debug.Log("Jumping to page: " + pageIndex + " with target value: " + targetValue);
        DOTween.To(() => Scrollbar.value, x => Scrollbar.value = x, targetValue, 0.2f);
    }

    internal void SetButtons()
    {
        if (Pages.Count > 1)
            NextPageButton.gameObject.SetActive(true);
    }

    internal void GoToStart()
    {
        activeIndex = 0;
        PreviousPageButton.gameObject.SetActive(false);
        if (Pages.Count > 1)
            NextPageButton.gameObject.SetActive(true);
        else
            NextPageButton.gameObject.SetActive(false);
        Debug.Log("Swiped Right. New Active Index: " + activeIndex);
        JumpToPage(activeIndex);
    }
}



