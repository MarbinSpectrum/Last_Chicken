using System.Collections.Generic;
using UnityEngine;
using Custom;

public class ObjectManager : ObjectPool
{
    public static ObjectManager instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static string[] objectName = new string[]
    {
        "Ladder",
        "MineBox",
        "MineCart",
        "Stone0",
        "Stone1",
        "Wagon",
        "WoodBoard0",
        "WoodBoard1",
        "Shovel",
        "WoodBox",
        "TreasureBox",
        "Stalagmite0",
        "Stalagmite1",
        "Stalagmite2",
        "Stalagmite3",
        "Stalagmite4",
        "Stalagmite5",
        "Sign"
    };

    [System.Serializable]
    public class ObjectStats
    {
        public int Hp = 0;
        public StructureObject.ObjectType ObjectType;
        public StructureObject.SpecialType SpecialType;
        public StructureObject.DamageSound DamageSound;
    }

    public static int FindData(string s)
    {
        for (int i = 0; i < objectName.Length; i++)
            if (objectName[i].Equals(s))
                return i;
        return 0;
    }

    public ObjectStats[] obejctData = new ObjectStats[objectName.Length];

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class TorchData
    {
        public GameObject gameObject;
        public Vector2Int pos;

        public TorchData(GameObject emp, Vector2Int vector)
        {
            gameObject = emp;
            pos = vector;
        }

    }
    List<TorchData> torchData = new List<TorchData>();


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject torch;
    GameObject boom;
    GameObject dynamite;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject mineWoodBoard;

    GameObject ladder;

    GameObject mineBox;

    GameObject mineCart;

    GameObject[] stone = new GameObject[2];

    GameObject wagon;

    GameObject[] woodBoard = new GameObject[2];

    GameObject shovel;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject altar;
    GameObject[] statue = new GameObject[6];
    GameObject[] altarBackgroundImgs = new GameObject[8];

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject fountain;
    GameObject fountainDecoration;
    GameObject[] chickenStatue = new GameObject[2];
    GameObject[] eggRock = new GameObject[2];
    GameObject[] rock = new GameObject[9];

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject[] stalagmite = new GameObject[6];

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject shop;
    GameObject[] shopRock = new GameObject[2];
    GameObject shopSign;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject woodBox;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject treasureBox;

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

