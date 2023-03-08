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
    [SerializeField] public Color gizmosColor = Color.white;
    [SerializeField] public float lineThickness = 3f;
    [SerializeField] public float pointRadius = 1f;

    [SerializeField]
    private List<Transform> points = new List<Transform>();
    
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
            Vector3 prevP = Vector3.zero;

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
        point.transform.position = Vector3.zero;
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
