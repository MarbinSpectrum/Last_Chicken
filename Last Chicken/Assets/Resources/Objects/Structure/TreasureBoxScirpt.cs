using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBoxScirpt : StructureObject
{
    GameObject light;

    public static bool useTreasureBoxRadar;

    #region[Awake]
    public override void Awake()
    {
        base.Awake();
        light = transform.Find("Body").Find("Light").gameObject;
    }
    #endregion

    #region[Start]
    public override void Start()
    {
        base.Start();
    }
    #endregion

    #region[Update]
    public override void Update()
    {
        base.Update();
        if (IsAtTerrain(bodyCollider))
            rigidbody2D.simulated = false;
        else
            rigidbody2D.simulated = true;
        light.SetActive(useTreasureBoxRadar);
    }
    #endregion

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
    }
    #endregion

}
