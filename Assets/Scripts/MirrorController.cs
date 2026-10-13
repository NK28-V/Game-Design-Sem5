using UnityEngine;

public class MirrorController : MonoBehaviour
{
    private Vector3 lastPosition;
    private bool hasBeenUsed = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (transform.position != lastPosition)
        {
            LineController[] controllers = FindObjectsOfType<LineController>();
            foreach (var controller in controllers)
            {
                if (controller.currentLaserBeam != null && controller.affectedMirrors.Contains(transform))
                {
                    Vector3 startPos = controller.laserPointer.position;
                    Vector3 dir = (controller.GetEndPoint() - startPos).normalized;
                    controller.currentLaserBeam.CastRay(startPos, dir);

                    // Ayna kullanımını kaydet
                    if (!hasBeenUsed)
                    {
                        hasBeenUsed = true;
                        if (GameManager.Instance != null)
                        {
                            GameManager.Instance.RegisterMirrorUse();
                        }
                    }
                }
            }

            lastPosition = transform.position;
        }
    }
}