            torch = Resources.Load("Objects/Item/Torch") as GameObject;
            boom = Resources.Load("Objects/Item/Boom/Boom") as GameObject;
            dynamite = Resources.Load("Objects/Item/Boom/Dynamite") as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            mineWoodBoard = Resources.Load("Objects/Structure/Stage01/MineRoadWoodBoard") as GameObject;
            ladder = Resources.Load("Objects/Structure/Stage01/Ladder") as GameObject;
            mineBox = Resources.Load("Objects/Structure/Stage01/MineBox") as GameObject;
            mineCart = Resources.Load("Objects/Structure/Stage01/MineCart") as GameObject;
            for (int i = 0; i < stone.Length; i++)
                stone[i] = Resources.Load("Objects/Structure/Stage01/Stone" + i) as GameObject;
            wagon = Resources.Load("Objects/Structure/Stage01/Wagon") as GameObject;
            for (int i = 0; i < woodBoard.Length; i++)
                woodBoard[i] = Resources.Load("Objects/Structure/Stage01/WoodBoard" + i) as GameObject;
            shovel = Resources.Load("Objects/Structure/Stage01/Shovel") as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            altar = Resources.Load("Objects/Structure/ChickenAltar/Altar") as GameObject;
            for (int i = 0; i < statue.Length; i++)
                statue[i] = Resources.Load("Objects/Structure/ChickenAltar/Statue" + i) as GameObject;
            for (int i = 0; i < altarBackgroundImgs.Length; i++)
                altarBackgroundImgs[i] = Resources.Load("Graphics/BackgroundImg/AltarImg" + i) as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            fountain = Resources.Load("Objects/Structure/ChickenFountain/Fountain") as GameObject;
            fountainDecoration = Resources.Load("Objects/Structure/ChickenFountain/FountainDecoration") as GameObject;
            for (int i = 0; i < chickenStatue.Length; i++)
                chickenStatue[i] = Resources.Load("Objects/Structure/ChickenFountain/ChickenStatue" + i) as GameObject;
            for (int i = 0; i < eggRock.Length; i++)
                eggRock[i] = Resources.Load("Objects/Structure/ChickenFountain/EggRock" + i) as GameObject;
            for (int i = 0; i < rock.Length; i++)
                rock[i] = Resources.Load("Objects/Structure/ChickenFountain/Rock" + i) as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            for (int i = 0; i < stalagmite.Length; i++)
                stalagmite[i] = Resources.Load("Objects/Trap/Stalagmite" + i) as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            shop = Resources.Load("Objects/Structure/Shop/Shop") as GameObject;
            for (int i = 0; i < shopRock.Length; i++)
                shopRock[i] = Resources.Load("Objects/Structure/Shop/ShopRock" + i) as GameObject;
            shopSign = Resources.Load("Objects/Structure/Shop/ShopSign") as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            woodBox = Resources.Load("Objects/Structure/WoodBox") as GameObject;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            treasureBox = Resources.Load("Objects/Structure/TreasureBox") as GameObject;
        }
    }
    #endregion

    #region[Update]
    private void Update()
    {
        //if (AltarScript.instance && AltarScript.instance.onPlayer)
        //{
        //    for (int i = 0; i < torchData.Count; i++)
        //        torchData[i].gameObject.SetActive(false);
        //}
        //else
        //{
        //    for (int i = 0; i < torchData.Count; i++)
        //    {
        //        if (!torchData[i].gameObject.activeSelf)
        //            torchData[i].gameObject.SetActive(true);
        //        Vector2Int pos = new Vector2Int(Mathf.FloorToInt(torchData[i].gameObject.transform.position.x), Mathf.FloorToInt(torchData[i].gameObject.transform.position.y));
        //        if (torchData[i].pos != pos)
        //        {
        //            torchData[i].gameObject.SetActive(false);
        //            torchData[i].pos = pos;
        //            torchData[i].gameObject.SetActive(true);
        //        }
        //    }
        //}
        //ObjectAct();

    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[횃불]
    public void Torch(Vector2 vector2, bool flipX = false)
    {
        string name = "Torch";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(torch);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
        GameObject torchLight = emp.transform.Find("BlockLight").gameObject;
        TorchData data = new TorchData(torchLight, new Vector2Int(Mathf.FloorToInt(vector2.x), Mathf.FloorToInt(vector2.y)));

        torchData.Add(data);
    }
    #endregion

    #region[폭탄]

    public void Boom(Vector2 vector2, bool flipX = false)
    {
        Boom(vector2, Vector2.zero, flipX);
    }

    public void Boom(Vector2 vector2, Vector2 Force, bool flipX = false)
    {
        string name = "Boom";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(boom);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);

        emp.GetComponent<Rigidbody2D>().AddForce(Force);
        if(Force != Vector2.zero)
            emp.GetComponent<Rigidbody2D>().angularVelocity = Random.value * 360;
    }
    #endregion

    #region[폭탄]
    public void Dynamite(Vector2 vector2 ,bool flipX = false)
    {
        string name = "Dynamite";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(dynamite);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);
    }
    #endregion


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[광산길나무판자]
    public void MineWoodBoard(Vector2 vector2, bool flipX = false)
    {
        string name = "MineWoodBoard";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(mineWoodBoard);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1);;
    }
    #endregion

    #region[사다리]
    public void Ladder(Vector2 vector2, bool flipX = false)
    {
        string name = objectName[0];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(ladder);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    #region[광물상자]
    public void MineBox(Vector2 vector2, bool flipX = false)
    {
        string name = objectName[1];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(mineBox);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    #region[광산카트]
    public void MineCart(Vector2 vector2, bool flipX = false)
    {
        string name = objectName[2];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(mineCart);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    #region[돌무더기]
    public void Stone(Vector2 vector2,int n, bool flipX = false)
    {
        if (!Exception.IndexOutRange(n, stone))
            return;

        string name = objectName[3 + n];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(stone[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }

    public void Stone(Vector2 vector2, bool flipX = false)
    {
        Stone(vector2, Random.Range(0, stone.Length), flipX);
    }
    #endregion

    #region[광산수레]
    public void Wagon(Vector2 vector2, bool flipX = false)
    {
        string name = objectName[5];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(wagon);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    #region[나무판자]
    public void WoodBoard(Vector2 vector2, int n, bool flipX = false)
    {
        if (!Exception.IndexOutRange(n, woodBoard))
            return;

        string name = objectName[6 + n];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(woodBoard[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }

    public void WoodBoard(Vector2 vector2, bool flipX = false)
    {
        WoodBoard(vector2, Random.Range(0, woodBoard.Length), flipX);
    }
    #endregion

    #region[삽]
    public void Shovel(Vector2 vector2, bool flipX = false)
    {
        string name = objectName[8];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(shovel);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[제단]
    public void Altar(Vector2 vector2, bool flipX = false)
    {
        string name = "Altar";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(altar);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    #region[석상]
    public void Statue(Vector2 vector2, int n, bool flipX = false)
    {
        if (!Exception.IndexOutRange(n, statue))
            return;

        string name = "Statue" + n;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(statue[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }

    public void Statue(Vector2 vector2, bool flipX = false)
    {
        Statue(vector2, Random.Range(0, statue.Length), flipX);
    }
    #endregion

    #region[제단배경이미지]
    public void AltarBackgroundImgs(Vector2 vector2, int n)
    {
        if (!Exception.IndexOutRange(n, altarBackgroundImgs))
            return;

        string name = "AltarBackgroundImgs" + n;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(altarBackgroundImgs[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
    }

    public void AltarBackgroundImgs(Vector2 vector2)
    {
        Statue(vector2, Random.Range(0, altarBackgroundImgs.Length));
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[분수]
    public void Fountain(Vector2 vector2, bool flipX = false)
    {
        string name = "Fountain";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(fountain);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    #region[분수장식]
    public void FountainDecoration(Vector2 vector2, bool flipX = false)
    {
        string name = "FountainDecoration";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(fountainDecoration);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    #region[닭석상]
    public void ChickenStatue(Vector2 vector2, int n, bool flipX = false)
    {
        if (!Exception.IndexOutRange(n, chickenStatue))
            return;

        string name = "ChickenStatue" + n;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(chickenStatue[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }

    public void ChickenStatue(Vector2 vector2, bool flipX = false)
    {
        Statue(vector2, Random.Range(0, chickenStatue.Length), flipX);
    }
    #endregion

    #region[달걀바위]
    public void EggRock(Vector2 vector2, int n, bool flipX = false)
    {
        if (!Exception.IndexOutRange(n, eggRock))
            return;

        string name = "EggRock" + n;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(eggRock[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }

    public void EggRock(Vector2 vector2, bool flipX = false)
    {
        Statue(vector2, Random.Range(0, eggRock.Length), flipX);
    }
    #endregion

    #region[바위]
    public void Rock(Vector2 vector2, int n, bool flipX = false)
    {
        if (!Exception.IndexOutRange(n, rock))
            return;

        string name = "Rock" + n;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(rock[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }

    public void Rock(Vector2 vector2, bool flipX = false)
    {
        Rock(vector2, Random.Range(0, rock.Length), flipX);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[석순]
    public void Stalagmite(Vector2 vector2, int n, bool flipX = false)
    {
        if (!Exception.IndexOutRange(n, stalagmite))
            return;

        string name = "Stalagmite" + n;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(stalagmite[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }

    public void Stalagmite(Vector2 vector2, bool flipX = false)
    {
        Stalagmite(vector2, Random.Range(0, stalagmite.Length), flipX);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[상점]
    public void Shop(Vector2 vector2, bool flipX = false)
    {
        string name = "Shop";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(shop);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    #region[상점바위]
    public void ShopRock(Vector2 vector2, int n, bool flipX = false)
    {
        if (!Exception.IndexOutRange(n, statue))
            return;

        string name = "ShopRock" + n;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(shopRock[n]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }

    public void ShopRock(Vector2 vector2, bool flipX = false)
    {
        ShopRock(vector2, Random.Range(0, shopRock.Length), flipX);
    }
    #endregion

    #region[상점표시]
    public void ShopSign(Vector2 vector2, bool flipX = false)
    {
        string name = "ShopSign";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(shopSign);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[나무상자]
    public void WoodBox(Vector2 vector2, bool flipX = false)
    {
        WoodBox(vector2, "Random", flipX);
    }
    public void WoodBox(Vector2 vector2, string item,bool flipX = false)
    {
        string name = objectName[9];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(woodBox);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.GetComponent<StructureObject>().inItem = item;
        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[보물상자]
    public void TreasureBox(Vector2 vector2, bool flipX = false)
    {
        TreasureBox(vector2, "Random", flipX);
    }
    public void TreasureBox(Vector2 vector2,string item, bool flipX = false)
    {
        string name = objectName[10];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(treasureBox);
            emp.transform.name = name;
            AddObject(emp);
        }
        emp.GetComponent<StructureObject>().inItem = item;
        emp.SetActive(true);
        emp.transform.localRotation = Quaternion.identity;
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.transform.localScale = new Vector3(flipX ? -1 : +1, 1, 1); ;
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[오브젝트풀링 데이터를 모두 비활성화]
    public override void PoolOff()
    {
        base.PoolOff();
        for (int i = 0; i < torchData.Count; i++)
            torchData[i].gameObject.SetActive(false);
        torchData.Clear();
    }
    #endregion

    #region[멀리 있는 오브젝트 비활성화]
    public override void ObjectAct()
    {
        if (!GameManager.instance.InGame())
            return;

        for (int i = 0; i < objectPool.Count; i++)
            if (objectPool[i] && objectPool[i].GetComponent<StructureObject>())
            {
                Vector3 targetScreenPos = Camera.main.WorldToViewportPoint(objectPool[i].transform.position);

                Vector2 size = new Vector2(1.5f, 1.5f);

                objectPool[i].GetComponent<StructureObject>().enabled =
                    !(
                    targetScreenPos.x > (1 + size.x) / 2f ||
                    targetScreenPos.x < (1 - size.x) / 2f ||
                    targetScreenPos.y > (1 + size.y) / 2f ||
                    targetScreenPos.y < (1 - size.x) / 2f
                    );
            }
    }
    #endregion
}