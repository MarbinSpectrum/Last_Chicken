using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineScript : TrapScript
{
    GameObject boom;

    BoomScript boomScript;

    public int range;
    public float speed;

    #region[Awake]
    public override void Awake()
    {
        base.Awake();
        boom = transform.Find("Piece").Find("Parts").Find("Body").gameObject;
        boomScript = boom.GetComponent<BoomScript>();
        boomScript.damage = Mathf.FloorToInt(damage);
        boomScript.range = range;
        boom.GetComponent<Animator>().speed = speed;
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
        boom.SetActive(true);
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
