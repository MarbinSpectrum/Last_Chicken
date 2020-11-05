using UnityEngine;
using System.Collections.Generic;

public class CustomCollider : MonoBehaviour
{
    [HideInInspector] public ConveyorBelt nowConveyorBelt;

    #region[각도에 따른 컨퍼넌트 offset]
    public Vector2 GetAngleOffset(BoxCollider2D col)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0, col.transform.eulerAngles.z);  // 회전각
        Vector2 angle = rotation * new Vector2(col.offset.x * transform.localScale.x, col.offset.y * transform.localScale.y);
        return angle;
    }
    #endregion

    #region[범위안에 닭이 있는지 검사]
    public bool IsAtChicken(BoxCollider2D col)
    {
        return IsAtChicken(col, Vector2.zero);
    }
    public bool IsAtChicken(BoxCollider2D col, Vector2 pos)
    {
        RaycastHit2D[] objects =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < objects.Length; i++)
            if (objects[i].transform.tag.Equals("Chicken"))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 플레이어가 있는지 검사]
    public bool IsAtPlayer(BoxCollider2D col)
    {
        return IsAtPlayer(col, Vector2.zero);
    }

    //RaycastHit2D[] tempObjects = new RaycastHit2D[10];
    public bool IsAtPlayer(BoxCollider2D col, Vector2 pos)
    {
        RaycastHit2D[] objects =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < objects.Length; i++)
            if (objects[i].transform.tag.Equals("Player"))
                return true;

        //int count = Physics2D.BoxCastNonAlloc
        //    (
        //        (Vector2)transform.position + GetAngleOffset(col) + pos,
        //        new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
        //        col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
        //        Vector2.zero, tempObjects, 1,
        //        1 << LayerMask.NameToLayer("Body")
        //    );

        //for (int i = 0; i < count; ++i)
        //    if (tempObjects[i].transform.tag.Equals("Player"))
        //        return true;

        return false;
    }
    #endregion

    #region[범위안에 지형이 있는지 검사]
    public bool IsAtTerrain(BoxCollider2D col, bool This = true)
    {
        return IsAtTerrain(col, Vector2.zero);
    }

    public bool IsAtTerrain(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        RaycastHit2D[] objects =
             Physics2D.BoxCastAll
                 (
                     (This ? (Vector2)transform.position : Vector2.zero) + GetAngleOffset(col) + pos,
                     new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                     col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                     Vector2.zero, 1,
                     1 << LayerMask.NameToLayer("Terrain")
                 );

        if(transform.tag == "Monster")
        {
            for (int i = 0; i < objects.Length; i++)
                if (objects[i].transform.tag.Equals("MonsterPass"))
                    return false;
        }

        return objects.Length > 0;
    }
    #endregion

    #region[매달리는 지형이 있는지 검사]
    public bool IsAtTerrainbyHang(BoxCollider2D col, bool This = true)
    {
        return IsAtTerrain(col, Vector2.zero);
    }

    public bool IsAtTerrainbyHang(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        RaycastHit2D[] objects =
        Physics2D.BoxCastAll
            (
                (This ? (Vector2)transform.position : Vector2.zero) + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Terrain")
            );

        for (int i = 0; i < objects.Length; i++)
            if (!objects[i].transform.tag.Equals("CantHang"))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 지형오브젝트가 있는지 검사]
    public bool IsAtTerrainObject(BoxCollider2D col, bool This = true)
    {
        return IsAtTerrainObject(col, Vector2.zero);
    }

    public bool IsAtTerrainObject(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        RaycastHit2D[] objects =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Terrain")
            );

        for (int i = 0; i < objects.Length; i++)
            if (objects[i].transform.tag.Equals("Object"))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 오브젝트가 있는지 검사]
    public bool IsAtObject(BoxCollider2D col, bool This = true)
    {
        return IsAtObject(col, Vector2.zero);
    }

    public bool IsAtObject(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        RaycastHit2D[] objects =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < objects.Length; i++)
            if (objects[i].transform.tag.Equals("Object"))
                return true;

        return false;
    }

    public bool IsAtItem(BoxCollider2D col, string name, bool This = true)
    {
        return IsAtItem(col, name, Vector2.zero);
    }
    public bool IsAtItem(BoxCollider2D col, string name, Vector2 pos, bool This = true)
    {
        RaycastHit2D[] objects =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < objects.Length; i++)
            if (objects[i].transform.name.Equals(name))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 특정테그 오브젝트 받기]

    public GameObject IsAtObjectWithTag(BoxCollider2D col,string tag)
    {
        return IsAtObjectWithTag(col, Vector2.zero, tag);
    }

    public GameObject IsAtObjectWithTag(BoxCollider2D col, Vector2 pos, string tag)
    {
        RaycastHit2D[] objects =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1
            );

        for (int i = 0; i < objects.Length; i++)
            if (objects[i].transform.tag.Equals(tag))
                return objects[i].transform.gameObject;

        return null;
    }
    #endregion

}
