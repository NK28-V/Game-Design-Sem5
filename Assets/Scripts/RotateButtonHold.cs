using UnityEngine;
using UnityEngine.EventSystems;

public class RotateButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform targetMirror;
    public float rotationSpeed = 90f;
    private bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    void Update()
    {
        if (isHolding && targetMirror != null)
        {
            targetMirror.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}