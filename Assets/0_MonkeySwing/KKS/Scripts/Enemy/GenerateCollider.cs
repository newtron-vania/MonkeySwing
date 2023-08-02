using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCollider : MonoBehaviour
{
    [SerializeField]
    EdgeCollider2D collider;
    public void SetEdgeCollider(LineRenderer line)
    {
        List<Vector2> edges = new List<Vector2>();

        for(int i=0; i<line.positionCount; i++)
        {
            Vector3 lineRendererPoint = line.GetPosition(i);
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }
        collider.SetPoints(edges);
    }

}
