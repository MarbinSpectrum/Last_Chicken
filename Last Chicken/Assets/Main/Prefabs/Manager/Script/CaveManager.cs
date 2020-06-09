using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManager : ObjectPool
{
    public static CaveManager instance;

    public GameObject[] shopCave;
    public GameObject[] smithyCave;
    public GameObject[] altarCave;
    public GameObject[] fountainCave;
    public static bool inCave;

    #region[Awake]
    public void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    #region[상점동굴]
    public void ShopCave(Vector3 vector3,int dic)
    {
        string name = "ShopCave" + dic;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(shopCave[dic]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector3.x, vector3.y, emp.transform.position.z);
        emp.transform.rotation = new Quaternion(0, 0, 0, 0);
        emp.GetComponent<ObjectCave>().ObjectInit();
    }
    #endregion

    #region[대장간동굴]
    public void SmithyCave(Vector3 vector3,int dic)
    {
        string name = "SmithyCave" + dic;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(smithyCave[dic]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector3.x, vector3.y, emp.transform.position.z);
        emp.transform.rotation = new Quaternion(0, 0, 0, 0);
        emp.GetComponent<ObjectCave>().ObjectInit();
    }
    #endregion

    #region[제단동굴]
    public void AltarCave(Vector3 vector3,int dic)
    {
        string name = "AltarCave" + dic;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(altarCave[dic]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector3.x, vector3.y, emp.transform.position.z);
        emp.transform.rotation = new Quaternion(0, 0, 0, 0);
        emp.GetComponent<ObjectCave>().ObjectInit();
    }
    #endregion

    #region[분수동굴]
    public void FountainCave(Vector3 vector3,int dic)
    {
        string name = "FountainCave" + dic;

        GameObject emp = FindObject(name);

        if (emp == null)
        {
            emp = Instantiate(fountainCave[dic]);
            emp.transform.name = name;
            AddObject(emp);
        }

        emp.SetActive(true);
        emp.transform.parent = transform;
        emp.transform.position = new Vector3(vector3.x, vector3.y, emp.transform.position.z);
        emp.transform.rotation = new Quaternion(0, 0, 0, 0);
        emp.GetComponent<ObjectCave>().ObjectInit();
    }
    #endregion

    public void CaveEnter(GameObject temp)
    {
        for (int i = 0; i < objectPool.Count; i++)
            if (objectPool[i].transform.position.x != -1000)
                objectPool[i].SetActive(false);
        temp.SetActive(true);
    }

    public void CaveExit()
    {
        for (int i = 0; i < objectPool.Count; i++)
            if (objectPool[i].transform.position.x != -1000)
                objectPool[i].SetActive(true);
    }


    #region[오브젝트풀링 데이터를 모두 비활성화]
    public override void PoolOff()
    {
        for (int i = 0; i < objectPool.Count; i++)
            if (objectPool[i])
            {
                objectPool[i].SetActive(false);
                objectPool[i].transform.position = new Vector3(-1000, -1000, objectPool[i].transform.position.z);
                objectPool[i].GetComponent<ObjectCave>().ObjectInit();
            }
    }
    #endregion
}
