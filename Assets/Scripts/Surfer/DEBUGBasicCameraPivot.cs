using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGBasicCameraPivot : MonoBehaviour
{
    private void Update() {
        Vector2 input = new(Input.GetAxis("Cam X"), -Input.GetAxis("Cam Y"));
        transform.Rotate(input.y, 0, 0, Space.Self  );
        transform.Rotate(0, input.x, 0, Space.World);
    }
}
