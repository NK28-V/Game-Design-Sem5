using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float speed;
    private bool isPlayed;
    void Start()
    {

    }

    void Update()
    {
        if (isPlayed)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }
    public void Play()
    {
        isPlayed = true;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPos,0.2f);
    }
}
