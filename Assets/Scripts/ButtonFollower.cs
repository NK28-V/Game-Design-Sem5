using UnityEngine;

public class ButtonFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 screenOffset;

    void Update()
    {
        if (target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
            transform.position = screenPos + screenOffset;
        }
    }
}