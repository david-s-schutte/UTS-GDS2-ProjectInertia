using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RailMesh2D : ScriptableObject
{
    [System.Serializable]
    public class Vertex
    {
        public Vector2 vert;
        public Vector2 normal;
        public float  uvs;
    }

    public Vertex[] vertices;
    public int[] lineIndices;

}
