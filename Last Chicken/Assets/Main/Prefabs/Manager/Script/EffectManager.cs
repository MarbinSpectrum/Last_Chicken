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
    GameObject[] lightFeather = new GameObject[3];

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject digGround;
    GameObject plopFluid;
    GameObject bubbleFluid;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject explosion;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject[] damageBlood = new GameObject[2];
    GameObject[] dieBlood = new GameObject[2];

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject getItem;
    Material dirtMat;
    Material snowMat;
    Material stoneMat;
    Material copperMat;
    Material sandMat;
    Material graniteMat;
    Material ironMat;
    Material silverMat;
    Material goldMat;
    Material mithrillMat;
    Material diamondMat;
    Material magnetiteMat;
    Material titaniumMat;
    Material cobaltMat;
    Material iceMat;
    Material grassMat;
    Material hearthStoneMat;

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
            for (int i = 0; i < 3; i++)
                lightFeather[i] = Resources.Load("Graphics/Effects/Feather/Feather" + i) as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            digGround = Resources.Load("Graphics/Effects/Ground/DigGround") as GameObject;
            dirtMat = Resources.Load("Graphics/Effects/Ground/DirtMat") as Material;
            snowMat = Resources.Load("Graphics/Effects/Ground/SnowMat") as Material;
            stoneMat = Resources.Load("Graphics/Effects/Ground/StoneMat") as Material;
            copperMat = Resources.Load("Graphics/Effects/Ground/CopperMat") as Material;
            sandMat = Resources.Load("Graphics/Effects/Ground/SandMat") as Material;
            graniteMat = Resources.Load("Graphics/Effects/Ground/GraniteMat") as Material;
            ironMat = Resources.Load("Graphics/Effects/Ground/IronMat") as Material;
            silverMat = Resources.Load("Graphics/Effects/Ground/SilverMat") as Material;
            goldMat = Resources.Load("Graphics/Effects/Ground/GoldMat") as Material;
            mithrillMat = Resources.Load("Graphics/Effects/Ground/MithrillMat") as Material;
            diamondMat = Resources.Load("Graphics/Effects/Ground/DiamondMat") as Material;
            magnetiteMat = Resources.Load("Graphics/Effects/Ground/MagnetiteMat") as Material;
            titaniumMat = Resources.Load("Graphics/Effects/Ground/TitaniumMat") as Material;
            cobaltMat = Resources.Load("Graphics/Effects/Ground/CobaltMat") as Material;
            iceMat = Resources.Load("Graphics/Effects/Ground/IceMat") as Material;
            grassMat = Resources.Load("Graphics/Effects/Ground/GrassMat") as Material;
            hearthStoneMat = Resources.Load("Graphics/Effects/Ground/HearthStoneMat") as Material;

            plopFluid = Resources.Load("Graphics/Effects/Ground/PlopFluid") as GameObject;
            bubbleFluid = Resources.Load("Graphics/Effects/Ground/BubbleFluid") as GameObject;

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

    #region[빛나는깃털]
    public void LightFeather(Vector2 vector2, bool flipX,Vector2 force, int type = 0)
    {
        string name = "LightFeather" + type;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(lightFeather[type]);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.GetComponent<Rigidbody2D>().AddForce(force);
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }

    public void LightFeather(Vector3 vector3, Vector2 force, int type = 0)
    {
        LightFeather(vector3, (Random.Range(0, 100) > 50 ? false : true), force, type);
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
        DigGround(vector2, StageData.GroundLayer.Dirt);
    }

    public void DigGround(Vector2 vector2, StageData.GroundLayer groundLayer)
    {
        string name = "DigGround";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(digGround);
            emp.transform.name = name;
            AddObject(emp);
        }

        Material temp = dirtMat;
        switch (groundLayer)
        {
            case StageData.GroundLayer.Dirt:
                temp = dirtMat;
                if (SceneController.instance.nowScene.Contains("Stage01"))
                    temp = dirtMat;
                else if (SceneController.instance.nowScene.Contains("Stage02"))
                    temp = snowMat;
                break;
            case StageData.GroundLayer.Stone:
                temp = stoneMat;
                break;
            case StageData.GroundLayer.Copper:
                temp = copperMat;
                break;
            case StageData.GroundLayer.Sand:
                temp = sandMat;
                break;
            case StageData.GroundLayer.Granite:
                temp = graniteMat;
                break;
            case StageData.GroundLayer.Iron:
                temp = ironMat;
                break;
            case StageData.GroundLayer.Silver:
                temp = silverMat;
                break;
            case StageData.GroundLayer.Gold:
                temp = goldMat;
                break;
            case StageData.GroundLayer.Mithril:
                temp = mithrillMat;
                break;
            case StageData.GroundLayer.Diamond:
                temp = diamondMat;
                break;
            case StageData.GroundLayer.Magnetite:
                temp = magnetiteMat;
                break;
            case StageData.GroundLayer.Titanium:
                temp = titaniumMat;
                break;
            case StageData.GroundLayer.Cobalt:
                temp = cobaltMat;
                break;
            case StageData.GroundLayer.Ice:
                temp = iceMat;
                break;
            case StageData.GroundLayer.Grass:
                temp = grassMat;
                break;
            case StageData.GroundLayer.HearthStone:
                temp = hearthStoneMat;
                break;
        }

        emp.GetComponent<ParticleSystem>().GetComponent<Renderer>().material = temp;

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
    }
    #endregion

    #region[풍덩]
    public void PlopFluid(Vector2 vector2, Color color,int num = 50)
    {
        PlopFluid(vector2, Vector2.zero, color);
    }

    public void PlopFluid(Vector2 vector2, int num = 50)
    {
        PlopFluid(vector2, Vector2.zero, Color.white);
    }

    public void PlopFluid(Vector2 vector2, Vector2 force, int num = 50)
    {
        PlopFluid(vector2, force, Color.white);
    }

    public void PlopFluid(Vector2 vector2,Vector2 force, Color color, int num = 50)
    {
        string name = "PlopFluid";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(plopFluid);
            emp.transform.name = name;
            AddObject(emp);
        }
        ParticleSystem particle = emp.GetComponent<ParticleSystem>();
        ParticleSystem.VelocityOverLifetimeModule velocity = particle.velocityOverLifetime;
        ParticleSystem.EmissionModule emission = particle.emission;
        velocity.x = force.x;
        velocity.y = force.y;
        emission.rateOverTime = num;
        var main = particle.main;
        main.startColor = color;
        
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
    }
    #endregion

    #region[기포]
    public void BubbleFluid(Vector2 vector2, float speed)
    {
        BubbleFluid(vector2, new Vector2(1,1), speed);
    }

    public void BubbleFluid(Vector2 vector2, Vector2 size, float speed)
    {
        string name = "BubbleFluid";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(bubbleFluid);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.GetComponent<BubbleScript>().speed = speed;
        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = size;
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
        emp.GetComponent<Follow>().followUI = UIManager.instance.itemObject[0].transform.Find("center").gameObject;
        if(size != Vector2.zero)
            emp.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = size;
    }
    #endregion

    #region[현재 아이템 표시]
    public void NowItem(Sprite sprite)
    {
        UIManager.instance.nowItemImage.sprite = sprite;
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

    #region[기포생성 이펙트]
    public void CreateBubbleEffect(Vector2 pos, Vector4 Range, int num,  float speed)
    {
        StartCoroutine(BubbleEffectAct(pos, Range, num, speed));
    }

    private IEnumerator BubbleEffectAct(Vector2 pos, Vector4 Range, int num, float speed)
    {
        for (int i = 0; i < num; i++)
        {
            float size = Random.Range(0.01f, 0.1f);
            BubbleFluid(pos + new Vector2(Random.Range(Range.x, Range.y), Random.Range(Range.z, Range.w)), new Vector2(size, size), speed);
            yield return new WaitForSeconds(0.01f);
        }
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