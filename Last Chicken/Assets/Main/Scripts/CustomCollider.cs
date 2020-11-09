using UnityEngine;
using System.Collections.Generic;

public class CustomCollider : MonoBehaviour
{
    public const int CHECKCOUNT = 25;

    public const string CONVEYORBELT = "ConveyorBelt";
    public const string BODY = "Body";
    public const string CHICKEN = "Chicken";
    public const string PLAYER = "Player";
    public const string TERRAIN = "Terrain";
    public const string MONSTER = "Monster";
    public const string MONSTER_PASS = "MonsterPass";
    public const string CANT_HANG = "CantHang";
    public const string OBJECT = "Object";
    public const string DAMAGE = "Damage";
    public const string MOVE = "Move";
    public const string STOP = "Stop";
    public const string RANDOM = "Random";
    public const string MINECART = "MineCart";
    public const string LIGHT = "Light";

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
    RaycastHit2D[] IsAtChickenArray = new RaycastHit2D[CHECKCOUNT];
    public bool IsAtChicken(BoxCollider2D col)
    {
        return IsAtChicken(col, Vector2.zero);
    }
    public bool IsAtChicken(BoxCollider2D col, Vector2 pos)
    {
        if (Vector2.Distance(Chicken.instance.transform.position, (Vector2)transform.position + pos) > Mathf.Sqrt(col.size.x * col.size.x + col.size.y * col.size.y))
            return false;
        int count = Physics2D.BoxCastNonAlloc
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, IsAtChickenArray,0,
                1 << LayerMask.NameToLayer(BODY)
            );

        for (int i = 0; i < count; i++)
            if (IsAtChickenArray[i].transform.tag.Equals(CHICKEN))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 플레이어가 있는지 검사]
    RaycastHit2D[] IsAtPlayerArray = new RaycastHit2D[CHECKCOUNT];
    public bool IsAtPlayer(BoxCollider2D col)
    {
        return IsAtPlayer(col, Vector2.zero);
    }

    public bool IsAtPlayer(BoxCollider2D col, Vector2 pos)
    {
        if(Vector2.Distance(Player.instance.transform.position,(Vector2)transform.position + pos) > Mathf.Max(col.size.x ,col.size.y)*2)
            return false;

        int count = Physics2D.BoxCastNonAlloc
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, IsAtPlayerArray,0,
                1 << LayerMask.NameToLayer(BODY)
            );

        for (int i = 0; i < count; ++i)
            if (IsAtPlayerArray[i].transform.tag.Equals(PLAYER))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 지형이 있는지 검사]
    RaycastHit2D[] IsAtTerrainArray = new RaycastHit2D[CHECKCOUNT];
    public bool IsAtTerrain(BoxCollider2D col, bool This = true)
    {
        return IsAtTerrain(col, Vector2.zero);
    }

    public bool IsAtTerrain(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        int count =
             Physics2D.BoxCastNonAlloc
                 (
                     (This ? (Vector2)transform.position : Vector2.zero) + GetAngleOffset(col) + pos,
                     new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                     col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                     Vector2.zero, IsAtTerrainArray,0,
                     1 << LayerMask.NameToLayer(TERRAIN)
                 );

        if(transform.tag == MONSTER)
        {
            for (int i = 0; i < count; i++)
                if (IsAtTerrainArray[i].transform.tag.Equals(MONSTER_PASS))
                    return false;
        }

        for (int i = 0; i < count; i++)
            if (IsAtTerrainArray[i])
                return true;
        return false;
    }
    #endregion

    #region[매달리는 지형이 있는지 검사]
    RaycastHit2D[] IsAtTerrainbyHangArray = new RaycastHit2D[CHECKCOUNT];
    public bool IsAtTerrainbyHang(BoxCollider2D col, bool This = true)
    {
        return IsAtTerrain(col, Vector2.zero);
    }

    public bool IsAtTerrainbyHang(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        int count =
        Physics2D.BoxCastNonAlloc
            (
                (This ? (Vector2)transform.position : Vector2.zero) + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, IsAtTerrainbyHangArray, 0,
                1 << LayerMask.NameToLayer(TERRAIN)
            );

        for (int i = 0; i < count; i++)
            if (!IsAtTerrainbyHangArray[i].transform.tag.Equals(CANT_HANG))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 지형오브젝트가 있는지 검사]
    RaycastHit2D[] IsAtTerrainObjectArray = new RaycastHit2D[CHECKCOUNT];
    public bool IsAtTerrainObject(BoxCollider2D col, bool This = true)
    {
        return IsAtTerrainObject(col, Vector2.zero);
    }

    public bool IsAtTerrainObject(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        int count = 
        Physics2D.BoxCastNonAlloc
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, IsAtTerrainObjectArray, 0,
                1 << LayerMask.NameToLayer(TERRAIN)
            );

        for (int i = 0; i < count; i++)
            if (IsAtTerrainObjectArray[i].transform.tag.Equals(OBJECT))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 오브젝트가 있는지 검사]
    RaycastHit2D[] IsAtObjectArray = new RaycastHit2D[CHECKCOUNT];
    public bool IsAtObject(BoxCollider2D col, bool This = true)
    {
        return IsAtObject(col, Vector2.zero);
    }

    public bool IsAtObject(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        int count =
        Physics2D.BoxCastNonAlloc
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, IsAtObjectArray, 0,
                1 << LayerMask.NameToLayer(BODY)
            );

        for (int i = 0; i < count; i++)
            if (IsAtObjectArray[i].transform.tag.Equals(OBJECT))
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
    RaycastHit2D[] IsAtObjectWithTagArray = new RaycastHit2D[CHECKCOUNT];
    public GameObject IsAtObjectWithTag(BoxCollider2D col,string tag)
    {
        return IsAtObjectWithTag(col, Vector2.zero, tag);
    }

    public GameObject IsAtObjectWithTag(BoxCollider2D col, Vector2 pos, string tag)
    {
        int count =
        Physics2D.BoxCastNonAlloc
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, IsAtObjectWithTagArray,0
            );

        for (int i = 0; i < count; i++)
            if (IsAtObjectWithTagArray[i].transform.tag.Equals(tag))
                return IsAtObjectWithTagArray[i].transform.gameObject;

        return null;
    }
    #endregion

}
