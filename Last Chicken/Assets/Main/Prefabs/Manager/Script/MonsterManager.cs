using System.Collections.Generic;
using TerrainEngine2D;
using UnityEngine;
using Custom;

public class MonsterManager : ObjectPool
{
    public static MonsterManager instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.Serializable]
    public class SpawnMonster
    {
        public bool[] monsters;
        public int[] monsterValue;
        public int monsterDistance;
        public int monsterNum;
        public SpawnMonster()
        {
            monsters = new bool[monsterName.Length];
            monsterValue = new int[monsterName.Length];
        }
    }

    List<int> monsterList;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static string[] monsterName = new string[] { "Bat", "Rat", "Snake", "Mole"};

    [System.Serializable]
    public class MonsterStats
    {
        public int Hp = 0;
        public float Speed = 0;
        public float AttackPower = 0;
        public Vector2 JumpPower = Vector2.zero;
    }
    
    public static int FindData(string s)
    {
        for (int i = 0; i < monsterName.Length; i++)
            if (monsterName[i].Equals(s))
                return i;
        return 0;
    }

    public MonsterStats[] monsterData = new MonsterStats[monsterName.Length];

    GameObject bat;

    GameObject rat;

    GameObject snake;

    GameObject mole;

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

            bat = Resources.Load("Objects/Monster/Bat") as GameObject;

            rat = Resources.Load("Objects/Monster/Rat") as GameObject;

            snake = Resources.Load("Objects/Monster/Snake") as GameObject;

            mole = Resources.Load("Objects/Monster/Mole") as GameObject;

            for (int i = 0; i < 100; i++)
            {
                Bat(new Vector2(-1000,-1000));
                Rat(new Vector2(-1000, -1000));
                Snake(new Vector2(-1000, -1000));
                Mole(new Vector2(-1000, -1000));
            }

            PoolOff();
        }
    }
    #endregion

    #region[Update]
    private void Update()
    {
        ObjectAct();
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[초기설정]
    public void Init(World world,int num,int dis = 20)
    {
        PoolOff();
        SetMonsterList(SceneController.instance.nowScene);

        for (int i = 0; i < num; i++)
            ReSwpawn(world, dis);
    }
    #endregion

    #region[몬스터 생성]
    public void ReSwpawn(World world ,int dis = 20)
    {
        List<Vector2Int> monsterPos = MonsterSpawnPos(world, dis);
        if (monsterPos.Count == 0)
            return;
        Vector2Int pos = monsterPos[Random.Range(0, monsterPos.Count)];
        if (monsterList.Count > 0)
        {
            int r = monsterList[Random.Range(0, monsterList.Count)];
            Vector2 offset = new Vector2(0, 1);
            switch (r)
            {
                case 0:
                    Bat(new Vector3(pos.x + offset.x, pos.y + offset.y, -2));
                    break;
                case 1:
                    Rat(new Vector3(pos.x + offset.x, pos.y + offset.y, -2));
                    break;
                case 2:
                    Snake(new Vector3(pos.x + offset.x, pos.y + offset.y, -2));
                    break;
                case 3:
                    Mole(new Vector3(pos.x + offset.x, pos.y + offset.y, -2));
                    break;
            }
        }
    }
    #endregion

    #region[몬스터 생성리스트 설정]
    public void SetMonsterList(string s)
    {
        if (monsterList != null)
            monsterList.Clear();
        switch (s)
        {
            case "Stage0101":
                monsterList = MonsterList(StageManager.instance.stage0101_Monsters);
                break;
            case "Stage0102":
                monsterList = MonsterList(StageManager.instance.stage0102_Monsters);
                break;
            case "Stage0103":
                monsterList = MonsterList(StageManager.instance.stage0103_Monsters);
                break;
            case "Stage0201":
                monsterList = MonsterList(StageManager.instance.stage0201_Monsters);
                break;
            default:
                Debug.Log("몬스터 리스트를 설정해주세요!!");
                monsterList = MonsterList(StageManager.instance.stage0101_Monsters);
                break;
        }
    }
    #endregion

    #region[생성되는 몬스터 랜덤값 설정]
    List<int> MonsterList(SpawnMonster list)
    {
        //드랍몬스터들
        List<int> tempList = new List<int>();
        for (int i = 0; i < list.monsters.Length; i++)
            if (list.monsters[i])
                tempList.Add(i);

        //합비율
        float sumValue = 0;
        for (int i = 0; i < tempList.Count; i++)
                sumValue += list.monsterValue[tempList[i]];
        sumValue = 100f / sumValue;

        //드랍률에따른 리스트생성
        List<int> emp = new List<int>();
        int v = 100;
        for (int i = 0; i < tempList.Count; i++)
        {
            if(i != tempList.Count - 1)
            {
                int temp = (int)(list.monsterValue[tempList[i]] * sumValue);
                v -= temp;
                for(int j = 0; j < temp; j++)
                    emp.Add(tempList[i]);
            }
            else
            {
                for (int j = 0; j < v; j++)
                    emp.Add(tempList[i]);
            }
        }

        return emp;
    }
    #endregion

    #region[몬스터 생성 위치 리스트]
    List<Vector2Int> MonsterSpawnPos(World world,int dis)
    {
        List<Vector2Int> emp = new List<Vector2Int>();

        bool [,] donSetPos = new bool[world.WorldWidth, world.WorldHeight];

        for (int x = StageData.instance.altarRect.x; x < StageData.instance.altarRect.x + StageData.instance.altarRect.width; x++)
            for (int y = StageData.instance.altarRect.y - StageData.instance.altarRect.height; y < StageData.instance.altarRect.y; y++)
                if (Exception.IndexOutRange(x, y, donSetPos))
                    donSetPos[x, y] = true;

        for (int x = StageData.instance.fountainRect.x; x < StageData.instance.fountainRect.x + StageData.instance.fountainRect.width; x++)
            for (int y = StageData.instance.fountainRect.y - StageData.instance.fountainRect.height; y < StageData.instance.fountainRect.y; y++)
                if (Exception.IndexOutRange(x, y, donSetPos))
                    donSetPos[x, y] = true;

        Vector2Int pos;
        int r = dis;
        for (int i = 0; i < transform.childCount; i++)
        {
            pos = new Vector2Int((int)transform.GetChild(i).position.x, (int)transform.GetChild(i).position.y);
            for (int x = pos.x - r; x < pos.x + r; x++)
                for (int y = pos.y - r; y < pos.y + r; y++)
                    if (Exception.IndexOutRange(x, y, donSetPos))
                        donSetPos[x, y] = true;
        }
        if(Player.instance)
        {
            pos = new Vector2Int((int)Player.instance.transform.position.x, (int)Player.instance.transform.position.y);
            for (int x = pos.x - r; x < pos.x + r; x++)
                for (int y = pos.y - r; y < pos.y + r; y++)
                    if (Exception.IndexOutRange(x, y, donSetPos))
                        donSetPos[x, y] = true;
        }


        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 0; y < world.WorldHeight; y++)
                if (Exception.IndexOutRange(x, y - 1, donSetPos))
                    if (StageData.instance.GetBlock(x, y - 1) == (StageData.GroundLayer)(-1))
                        donSetPos[x, y] = true;

        for (int x = 0; x < world.WorldWidth; x++)
            for (int y = 20; y < world.WorldHeight - 20; y++)
                if (StageData.instance.GetBlock(x, y) == (StageData.GroundLayer)(-1) && !donSetPos[x,y])
                    emp.Add(new Vector2Int(x, y));

        //for (int i = 0; i < GroundManager.instance.linkAreaList.Count; i++)
        //    if (Vector2.Distance(Player.instance.transform.position, GroundManager.instance.linkAreaList[i]) < 30)
        //        if (Vector2.Distance(Player.instance.transform.position, GroundManager.instance.linkAreaList[i]) >= 20)
        //            emp.Add(GroundManager.instance.linkAreaList[i]);
        return emp;
    }
    #endregion

    #region[멀리 있는 오브젝트 비활성화]
    public override void ObjectAct()
    {
        if (!GameManager.instance.InGame())
            return;

        //for (int i = 0; i < objectPool.Count; i++)
        //    if (objectPool[i] && objectPool[i].GetComponent<Monster>())
        //    {
        //        Vector3 targetScreenPos = Camera.main.WorldToViewportPoint(objectPool[i].transform.position);

        //        Vector2 size = new Vector2(1.5f, 1.5f);

        //        objectPool[i].GetComponent<Monster>().enabled =
        //            !(
        //            targetScreenPos.x > (1 + size.x) / 2f ||
        //            targetScreenPos.x < (1 - size.x) / 2f ||
        //            targetScreenPos.y > (1 + size.y) / 2f ||
        //            targetScreenPos.y < (1 - size.x) / 2f
        //            );
        //    }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[박쥐]
    public void Bat(Vector3 vector3)
    {
        string name = monsterName[0];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(bat);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector3.x, vector3.y, emp.transform.position.z);
        emp.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    #endregion

    #region[쥐]
    public void Rat(Vector3 vector3)
    {
        string name = monsterName[1];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(rat);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector3.x, vector3.y, emp.transform.position.z);
        emp.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    #endregion

    #region[뱀]
    public void Snake(Vector3 vector3)
    {
        string name = monsterName[2];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(snake);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector3.x, vector3.y, emp.transform.position.z);
        emp.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    #endregion

    #region[두더지]
    public void Mole(Vector3 vector3)
    {
        string name = monsterName[3];

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(mole);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector3.x, vector3.y, emp.transform.position.z);
        emp.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}