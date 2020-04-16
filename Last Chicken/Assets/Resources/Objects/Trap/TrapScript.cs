using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapScript : StructureObject
{
    GameObject light;

    public static bool useTrapRadar;

    public float damage;

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
        light.SetActive(useTrapRadar);
    }
    #endregion

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
    }
    #endregion

}
