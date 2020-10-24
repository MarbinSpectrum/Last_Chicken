using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : CustomCollider
{
    public BoxCollider2D check;

    [Range(-0.9f,0.9f)]
    public float power;

}
