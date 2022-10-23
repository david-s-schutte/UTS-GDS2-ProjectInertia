using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindRailController : MonoBehaviour
{
    public List<Transform> GetNodes() {
        Transform[] nodes = GetComponentsInChildren<Transform>();
        List<Transform> nodeList = new List<Transform>(nodes);
        nodeList.RemoveAt(0);
        return nodeList;
    }

    public List<Transform> GetRemainingNodes(Transform fromPoint) {
        List<Transform> nodes = GetNodes();

        int closestNodeIndex = 0;
        float closestNodeDistance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++) {
            float distance = (nodes[i].position - fromPoint.position).sqrMagnitude;
            if (distance < closestNodeDistance) {
                closestNodeIndex = i;
                closestNodeDistance = distance;
            }
        }
        
        if (closestNodeIndex > 0) {
            nodes.RemoveRange(0, closestNodeIndex);
        }

        return nodes;
    }
}
