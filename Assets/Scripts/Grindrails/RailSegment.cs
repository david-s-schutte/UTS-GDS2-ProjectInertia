#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
public class RailSegment : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float t = 0;
    [SerializeField] Transform[] controlPoints = new Transform[4];

    [SerializeField] RailMesh2D shape2D;
    Vector3 GetPos(int i) => controlPoints[i].position;



    public void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawSphere(GetPos(i), 0.05f);
        }

        OrientedPoint testPoint = GetBezierOP(t);
        Handles.DrawBezier(GetPos(0), GetPos(3), GetPos(1), GetPos(2),
            Color.white, EditorGUIUtility.whiteTexture, 2f);

        void DrawPoint(Vector2 localPos) => Gizmos.DrawSphere(testPoint.LocalToWorld(localPos), 0.1f);

        for(int i = 0; i < shape2D.vertices.Length; i++)
        {
            DrawPoint(shape2D.vertices[i].vert);
        }

        Gizmos.color = Color.red;
       
        Gizmos.DrawSphere(testPoint.pos, 0.03f);

        Handles.PositionHandle(testPoint.pos, testPoint.rot);
        Gizmos.DrawSphere(testPoint.LocalToWorld(Vector3.right * 0.2f), 0.01f);

        Gizmos.color = Color.white;

    }



    OrientedPoint GetBezierOP(float t) //oriented point
    {
        Vector3 p0 = GetPos(0);
        Vector3 p1 = GetPos(1);
        Vector3 p2 = GetPos(2);
        Vector3 p3 = GetPos(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 pos = Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;

        return new OrientedPoint(pos, tangent);
    }
}
#endif