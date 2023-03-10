using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private RouteMaster rm;
    private Transform[] points;

    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private float detectRadius = 0.1f;

    private uint nextPoint = 1;
    
    private void Start()
    {
        if (rm != null)
            points = rm.points.ToArray();
        else
            points = new Transform[0];
        
        if (points.Length > 0 )
            transform.position = points[0].position;
    }

    private void Update()
    {
        if (rm != null)
        {
            Vector3 dir = points[nextPoint].position - transform.position;

            if (dir.sqrMagnitude > detectRadius)
            {
                transform.Translate(0, 0, moveSpeed * Time.deltaTime);
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);
            }
            else
            {

                if (nextPoint + 1 == points.Length)
                {
                    Debug.Log("Route completed");
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                else
                {
                    Debug.Log("Point reached");
                    nextPoint++;
                }
            }

        }
    }
}
