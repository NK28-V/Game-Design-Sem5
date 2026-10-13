using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform targetMirror;
    private bool isDragging = false;
    private Vector3 offset;
    [HideInInspector] public ButtonFollower buttonFollower;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.CheckRightToMove())
        {
            isDragging = true;
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = targetMirror.position - new Vector3(world.x, world.y, targetMirror.position.z);
            GameManager.Instance.MirrorInteraction = true;
            GameManager.Instance.DecreaseRightToMove();
            buttonFollower.enabled = false;
            Debug.Log("move");
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("on pointet up");
        isDragging = false;
        GameManager.Instance.MirrorInteraction = false;
        buttonFollower.enabled = true;
        foreach (var laserController in GameManager.Instance.laserControllers)
        {
            laserController.DrawLaserAgain();
        }
    }

    void Update()
    {
        Debug.Log(isDragging);
        if (isDragging && targetMirror != null)
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = new Vector3(world.x, world.y, targetMirror.position.z);
            targetMirror.position = newPos + offset;
            Debug.Log("position change");
        }
        else if (isDragging)
        {
            Debug.Log("position change");
        }
    }
}

