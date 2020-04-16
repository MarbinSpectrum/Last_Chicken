using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class EffectManager : ObjectPool
{
    public static EffectManager instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject land;
    GameObject highLand;
    GameObject[] attackEffect = new GameObject[3];
    GameObject chickenFeather;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject digGround;
    GameObject plopFluid;
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject explosion;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject[] damageBlood = new GameObject[2];
    GameObject[] dieBlood = new GameObject[2];

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject getItem;
    GameObject glitter;
    public Color getMineColor;
    public float intensity;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject dust;
    List<GameObject> dustList = new List<GameObject>();
    float dustCheckTime = 0;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[진동 데이터]
    [System.Serializable]
    public class VibrationData
    {
        public int num;
        public float power;
    }

    public VibrationData monsterDeadVibration = new VibrationData();
    public VibrationData boomExplosionVibration = new VibrationData();

    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            land = Resources.Load("Graphics/Effects/Player/Land") as GameObject;
            highLand = Resources.Load("Graphics/Effects/Player/HighLand") as GameObject;
            for (int i = 0; i < attackEffect.Length; i++)
                attackEffect[i] = Resources.Load("Graphics/Effects/Player/Attack/AttackEffect" + i) as GameObject;
            chickenFeather = Resources.Load("Graphics/Effects/Feather/ChickenFeather") as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            digGround = Resources.Load("Graphics/Effects/Ground/DigGround") as GameObject;
            plopFluid = Resources.Load("Graphics/Effects/Ground/PlopFluid") as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            explosion = Resources.Load("Graphics/Effects/Boom/Explosion") as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            for (int i = 0; i < damageBlood.Length; i++)
                damageBlood[i] = Resources.Load("Graphics/Effects/Blood/DamageBlood" + i) as GameObject;
            for (int i = 0; i < dieBlood.Length; i++)
                dieBlood[i] = Resources.Load("Graphics/Effects/Blood/DieBlood" + i) as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            dust = Resources.Load("Graphics/Effects/Dust/Dust") as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            getItem = Resources.Load("Graphics/Effects/UI/GetItem") as GameObject;
            glitter = Resources.Load("Graphics/Effects/Glitter/Glitter") as GameObject;
        }
    }
    #endregion

    #region[Update]
    public void Update()
    {
        ObjectAct();
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[일반착지]
    public void Land(Vector2 vector2, bool flipX = false)
    {
        string name = "Land";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(land);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }
    #endregion

    #region[높은착지]
    public void HighLand(Vector2 vector2, bool flipX = false)
    {
        string name = "HighLand";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(highLand);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }
    #endregion

    #region[공격]
    public void Attack(Vector2 vector2,bool flipX, int type = 0)
    {
        string name = "AttackEffect" + type;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(attackEffect[type]);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }

    public void Attack(Vector3 vector3, int type = 0)
    {
        Attack(vector3, false, type);
    }
    #endregion

    #region[닭깃털]
    public void ChickenFeather(Vector2 vector2, bool flipX)
    {
        string name = "ChickenFeather";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(chickenFeather);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[폭발]
    public void Explosion(Vector2 vector2, bool flipX)
    {
        string name = "Explosion";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(explosion);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }

    public void Explosion(Vector3 vector3)
    {
        Explosion(vector3, false);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[피격 출혈]
    public void DamageBlood(Vector2 vector2, bool flipX, int type = 0)
    {
        string name = "DamageBlood" + type;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(damageBlood[type]);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }

    public void DamageBlood(Vector3 vector3, int type = 0)
    {
        DamageBlood(vector3, false, type);
    }
    #endregion

    #region[사망 출혈]
    public void DieBlood(Vector2 vector2, bool flipX, int type = 0)
    {
        string name = "DieBlood" + type;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(dieBlood[type]);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }

    public void DieBlood(Vector3 vector3, int type = 0)
    {
        DieBlood(vector3, false, type);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[땅 채광]
    public void DigGround(Vector2 vector2)
    {
        DigGround(vector2, Color.white);
    }

    public void DigGround(Vector2 vector2, Color color)
    {
        string name = "DigGround";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(digGround);
            emp.transform.name = name;
            AddObject(emp);
        }
        var main = emp.GetComponent<ParticleSystem>().main;
        main.startColor = color;
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
    }
    #endregion

    #region[땅 채광]
    public void PlopFluid(Vector2 vector2)
    {
        PlopFluid(vector2, Color.white);
    }

    public void PlopFluid(Vector2 vector2, Color color)
    {
        string name = "PlopFluid";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(plopFluid);
            emp.transform.name = name;
            AddObject(emp);
        }
        var main = emp.GetComponent<ParticleSystem>().main;
        main.startColor = color;
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[아이템획득]

    public void GetItem(Vector2 vector2, bool flipX, string itemName)
    {
        GetItem(vector2, Vector2.zero, flipX, ItemManager.instance.itemData[ItemManager.FindData(itemName)].itemImg);
    }
    public void GetItem(Vector2 vector2, Vector2 size, bool flipX, string itemName)
    {
        if (ItemManager.FindData(itemName) == -1)
            return;

        GetItem(vector2, size, flipX, ItemManager.instance.itemData[ItemManager.FindData(itemName)].itemImg);

    }

    public void GetItem(Vector2 vector2, bool flipX, Sprite sprite)
    {
        GetItem(vector2, Vector2.zero, flipX, sprite);
    }

    public void GetItem(Vector2 vector2,Vector2 size, bool flipX, Sprite sprite)
    {
        if (sprite == null)
            return;
        string name = "GetItem";

        GameObject emp = FindObject(name,true);

        if (emp == null)
        {
            emp = Instantiate(getItem);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.GetComponent<SpriteRenderer>().sprite = sprite;
        emp.GetComponent<Follow>().followUI = UIManager.instance.activeItemImg.transform.GetChild(0).gameObject;
        if(size != Vector2.zero)
            emp.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = size;

        UIManager.instance.nowItem.SetActive(true);
        UIManager.instance.nowItemAnimator.SetTrigger("Change");
    }
    #endregion

    #region[광물획득]
    public void Glitter(Vector2 vector2, Color color)
    {
        string name = "Glitter";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(glitter);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
    }

    public void Glitter(Vector3 vector3)
    {
        Glitter(vector3, Color.white);
    }
    #endregion

    #region[먼지]
    public void Dust(Vector2 vector2, Vector2 size)
    {
        string name = "Dust";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(dust);
            dustList.Add(emp);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(size.x, size.y, 1);
    }

    public void Dust(Vector2 vector2)
    {
        float size = Random.Range(0.3f, 1f);
        Dust(vector2, new Vector2(size, size));
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[진동이펙트]
    public void Vibration(int num, float power)
    {
        StartCoroutine(VibrationAct(num, power));
    }

    private IEnumerator VibrationAct(int num, float power)
    {
        Vector3 basePos = Camera.main.transform.position;
        for (int i = 0; i < num; i++)
        {
            Vector2 dic = Vector2.right * power;
            dic = Quaternion.Euler(0, 0, Random.Range(0, 360)) * dic;
            Camera.main.transform.position = basePos + new Vector3(dic.x, dic.y, 0);
            yield return new WaitForSeconds(0.01f);
        }
        Camera.main.transform.position = basePos;
    }
    #endregion

    #region[하트이펙트]
    public void HearthEffect()
    {
        for (int i = 0; i < UIManager.instance.hearthData.Count; i++)
            UIManager.instance.hearthData[i].hearthAni.SetTrigger("Act");
    }
    #endregion

    #region[데미지 이펙트]
    public void DamageEffect()
    {
        UIManager.instance.playerStateAni.SetTrigger("Act");
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[멀리있는 오브젝트 비활성화]
    public override void ObjectAct()
    {
        if (!GameManager.instance.InGame())
            return;

        dustCheckTime += Time.deltaTime;

        if (dustCheckTime <= 1)
            return;

        dustCheckTime = 0;

        for (int i = 0; i < dustList.Count; i++)
        {
            Vector3 targetScreenPos = Camera.main.WorldToViewportPoint(dustList[i].transform.position);

            Vector2 size = new Vector2(1.5f, 1.5f);

            bool flag =
                !(
                        targetScreenPos.x > (1 + size.x) / 2f ||
                        targetScreenPos.x < (1 - size.x) / 2f ||
                        targetScreenPos.y > (1 + size.y) / 2f ||
                        targetScreenPos.y < (1 - size.x) / 2f
                );

            if (dustList[i].activeSelf != flag)
                dustList[i].SetActive(flag);
        }
    }
    #endregion
}