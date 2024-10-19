using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.Mathematics;

public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float buttonZoomScale = 1.2f;
    public float buttonTwistScale = -4f;
    public bool isMenuButton;
    public Color menuColor;
    public CanvasGroup icon;
    public CanvasGroup menuContent;
    public bool pauseGame;
    public ButtonAnimation parentMenu;
    public bool disableTwist;
    private RectTransform rectTransform;
    private Image image;
    private bool isMenuOn;
    private Color orgColor;

    private bool disableAnim;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (disableTwist == false) { rectTransform.localRotation = Quaternion.Euler(0,0,4.413f); }
        image = GetComponent<Image>();
        orgColor = image.color;

        if (isMenuButton) 
        {
            menuContent.gameObject.SetActive(false);
            menuContent.alpha = 0;
        }
    }

    void OnClick()
    {
        if (pauseGame && FindAnyObjectByType<UIManager>().db == false) 
        {
            if (parentMenu && FindAnyObjectByType<UIManager>().paused == true)
            {
                parentMenu.CloseMenu();
            }
            FindAnyObjectByType<UIManager>().Pause();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isMenuButton == true && isMenuOn == true) {return;}
        if (disableTwist == false) { rectTransform.DORotate(new Vector3(0,0,buttonTwistScale), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true); }
        rectTransform.DOScale(new Vector3(buttonZoomScale,buttonZoomScale,buttonZoomScale), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true);
    }

    public async void OnPointerUp(PointerEventData eventData)
    {
        if (isMenuButton == false) 
        {
            OnClick();
            
            if (disableTwist == false) { rectTransform.DOLocalRotate(new Vector3(0,0,4.413f), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true); }
            rectTransform.DOScale(new Vector3(1f,1f,1f), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true);
        }
        else if (isMenuOn == false && FindAnyObjectByType<UIManager>().paused == false)
        {
            disableAnim = true;
            menuContent.gameObject.SetActive(true);

            rectTransform.DOScale(new Vector3(1f,1f,1f), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true);
            rectTransform.DORotate(new Vector3(0,0,181), 0.8f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart).SetUpdate(true);
            rectTransform.DOAnchorPos(new Vector2(0,0), 1f).SetEase(Ease.OutQuart).SetUpdate(true);
            rectTransform.DOSizeDelta(new Vector2(1200, 490), 1f).SetEase(Ease.OutQuart).SetUpdate(true);
            DOTween.To(() => image.pixelsPerUnitMultiplier, x => image.pixelsPerUnitMultiplier = x, 8.5f, 1f).SetUpdate(true);

            OnClick();

            image.DOColor(menuColor, 1f).SetEase(Ease.OutQuart).SetUpdate(true);
            await icon.DOFade(0, 0.25f).SetUpdate(true).AsyncWaitForCompletion();
            menuContent.DOFade(1, 0.25f).SetUpdate(true);
            isMenuOn = true;
        }
    }

    public async void CloseMenu()
    {
        rectTransform.DOLocalRotate(new Vector3(0,0,4.413f), 0.8f).SetEase(Ease.InOutQuart).SetUpdate(true);
        rectTransform.DOAnchorPos(new Vector2(-882.9f,466.6f), 1f).SetEase(Ease.InOutQuart).SetUpdate(true);
        rectTransform.DOScale(new Vector3(1f,1f,1f), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true);
        rectTransform.DOSizeDelta(new Vector2(75, 75), 1f).SetEase(Ease.InOutQuart).SetUpdate(true);
        DOTween.To(() => image.pixelsPerUnitMultiplier, x => image.pixelsPerUnitMultiplier = x, 30.5f, 1f);

        await menuContent.DOFade(0, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
        icon.DOFade(1, 0.25f).SetUpdate(true);
        image.DOColor(orgColor, 0.5f).SetUpdate(true);
        menuContent.gameObject.SetActive(false);
        isMenuOn = false;
        disableAnim = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (disableAnim == true) {return;}
        if (disableTwist == false) { rectTransform.DORotate(new Vector3(0,0,(4.413f+buttonTwistScale)/2f), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true); }
        rectTransform.DOScale(new Vector3((buttonZoomScale+1)/2,(buttonZoomScale+1)/2,(buttonZoomScale+1)/2), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (disableAnim == true) {return;}
        if (disableTwist == false) { rectTransform.DOLocalRotate(new Vector3(0,0,4.413f), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true); }
        rectTransform.DOScale(new Vector3(1f,1f,1f), 0.2f).SetEase(Ease.OutQuart).SetUpdate(true);
    }
}
