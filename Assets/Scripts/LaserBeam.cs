using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LaserBeam2D
{
    private Vector2 pos, dir;
    private Material material;
    private GameObject laserObjInternal;
    private LineRenderer laser;
    private List<Vector3> laserIndices = new List<Vector3>();
    public GameObject laserObj => laserObjInternal;

    public bool isCheckpointHit = false;
    public bool isTargetHit = false;
    public string laserTag;
    private int maxBounces = 100;
    private List<Transform> mirrorListRef;

    public LaserBeam2D(Vector2 pos, Vector2 dir, Material material, string tag, List<Transform> mirrorListRef)
    {
        this.pos = pos;
        this.dir = dir.normalized;
        this.material = material;
        this.laserTag = tag;
        this.mirrorListRef = mirrorListRef;

        laserObjInternal = new GameObject("Laser Beam 2D");
        laserObjInternal.tag = tag;

        laser = laserObjInternal.AddComponent<LineRenderer>();
        laser.material = material;
        laser.startWidth = 0.1f;
        laser.endWidth = 0.1f;
        laser.sortingOrder = 5;

        CastRay(pos, dir);
    }

    public void CastRay(Vector2 startPos, Vector2 direction)
    {
        foreach (Transform child in laserObjInternal.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        laserIndices.Clear();
        laserIndices.Add(startPos);
        Vector2 currentPos = startPos;
        Vector2 currentDir = direction;

        for (int i = 0; i < maxBounces; i++)
        {
            int layerMask = ~(1 << LayerMask.NameToLayer("IgnoreLaser"));
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, 100f, layerMask);

            if (hit.collider != null)
            {
                GameObject hitObj = hit.collider.gameObject;
                string checkpointTag = laserTag + "Checkpoint";
                string targetTag = laserTag + "Target";
                string buttonTag = laserTag + "Button";

                if (hitObj.CompareTag(laserTag) && hit.collider.transform.root == laserObjInternal.transform)
                {
                    currentPos = hit.point + currentDir * 0.01f;
                    continue;
                }

                if (hitObj.tag.EndsWith("Laser") && !hitObj.CompareTag(laserTag))
                {
                    Debug.Log($"{laserTag} başka lazerle çarpıştı!");
                    laserIndices.Add(hit.point);
                    CreateSegmentCollider(currentPos, hit.point);
                    break;
                }

                if (hitObj.CompareTag(checkpointTag))
                {
                    if (!isCheckpointHit)
                    {
                        isCheckpointHit = true;
                        Debug.Log($"{laserTag} lazer CHECKPOINT’E ULAŞTI!");
                    }
                    CreateSegmentCollider(currentPos, hit.point);
                    currentPos = hit.point + currentDir * 0.05f;
                    continue;
                }

                if (hitObj.CompareTag(targetTag))
                {
                    if (!isCheckpointHit)
                    {
                        Debug.LogError($"{laserTag} lazer CHECKPOINT’e uğramadan hedefe ulaştı! HATA!");
                    }
                    else
                    {
                        if (!isTargetHit)
                        {
                            isTargetHit = true;
                            Debug.Log($"{laserTag} lazer HEDEF’E ULAŞTI! TAMAMLANDI!");
                        }
                    }

                    laserIndices.Add(hit.point);
                    CreateSegmentCollider(currentPos, hit.point);
                    break;
                }

                if (hitObj.CompareTag("Block"))
                {
                    Debug.Log($"{laserTag} BLOCK objesine çarptı, lazer kesildi.");
                    laserIndices.Add(hit.point);
                    CreateSegmentCollider(currentPos, hit.point);
                    break;
                }

                if (hitObj.CompareTag("Mirror"))
                {
                    if (!mirrorListRef.Contains(hitObj.transform))
                        mirrorListRef.Add(hitObj.transform);
                    laserIndices.Add(hit.point);
                    CreateSegmentCollider(currentPos, hit.point);
                    currentPos = hit.point;
                    
                    currentDir = Vector2.Reflect(currentDir, hit.normal);
                    currentPos = currentPos + (currentDir / 2);
                    
                    
                    continue;
                }
                else if (hitObj.CompareTag(buttonTag))
                {
                    hitObj.GetComponent<ButtonTrigger>().MoveableObstacle.Play();
                }

                laserIndices.Add(hit.point);
                CreateSegmentCollider(currentPos, hit.point);
                break;
            }
            else
            {
                Vector2 endPoint = currentPos + currentDir * 100f;
                laserIndices.Add(endPoint);
                CreateSegmentCollider(currentPos, endPoint);
                break;
            }
        }

        laser.positionCount = laserIndices.Count;
        for (int i = 0; i < laserIndices.Count; i++)
            laser.SetPosition(i, laserIndices[i]);
    }

    private void CreateSegmentCollider(Vector2 start, Vector2 end)
    {
        Vector2 dir = end - start;
        float length = dir.magnitude;
        if (length < 0.01f) return;

        GameObject segmentCollider = new GameObject("LaserSegmentCollider2D");
        segmentCollider.transform.parent = laserObjInternal.transform;

        BoxCollider2D box = segmentCollider.AddComponent<BoxCollider2D>();
        Vector2 midPoint = (start + end) / 2f;
        segmentCollider.transform.position = midPoint;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        segmentCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
        box.size = new Vector2(length, 0.1f);
        box.isTrigger = true;
        segmentCollider.tag = laserTag;
    }
}
