using UnityEngine;
using UnityEngine.EventSystems;

public class RotateButtonManual : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ManualRotator rotator;
    private bool started;

    public void OnPointerDown(PointerEventData eventData)
    {
        rotator?.BeginRotation();
        started = true;
        GameManager.Instance.MirrorInteraction = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rotator?.EndRotation();
        started = true;
        GameManager.Instance.MirrorInteraction = false;
        foreach (var laserController in GameManager.Instance.laserControllers)
        {
            laserController.DrawLaserAgain();
        }
    }
    private void Update() {
        if (started)
        {
            transform.rotation = rotator.transform.rotation;
        }
    }
}