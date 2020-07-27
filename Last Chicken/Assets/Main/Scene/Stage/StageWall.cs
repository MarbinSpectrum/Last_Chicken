using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StageWall : MonoBehaviour
{
    public static StageWall instance;
    public void Awake()
    {
        instance = this;
    }

}
