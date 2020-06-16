﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;

public class ItemManager : ObjectPool
{
    public static ItemManager instance;

    public static string[] itemName = new string[] 
    {
        "Bell",
        "Feather_Shoes",
        "Light_Pick",
        "Medkit",
        "Torch",
        "FourLeafClover" ,
        "Coke",
        "Beer",
        "Monster_Radar",
        "Russian_Roulette",
        "Mine_Helmet",
        "Heavy_Pick" ,
        "Advanced_Pick",
        "BoomItem",
        "Dynamite",
        "Smart_Light_Pick",
        "Smart_Heavy_Pick",
        "Smart_Advanced_Pick",
        "MineBag",
        "Smart_MineBag",
        "SaleCoupon",
        "ShopVIpNormal",
        "ShopVIpSpecial",
        "OldPocket",
        "RandomDice",
        "TreasureBox_Radar",
        "Trap_Radar",
        "RainbowPocket",
        "Garbage",
        "Hammer",
        "Crampons"
    };

    #region[액티브 아이템 여부검사]
    public static bool CheckActiveItem(string itemName)
    {

        bool activeItem = false;
        switch (itemName)
        {
            case "Bell":
                activeItem = true;
                break;
            case "Coke":
                activeItem = true;
                break;
            case "Beer":
                activeItem = true;
                break;
            case "Russian_Roulette":
                activeItem = true;
                break;
            case "BoomItem":
                activeItem = true;
                break;
            case "Dynamite":
                activeItem = true;
                break;
            case "ShopVIpSpecial":
                activeItem = true;
                break;
            case "OldPocket":
                activeItem = true;
                break;
            case "RainbowPocket":
                activeItem = true;
                break;
            case "RandomDice":
                activeItem = true;
                break;
        }
        return activeItem;
    }
    #endregion

    #region[패시브 아이템 여부검사]
    public static bool CheckPassiveItem(string itemName)
    {
        return !CheckActiveItem(itemName);
    }
    #endregion

    #region[쿨타임 아이템 여부검사]
    public static bool CheckCoolTimeItem(string itemName)
    {

        bool activeItem = false;
        switch (itemName)
        {
            case "Bell":
                activeItem = true;
                break;
            case "BoomItem":
                activeItem = true;
                break;
            case "Dynamite":
                activeItem = true;
                break;
        }
        return activeItem;
    }
    #endregion

    #region[재등장 아이템 여부검사]
    public static bool CheckReSpawnItem(string itemName)
    {
        switch (itemName)
        {
            case "Coke":
            case "Beer":
            case "OldPocket":
            case "RainbowPocket":
                return true;
        }
        return false;
    }
    #endregion

    #region[획득발동 아이템 여부검사]
    public static bool CheckGetActiveItem(string itemName)
    {
        switch (itemName)
        {
            case "Russian_Roulette":
            case "OldPocket":
            case "RainbowPocket":
            case "RandomDice":
            case "ShopVIpSpecial":
            case "Coke":
            case "Beer":
                return true;
        }
        return false;
    }
    #endregion

    #region[갯수 아이템 여부검사]
    public bool AddItemCheck(string name)
    {
        //if (name.Equals("BoomItem") && HasItemCheck(name))
        //    return true;
        //if (name.Equals("Dynamite") && HasItemCheck(name))
        //    return true;
        return false;
    }
    #endregion

    public int normalRate;
    public int rareRate;
    public int specialRate;
    public int legendRate;

    public enum ItemLevel
    {
        일반,
        희귀,
        특급,
        전설
    }

    [System.Serializable]
    public class ItemStats
    {
        public Sprite itemImg;
        public string itemName;
        public string itemName_Eng;
        public float value0;
        public float value1;
        public ItemLevel itemLevel;
        public string itemExplain;
        public string itemExplain_Eng;
        public int cost;
        public bool spawnObject;
        public bool spawnShop;
        public bool spawnTreasureBox;
        public bool activeItem;
        public string[] shopItemExplain = new string[4];
        public string[] shopItemExplain_Eng = new string[4];
    }

