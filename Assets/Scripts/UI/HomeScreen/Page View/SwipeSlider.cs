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

    public List<GameObject> Pages = new List<GameObject>();


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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float diff = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        Debug.Log("DIF: " + diff);

        if (Mathf.Abs(diff) > 0.2f)
        {
            if (diff > 0 && activeIndex < Pages.Count - 1)
            {
                activeIndex++;
                Debug.Log("Swiped Left. New Active Index: " + activeIndex);
                JumpToPage(activeIndex);
            }
            else if (diff < 0 && activeIndex > 0)
            {
                activeIndex--;
                Debug.Log("Swiped Right. New Active Index: " + activeIndex);
                JumpToPage(activeIndex);
            }
        }
    }

    public void JumpToPage(int page)
    {
        float targetValue = (page * 1.0f / (Pages.Count - 1) * 1.0f);
        Debug.Log("Jumping to page: " + page + " with target value: " + targetValue);
        DOTween.To(() => Scrollbar.value, x => Scrollbar.value = x, targetValue, 0.2f);
    }
}



