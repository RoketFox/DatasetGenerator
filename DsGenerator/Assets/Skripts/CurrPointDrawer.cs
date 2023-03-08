using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CurrPointDrawer : MonoBehaviour
{
    private RouteMaster master;
    [SerializeField] private bool getDataFromMaster = true;
    [SerializeField] private Color gizmosColor = Color.white;
    [SerializeField] private float pointRadius = 1f;
    
    public Image icon;
    
    private Color currColor;
    private float currRadius;
    
    
    private void OnDrawGizmosSelected()
    {
        if (getDataFromMaster)
        {
            master = GetComponentInParent<RouteMaster>();

            currColor = master.gizmosColor;
            currRadius = master.pointRadius;
        }
        else
        {
            currColor = gizmosColor;
            currRadius = pointRadius;
        }

        Gizmos.color = currColor;
        Gizmos.DrawSphere(transform.position, currRadius);
    }
}