    public static int FindData(string s)
    {
        for (int i = 0; i < itemName.Length; i++)
            if (itemName[i].Equals(s))
                return i;
        return -1;
    }

    public ItemStats[] itemData = new ItemStats[itemName.Length];

    public class ItemInfo
    {
        public bool hasItem;
        public bool used;
    }

    public Dictionary<string, ItemInfo> nowItemList = new Dictionary<string, ItemInfo>();

    GameObject itemPrefab;
    GameObject mineralPrefab;

    public List<int> ReSpawnItemList = new List<int>();

    [HideInInspector] public List<GameObject> fieldObject = new List<GameObject>();
    [HideInInspector] public List<GameObject> caveObject = new List<GameObject>();
    [HideInInspector] public List<GameObject> mineralObject = new List<GameObject>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public void Awake()
    {
        if (instance == null)
        {
            for (int i = 0; i < itemName.Length; i++)
                nowItemList.Add(itemName[i], new ItemInfo());
            instance = this;

            itemPrefab = Resources.Load("Objects/Item/ItemPrefab") as GameObject;
            mineralPrefab = Resources.Load("Objects/Item/Mineral/MineralPrefab") as GameObject;
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

    #region[아이템 생성]
    public void SpawnItem(Vector2 vector2, string name,int num = -1)
    {
        SpawnItem(vector2, name, 10000, num);
    }

    public void SpawnItem(Vector2 vector2, string name, float cool, int num = -1)
    {
        if (FindData(name) == -1)
            name = "";

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(itemPrefab);
            emp.transform.name = name;
            AddObject(emp);
        }

        if (num == -1)
        {
            //if (name.Equals("BoomItem"))
            //    emp.GetComponent<ItemScript>().num = (int)itemData[FindData("BoomItem")].value1;
            //else if (name.Equals("Dynamite"))
            //    emp.GetComponent<ItemScript>().num = (int)itemData[FindData("Dynamite")].value1;
        }
        else
        {
            emp.GetComponent<ItemScript>().num = num;
        }

        emp.GetComponent<ItemScript>().cool = cool;

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        if (CaveManager.inCave)
            caveObject.Add(emp);
        else
            fieldObject.Add(emp);

    }
    #endregion

    #region[오브젝트에서 랜덤하게 아이템 생성]
    public int GetRandomItemAtWoodBox()
    {
        float randomValue = Random.Range(0, 100);

        List<int> itemList = new List<int>();

        if (randomValue < normalRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.일반 && itemData[i].spawnObject &&
                    !nowItemList[itemName[i]].used && 
                    !HasItemCheck(itemName[i]) &&
                    (!CheckReSpawnItem(itemName[i]) ? !ReSpawnItemList.Contains(i) : true))
                    itemList.Add(i);
        }
        else if (randomValue < normalRate + rareRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.희귀 && itemData[i].spawnObject &&
                    !nowItemList[itemName[i]].used && 
                    !HasItemCheck(itemName[i]) &&
                    (!CheckReSpawnItem(itemName[i]) ? !ReSpawnItemList.Contains(i) : true))
                    itemList.Add(i);
        }
        else if (randomValue < normalRate + rareRate + specialRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.특급 && itemData[i].spawnObject &&
                    !nowItemList[itemName[i]].used &&
                    !HasItemCheck(itemName[i]) &&
                    (!CheckReSpawnItem(itemName[i]) ? !ReSpawnItemList.Contains(i) : true))
                    itemList.Add(i);
        }
        else if (randomValue < normalRate + rareRate + specialRate + legendRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.전설 && itemData[i].spawnObject &&
                    !nowItemList[itemName[i]].used && 
                    !HasItemCheck(itemName[i]) &&
                    (!CheckReSpawnItem(itemName[i]) ? !ReSpawnItemList.Contains(i) : true))
                    itemList.Add(i);
        }

        if (itemList.Count > 0)
            return itemList[Random.Range(0, itemList.Count)];

        return -1;
    }

    public void SpawnItemRandomAtObject(Vector2 vector2)
    {
        int itemNum = GetRandomItemAtWoodBox();

        if (Exception.IndexOutRange(itemNum, itemName))
        {
            if (CheckReSpawnItem(itemName[itemNum]))
            {
                int countItem = 0;
                for (int i = 0; i < ReSpawnItemList.Count; i++)
                    if (ReSpawnItemList[i] == itemNum)
                        countItem++;
                ReSpawnItemList.Add(itemNum);
                if (Random.Range(0, 100) >= 100 * Mathf.Pow(0.5f, countItem))
                    itemNum = -1;
            }
            ReSpawnItemList.Add(itemNum);
        }

        if (itemNum != -1)
            SpawnItem(vector2, itemName[itemNum]);
    }
    #endregion

    #region[보물상자에서 랜덤하게 아이템 생성]
    public int GetRandomItemAtTreasureBox()
    {
        float randomValue = Random.Range(0, specialRate + legendRate);

        List<int> itemList = new List<int>();

        if (randomValue < specialRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.특급 && itemData[i].spawnTreasureBox &&
                    !nowItemList[itemName[i]].used && !HasItemCheck(itemName[i]))
                    itemList.Add(i);
        }
        else
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.전설 && itemData[i].spawnTreasureBox &&
                    !nowItemList[itemName[i]].used && !HasItemCheck(itemName[i]))
                    itemList.Add(i);
        }

        if (itemList.Count > 0)
            return itemList[Random.Range(0, itemList.Count)];

        return -1;
    }

    public void SpawnItemRandomAtTreasureBox(Vector2 vector2)
    {
        int itemNum = GetRandomItemAtTreasureBox();
        if (itemNum != -1)
            SpawnItem(vector2, itemName[itemNum]);
    }
    #endregion

    #region[상점에서 랜덤하게 아이템 생성]
    public int GetRandomItemAtShop()
    {
        float randomValue = Random.Range(0, normalRate + rareRate + specialRate + legendRate);

        List<int> itemList = new List<int>();

        if (randomValue < normalRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.일반 && itemData[i].spawnShop &&
                    !nowItemList[itemName[i]].used && !HasItemCheck(itemName[i]))
                    itemList.Add(i);
        }
        else if (randomValue < normalRate + rareRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.희귀 && itemData[i].spawnShop &&
                    !nowItemList[itemName[i]].used && !HasItemCheck(itemName[i]))
                    itemList.Add(i);
        }
        else if (randomValue < normalRate + rareRate + specialRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.특급 && itemData[i].spawnShop &&
                    !nowItemList[itemName[i]].used && !HasItemCheck(itemName[i]))
                    itemList.Add(i);
        }
        else if (randomValue < normalRate + rareRate + specialRate + legendRate)
        {
            for (int i = 0; i < itemName.Length; i++)
                if (itemData[i].itemLevel == ItemLevel.전설 && itemData[i].spawnShop &&
                    !nowItemList[itemName[i]].used && !HasItemCheck(itemName[i]))
                    itemList.Add(i);
        }

        if (itemList.Count > 0)
            return itemList[Random.Range(0, itemList.Count)];

        return 6;
    }
    #endregion

    #region[멀리 있는 오브젝트 비활성화]
    public override void ObjectAct()
    {
        for (int i = 0; i < objectPool.Count; i++)
            if (objectPool[i] && objectPool[i].GetComponent<ItemScript>())
            {
                Vector3 targetScreenPos = Camera.main.WorldToViewportPoint(objectPool[i].transform.position);

                Vector2 size = new Vector2(1.5f, 1.5f);

                objectPool[i].GetComponent<ItemScript>().enabled =
                    !(
                    targetScreenPos.x > (1 + size.x) / 2f ||
                    targetScreenPos.x < (1 - size.x) / 2f ||
                    targetScreenPos.y > (1 + size.y) / 2f ||
                    targetScreenPos.y < (1 - size.x) / 2f
                    );
            }
    }
    #endregion

    public void FieldObjectAct(bool act)
    {
        for (int i = 0; i < fieldObject.Count; i++)
            fieldObject[i].SetActive(act);
    }

    public void CaveObjectAct(bool act)
    {
        for (int i = 0; i < caveObject.Count; i++)
            caveObject[i].SetActive(act);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[광물 생성]
    public void SpawnMineral(Vector2 vector2, string name)
    {
        SpawnMineral(vector2, Vector2.zero, name);
    }

    public void SpawnMineral(Vector2 vector2, Vector2 Force, string name)
    {
        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(mineralPrefab);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector2.x, vector2.y, emp.transform.position.z);
        emp.GetComponent<Rigidbody2D>().AddForce(Force);
        if (Force != Vector2.zero)
            emp.GetComponent<Rigidbody2D>().angularVelocity = Random.value * 360;
        mineralObject.Add(emp);
    }
    #endregion

    public void MineralObjectAct(bool act)
    {
        for (int i = 0; i < mineralObject.Count; i++)
            if (mineralObject[i].transform.position.x != -1000)
                mineralObject[i].SetActive(act);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[액티브 아이템 사용가능 검사]
    public bool CanUseActiveItem(string name)
    {
        if (GameManager.instance.itemSlot[0].Equals(name))
        {
            switch(name)
            {
                case "Bell":
                    return GameManager.instance.itemCool[0] >= itemData[FindData("Bell")].value0;
                case "BoomItem":
                    return GameManager.instance.itemCool[0] >= itemData[FindData("BoomItem")].value1;
                case "Dynamite":
                    return GameManager.instance.itemCool[0] >= itemData[FindData("Dynamite")].value1;
            }
            return true;
        }
        return false;
    }
    #endregion

    #region[패시브 아이템 사용가능 검사]
    public bool CanUsePassiveItem(string name)
    {
        return HasItemCheck(name);
    }
    #endregion

    #region[아이템 보유여부검사]
    public bool HasItemCheck(string name)
    {
        for(int i = 0; i < 6; i++)
            if (GameManager.instance.itemSlot[i].Equals(name))
                return true;
        return false;
    }
    #endregion

    #region[아이템을 소비]
    public void CostItem(string name,int n = 1)
    {
        if (!HasItemCheck(name))
            return;

        for (int i = 0; i < 6; i++)
            if (GameManager.instance.itemSlot[i].Equals(name))
            {
                GameManager.instance.itemNum[i] -= n;
                if (GameManager.instance.itemNum[i] <= 0)
                {
                    GameManager.instance.itemSlot[i] = "";
                    GameManager.instance.itemNum[i] = 0;
                    UIManager.instance.MoveItem();
                }
                return;
            }
    }
    #endregion

    #region[아이템을 사용]
    public void UseItem(string name)
    {
        if (!HasItemCheck(name))
            return;

        for (int i = 0; i < 6; i++)
            if (GameManager.instance.itemSlot[i].Equals(name))
            {
                switch (name)
                {
                    case "Bell":
                        GameManager.instance.itemCool[0] = 0;
                        break;
                    case "BoomItem":
                        GameManager.instance.itemCool[0] = 0;
                        break;
                    case "Dynamite":
                        GameManager.instance.itemCool[0] = 0;
                        break;
                }
                break;
            }
    }
    #endregion

    #region[아이템을 추가]
    public void AddItem(string name,int n)
    {
        for (int i = 0; i < 6; i++)
            if (GameManager.instance.itemSlot[i].Equals(name))
            {
                GameManager.instance.itemNum[i] += n;
                return;
            }
    }
    #endregion

}
