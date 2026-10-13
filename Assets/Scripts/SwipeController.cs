using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] private Vector3 pageStep;
    [SerializeField] RectTransform allPagesRect;

    [SerializeField] private float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    private float dragThreshould;

    [SerializeField] Button previousPageButton, nextPageButton;
    AudioManager audioManager;

    private void Awake()
    {
        LeanTween.reset();
        currentPage = 1;
        targetPos = allPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
        UpdateArrowButton();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void NextPage()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
            audioManager.PlaySFX(audioManager.nextPage);
        }
        else
        {
            MovePage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
            audioManager.PlaySFX(audioManager.nextPage);
        }
        else
        {
            MovePage();
        }
    }

    public void MovePage()
    {
        allPagesRect.LeanMoveLocal(targetPos, tweenTime)
            .setEase(tweenType)
            .setIgnoreTimeScale(true);
        UpdateArrowButton();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) PreviousPage();
            else NextPage();
        }
        else
        {
            MovePage();
        }
    }

    void UpdateArrowButton()
    {
        if (nextPageButton != null)
        {
            nextPageButton.interactable = (currentPage < maxPage);
            if (maxPage <= 1)
            {
                nextPageButton.gameObject.SetActive(false);
            }
        }
        if (previousPageButton != null)
        {
            previousPageButton.interactable = (currentPage > 1);
            if (maxPage <= 1)
            {
                previousPageButton.gameObject.SetActive(false);
            }
        }
    }
}

