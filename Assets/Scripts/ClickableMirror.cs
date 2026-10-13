using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableMirror : MonoBehaviour, IPointerClickHandler
{
    public GameObject rotateButtonPrefab;
    private GameObject spawnedButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (spawnedButton != null)
        {
            Destroy(spawnedButton);
        }

        
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.5f, 0.5f, 0));
        Canvas canvas = FindObjectOfType<Canvas>();
        spawnedButton = Instantiate(rotateButtonPrefab, canvas.transform);
        spawnedButton.transform.position = screenPos;

        
        spawnedButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            transform.Rotate(0, 0, 90f);
        });
    }
}