using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Class to control route points.
/// Have to be placed on empty gameobject.
/// </summary>
public class RouteMaster : MonoBehaviour
{
    [SerializeField] private bool addAtLast = true;

    public Color gizmosColor = Color.white;
    public float lineThickness = 3f;
    public float pointRadius = 1f;

    public List<Transform> points = new List<Transform>();
    
    private void OnDrawGizmos()
    {
        

        Gizmos.color = gizmosColor;

        for(int i = 0; i < points.Count; i++)
        {
            if (points[i] == null)
            {
                points.RemoveAt(i);
            }

            Vector3 currP = points[i].position;
            Vector3 prevP = transform.position;

            if (i > 0)
                prevP = points[i - 1].position;
            else if (i == 0 && points.Count > 1)
                prevP = points[points.Count - 1].position;

            Handles.DrawBezier(prevP, currP, prevP, currP, gizmosColor, null, lineThickness);
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawSphere(points[i].position, pointRadius);
        }
    }

    public void CreatePoint()
    {
        GameObject point = new GameObject("Point" + points.Count);
        point.transform.parent = transform;
        
        if (addAtLast)
            if (points.Count > 0)
                point.transform.position = points[points.Count - 1].position;
            else
                point.transform.position = transform.position;
        else
            point.transform.position = transform.position;
        
        point.AddComponent<CurrPointDrawer>();
        points.Add(point.transform);
    }

    public void DeleteLastPoint()
    {
        if (points.Count > 0)
        {
            DestroyImmediate(points[points.Count - 1].gameObject);
            points.RemoveAt(points.Count - 1);
        }
    }

    public void DeleteAllPoints()
    {
        if (points.Count > 0)
        {
            foreach (Transform point in points)
            {
                DestroyImmediate(point.gameObject);
            }
            points.Clear();
        }
    }
}
