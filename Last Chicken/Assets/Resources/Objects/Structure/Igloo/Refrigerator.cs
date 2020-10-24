using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    public static List<Refrigerator> refrigeratorList = new List<Refrigerator>();

    public float coolTime = 0;

    #region[Update]
    private void Update()
    {
        coolTime -= Time.deltaTime;
        if(coolTime <= 0 && MonsterManager.instance.monsterNum < 100)
        {
            coolTime = 2;
            MonsterManager.instance.Penguin(new Vector3(transform.position.x, transform.position.y, -2));
        }
    }
    #endregion

    #region[OnEnable]
    private void OnEnable()
    {
        refrigeratorList.Add(this);
    }
    #endregion

    #region[OnDisable]
    private void OnDisable()
    {
        refrigeratorList.Remove(this);
    }
    #endregion

    
}
