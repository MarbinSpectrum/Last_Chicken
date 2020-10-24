﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    Vector3 basePos;
    new Rigidbody2D rigidbody2D;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        basePos = transform.localPosition;
    }
    #endregion

    #region[OnEnable]
    void OnEnable()
    {
        int dic = Random.Range(0, 100) > 50 ? +1 : -1;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(new Vector2(dic * Random.Range(800, 1500), Random.Range(800, 1500)));
        rigidbody2D.angularVelocity = Random.value * 360;
    }
    #endregion

    #region[OnDisable]
    void OnDisable()
    {
        transform.localPosition = basePos;
        transform.localRotation = Quaternion.identity;
    }
    #endregion
}
