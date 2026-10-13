using UnityEngine;

public class ManualRotator : MonoBehaviour
{
    private bool isRotating = false;
    private float lastX;
    [SerializeField] float sensivity = 2;

    public void BeginRotation()
    {
        isRotating = true;
        lastX = Input.mousePosition.x; 
    }

    public void EndRotation()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            float delta = Input.mousePosition.x - lastX;
            lastX = Input.mousePosition.x;
            transform.Rotate(0, 0, delta * 0.3f * sensivity);
        }
    }
}