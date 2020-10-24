using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineScript : TrapScript
{
    GameObject boom;

    BoomScript boomScript;

    public int range;
    public float speed;

    public float soundtime = 0;
    public float soundFlag = 0.5f;

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
        BombSound();
    }
    #endregion

    #region[OnEnable]
    public override void OnEnable()
    {
        base.OnEnable();
        boom.SetActive(true);
        soundFlag = 0.5f;
        soundtime = 0;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

    #region[폭탄소리]
    public void BombSound()
    {
        if (nowHp > 0 || !boom.activeSelf)
            return;
        soundtime += Time.deltaTime;
        if (soundtime > soundFlag)
        {
            soundtime = 0;
            soundFlag *= 0.55f;
            SoundManager.instance.BombCount();
        }
    }
    #endregion
}
