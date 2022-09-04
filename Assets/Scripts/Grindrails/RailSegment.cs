#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
//[ExecuteInEditMode]
public class RailSegment : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float t = 0;
    [SerializeField] int nodeCount = 16;
    [SerializeField] Transform[] controlPoints = new Transform[4];

    [SerializeField] RailMesh2D shape2D;

    [Range(2, 32)]
    [SerializeField] int edgeCount = 16;

    Vector3 GetPos(int i) => controlPoints[i].position;

    Mesh mesh;
    MeshCollider collider;

    [SerializeField] GameObject nodes;
    List<Collider> railNodes;
    [SerializeField] Collider railNode;

    [SerializeField] bool isEditing = false;
    private void Awake()
    {
        mesh = new();
        mesh.name = "Rail";   
        GetComponent<MeshFilter>().sharedMesh = mesh;
        collider = new();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        if (!isEditing)
            GenerateMesh();
            transform.position -= gameObject.GetComponentInParent<Transform>().position;
       
    }

    private void Update()
    {   
        if(isEditing)
            GenerateMesh();
            transform.position -= gameObject.GetComponentInParent<Transform>().position;
       
    }

    void GenerateMesh()
    {
        railNodes = new();
        mesh.Clear();
        railNodes.Clear();

        //Vertices
        List<Vector3> verts = new();
        for(int i = 0; i < edgeCount; i++)
        {
            float t = i / (edgeCount - 1f);
            OrientedPoint op = GetBezierOP(t);
            Collider node = Instantiate(railNode, op.pos, op.rot);
            node.transform.parent = nodes.transform;
            railNodes.Add(node);
            for(int j = 0; j < shape2D.vertices.Length; j++)
            {
                //Debug.Log(shape2D.vertices[j].vert);
                
                verts.Add(op.LocalToWorld(shape2D.vertices[j].vert));
            }
        }

        //Rail nodes
        for(int i = 0; i < nodeCount; i++)
        {
            OrientedPoint op = GetBezierOP(1/nodeCount);
        }
        //Tris
        List<int> tris = new();
        
        for (int i = 0; i < edgeCount - 1; i++)
        {
            int rootIndex = i * shape2D.vertices.Length;
            int nextIndex = (i + 1) * shape2D.vertices.Length;

            for(int j = 0; j < shape2D.lineIndices.Length; j+=2)
            {
                int lineIndexA = shape2D.lineIndices[j];
                int lineIndexB = shape2D.lineIndices[j + 1];

                int currentA = rootIndex + lineIndexA;
                int currentB = rootIndex + lineIndexB;
                int nextA = nextIndex + lineIndexA;
                int nextB = nextIndex + lineIndexB;

                tris.Add(currentA);
                tris.Add(nextA);
                tris.Add(nextB);

                tris.Add(currentA);
                tris.Add(nextB);
                tris.Add(currentB);

                /**Debug.Log(currentA);
                Debug.Log(nextA);
                Debug.Log(nextB);

                Debug.Log(currentA);
                Debug.Log(nextB);
                Debug.Log(currentB);
                **/
            }
        }
        
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        collider = new();
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawSphere(GetPos(i), 0.05f);
        }

        OrientedPoint testPoint = GetBezierOP(t);
        Handles.DrawBezier(GetPos(0), GetPos(3), GetPos(1), GetPos(2),
            Color.white, EditorGUIUtility.whiteTexture, 2f);

        //void DrawPoint(Vector2 localPos) => Gizmos.DrawSphere(testPoint.LocalToWorld(localPos), 0.1f);

        Vector3[] verts = shape2D.vertices.Select(v => testPoint.LocalToWorld(v.vert)).ToArray();
        
        for(int i = 0; i < shape2D.vertices.Length; i+=2)
        {

            Vector3 a = verts[shape2D.lineIndices[i]];
            Vector3 b = verts[shape2D.lineIndices[i + 1]];
            Gizmos.DrawLine(a, b);
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