using UnityEngine;

public class DraggableMirror : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                isDragging = true;
                Vector3 hitPoint = hit.point;
                offset = transform.position - hitPoint;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 newPos = hit.point + offset;
                
            }
        }
    }
}