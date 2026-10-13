using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    public Material laserMaterial;
    public Material aimLineMaterial;
    public Transform laserPointer;
    public string laserTag;

    private LineRenderer aimLine;
    private List<Vector3> drawnPoints = new List<Vector3>();
    private bool isDrawing = false;

    public LaserBeam2D currentLaserBeam;
    private int updateFrameCount = 0;
    private static LineController activeController = null;

    public float drawDuration = 5f;
    private float drawTimeLeft = 0f;
    
    private Slider drawTimerSlider;
    public List<Transform> affectedMirrors = new List<Transform>();
    private Transform lastSelected;
    private Vector3 lastDir;


    void Start()
    {
        aimLine = gameObject.AddComponent<LineRenderer>();
        aimLine.material = aimLineMaterial;
        aimLine.startWidth = 0.05f;
        aimLine.endWidth = 0.05f;
        aimLine.positionCount = 0;

        
        
    }

    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
            {
                if (hit.collider.CompareTag("MirrorClickArea") && lastSelected != hit.transform)
                {
                    Transform targetMirror = hit.collider.transform.root;


                    foreach (GameObject btn in GameObject.FindGameObjectsWithTag("RotateButton"))
                        Destroy(btn);
                    foreach (GameObject btn in GameObject.FindGameObjectsWithTag("MoveButton"))
                        Destroy(btn);


                    Vector3 screenPos = Camera.main.WorldToScreenPoint(targetMirror.position + new Vector3(0.5f, 0.5f, 0));
                    Canvas canvas = FindObjectOfType<Canvas>();


                    GameObject rotateBtn = Instantiate(Resources.Load<GameObject>("RotateButton"), canvas.transform);

                    rotateBtn.tag = "RotateButton";
                    rotateBtn.GetComponent<RotateButtonManual>().rotator = targetMirror.GetComponent<ManualRotator>();
                    Texture texture = targetMirror.GetComponent<MeshRenderer>().materials[0].GetTexture("_MainTex");
                    Sprite sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    rotateBtn.GetComponent<Image>().sprite = sprite;





                    GameObject moveBtn = Instantiate(Resources.Load<GameObject>("MoveButton"), canvas.transform);
                    moveBtn.transform.position = screenPos + new Vector3(0, 0, 0);
                    moveBtn.tag = "MoveButton";
                    moveBtn.GetComponent<MoveButtonController>().targetMirror = targetMirror;

                    moveBtn.AddComponent<ButtonFollower>().target = targetMirror;
                    moveBtn.GetComponent<ButtonFollower>().screenOffset = new Vector3(0, 0, 0);
                    moveBtn.GetComponent<MoveButtonController>().buttonFollower = moveBtn.GetComponent<ButtonFollower>();

                    lastSelected = hit.transform;
                    return;
                }
            }


            float myDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)), laserPointer.position);

            if (activeController == null || myDistance < Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)), activeController.laserPointer.position))
            {
                activeController = this;
            }

            if (activeController == this)
            {
                StartCoroutine(DrawTimeSliderActivate());
            }
        }

        if (isDrawing && Input.GetMouseButton(0) && activeController == this && !GameManager.Instance.MirrorInteraction)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                if (drawnPoints.Count == 0 || Vector3.Distance(drawnPoints[drawnPoints.Count - 1], point) > 0.1f)
                {
                    drawnPoints.Add(point);
                    aimLine.positionCount = drawnPoints.Count;
                    aimLine.SetPositions(drawnPoints.ToArray());
                }
            }

            drawTimeLeft -= Time.deltaTime;
            

            if (drawTimeLeft <= 0f)
            {
                isDrawing = false;
                activeController = null;

                if (drawnPoints.Count < 2) return;

                Vector3 endPoint = drawnPoints[drawnPoints.Count - 1];
                Vector3 dir = (endPoint - laserPointer.position).normalized;
                if (dir != Vector3.zero)
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
                    laserPointer.rotation = Quaternion.Euler(0, 0, angle);
                }

                if (currentLaserBeam != null)
                    GameObject.Destroy(currentLaserBeam.laserObj);

                currentLaserBeam = new LaserBeam2D(laserPointer.position, dir, laserMaterial, laserTag, affectedMirrors);


                aimLine.positionCount = 0;
                updateFrameCount = 7;

                
            }
        }

        if (Input.GetMouseButtonUp(0) && activeController == this)
        {
            isDrawing = false;
            activeController = null;

            if (drawnPoints.Count < 2) return;

            Vector3 endPoint = drawnPoints[drawnPoints.Count - 1];
            Vector3 dir = (endPoint - laserPointer.position).normalized;
            if (dir != Vector3.zero)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
                laserPointer.rotation = Quaternion.Euler(0, 0, angle);
            }

            if (currentLaserBeam != null)
                GameObject.Destroy(currentLaserBeam.laserObj);

            currentLaserBeam = new LaserBeam2D(laserPointer.position, dir, laserMaterial, laserTag, affectedMirrors);


            aimLine.positionCount = 0;
            updateFrameCount = 12;

            



        }

        if (currentLaserBeam != null && updateFrameCount > 0)
        {
            Vector3 endPoint = drawnPoints.Count > 0 ? drawnPoints[drawnPoints.Count - 1] : laserPointer.position + laserPointer.forward * 10f;
            Vector3 dir = (endPoint - laserPointer.position).normalized;
            lastDir = dir;
            currentLaserBeam.CastRay(laserPointer.position, dir);
            GameManager.Instance.ResetOtherLasers(activeController);
            updateFrameCount--;

            if (GameManager.Instance != null && !string.IsNullOrEmpty(laserTag))
            {
                GameManager.Instance.RegisterLightSource(laserTag);
            }
        }
    }
    public IEnumerator DrawTimeSliderActivate()
    {
        yield return new WaitForSeconds(0.1f);
        if (!GameManager.Instance.MirrorInteraction)
        {
            Debug.Log("time slider active " + GameManager.Instance.MirrorInteraction);
            isDrawing = true;
            drawnPoints.Clear();

            drawTimeLeft = drawDuration;
            
            
            
        }
    }
    public void DrawLaserAgain()
    {
        currentLaserBeam?.CastRay(laserPointer.position, lastDir);
    }

    public Vector3 GetEndPoint()
    {
        if (drawnPoints.Count > 0)
            return drawnPoints[drawnPoints.Count - 1];
        return laserPointer.position;
    }
}
