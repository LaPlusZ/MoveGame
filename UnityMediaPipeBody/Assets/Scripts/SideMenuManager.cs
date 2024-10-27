using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class SideMenuManager : MonoBehaviour
{
    [SerializeField] List<GameObject> menus;
    public int pageOpening;
    public bool opening;
    public GameObject menusParent;
    
    Vector2 orgPos;
    RectTransform rt;
    bool db;

    void Start()
    {
        rt = menusParent.GetComponent<RectTransform>();
        orgPos = rt.anchoredPosition;
        rt.anchoredPosition = new Vector2(-rt.anchoredPosition.x, rt.anchoredPosition.y);
        foreach (GameObject page in menus) { page.SetActive(false); }
    }

    public async void open(int i)
    {
        if (opening == false && db == false)
        {
            db = true;
            menus[i].SetActive(true);
            await rt.DOAnchorPosX(orgPos.x, 1).SetEase(Ease.InOutQuart).AsyncWaitForCompletion();
            pageOpening = i;
            opening = true;
            db = false;
        }
        else if (opening == true && db == false && pageOpening != i)
        {
            db = true;
            await rt.DOAnchorPosX(-orgPos.x, 0.5f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
            foreach (GameObject page in menus) { page.SetActive(false); }
            menus[i].SetActive(true);
            await rt.DOAnchorPosX(orgPos.x, 0.5f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            pageOpening = i;
            opening = true;
            db = false;
        }
        else if (opening == true && db == false && pageOpening == i)
        {
            db = true;
            await rt.DOAnchorPosX(-orgPos.x, 1).SetEase(Ease.InOutQuart).AsyncWaitForCompletion();
            foreach (GameObject page in menus) { page.SetActive(false); }
            opening = false;
            db = false;
        }
    }

    public async void close()
    {
        if (opening == true && db == false)
        {
            db = true;
            await rt.DOAnchorPosX(-orgPos.x, 1).SetEase(Ease.InOutQuart).AsyncWaitForCompletion();
            foreach (GameObject page in menus) { page.SetActive(false); }
            opening = false;
            db = false;
        }
    }
}
