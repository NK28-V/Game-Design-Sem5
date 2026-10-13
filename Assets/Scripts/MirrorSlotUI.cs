using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MirrorSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject mirrorPrefab;
    public int availableCount = 3;
    public Text countText;
    public Image iconImage;
    public Sprite mirrorSprite;

    private GameObject draggingMirror;

    void Start()
    {
        UpdateUI();
        if (iconImage != null && mirrorSprite != null)
            iconImage.sprite = mirrorSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (availableCount <= 0 || mirrorPrefab == null)
        {
            draggingMirror = null;
            return;
        }

        GameManager.Instance.MirrorInteraction = true;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        draggingMirror = Instantiate(mirrorPrefab, worldPos, Quaternion.identity);
        draggingMirror.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (draggingMirror != null)
        {
            GameManager.Instance.MirrorInteraction = true;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            draggingMirror.transform.position = worldPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingMirror != null)
        {
            draggingMirror.GetComponent<CanvasGroup>().blocksRaycasts = true;


            if (availableCount > 0)
            {
                availableCount--;
            }


            availableCount = Mathf.Max(0, availableCount);

            UpdateUI();
        }
        Debug.Log("mirror interaction false ");
        GameManager.Instance.MirrorInteraction = false;
        foreach (var laserController in GameManager.Instance.laserControllers)
        {
            laserController.DrawLaserAgain();
        }
    }

    private void UpdateUI()
    {
        countText.text = availableCount.ToString();
        GetComponent<Button>().interactable = availableCount > 0;
    }
}