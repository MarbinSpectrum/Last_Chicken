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

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[오브젝트 피격처리]
    public override void ObjectBreak(int n)
    {
        damageTime = 0.1f;
        nowHp -= n;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(new Vector2(0, 25), ForceMode2D.Impulse);
        if (nowHp <= 0)
        {
            if (specialType == SpecialType.아이템드랍)
            {
                if(inItem.Equals("Random"))
                    ItemManager.instance.SpawnItemRandomAtTreasureBox(transform.position);
                else
                    ItemManager.instance.SpawnItem(transform.position, inItem);
            }

            body.SetActive(false);
            piece.SetActive(true);
        }
        else
        {
            StartCoroutine(Vibration(10));
        }
    }
    #endregion
}
