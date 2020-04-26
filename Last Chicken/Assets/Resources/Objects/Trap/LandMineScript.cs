using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineScript : TrapScript
{
    BoomScript boomScript;
    public int range;
    #region[Awake]
    public override void Awake()
    {
        base.Awake();
        boomScript = transform.Find("Piece").Find("Parts").Find("Body").GetComponent<BoomScript>();
        boomScript.damage = Mathf.FloorToInt(damage);
        boomScript.range = range;
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
    }
    #endregion

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
    }
    #endregion

    #region[활성화설정]
    public override void ObjectActive()
    {
        if (!body.activeSelf)
        {
            time += Time.deltaTime;

            if (time > 10)
                gameObject.SetActive(false);
        }
        else
            SpecialEvent();

        damageTime -= Time.deltaTime;
    }
    #endregion
}
