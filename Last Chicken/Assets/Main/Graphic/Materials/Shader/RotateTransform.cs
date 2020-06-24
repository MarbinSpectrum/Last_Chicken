using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RotateTransform : MonoBehaviour
{
    public float dic = 1;
    public float angle = 0;
    void Update()
    {
        angle += dic * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
