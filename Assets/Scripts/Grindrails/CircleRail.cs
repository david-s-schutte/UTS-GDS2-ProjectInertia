using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRail : MonoBehaviour
{
    [SerializeField] RailMesh2D shape2D;

    [Range(2, 32)]
    [SerializeField] int edgeCount = 16;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMesh()
    {

    }
}
