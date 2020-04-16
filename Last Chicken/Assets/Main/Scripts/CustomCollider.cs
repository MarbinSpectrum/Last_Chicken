using UnityEngine;

public class CustomCollider : MonoBehaviour
{
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
        RaycastHit2D[] monsters =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < monsters.Length; i++)
            if (monsters[i].transform.tag.Equals("Chicken"))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 플레이어가 있는지 검사]
    public bool IsAtPlayer(BoxCollider2D col)
    {
        return IsAtPlayer(col, Vector2.zero);
    }

    public bool IsAtPlayer(BoxCollider2D col, Vector2 pos)
    {
        RaycastHit2D[] monsters =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < monsters.Length; i++)
            if (monsters[i].transform.tag.Equals("Player"))
                return true;

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
        bool check =
            Physics2D.BoxCast
            (
                (This ? (Vector2)transform.position : Vector2.zero) + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Terrain")
            );

        return check;
    }
    #endregion

    #region[범위안에 오브젝트가 있는지 검사]
    public bool IsAtObject(BoxCollider2D col, bool This = true)
    {
        return IsAtObject(col, Vector2.zero);
    }

    public bool IsAtObject(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        RaycastHit2D[] monsters =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Body")
            );

        for (int i = 0; i < monsters.Length; i++)
            if (monsters[i].transform.tag.Equals("Object"))
                return true;

        return false;
    }
    #endregion

    #region[범위안에 오브젝트가 있는지 검사]
    public bool IsAtTerrainObject(BoxCollider2D col, bool This = true)
    {
        return IsAtTerrainObject(col, Vector2.zero);
    }

    public bool IsAtTerrainObject(BoxCollider2D col, Vector2 pos, bool This = true)
    {
        RaycastHit2D[] monsters =
        Physics2D.BoxCastAll
            (
                (Vector2)transform.position + GetAngleOffset(col) + pos,
                new Vector2(col.size.x * Mathf.Abs(transform.localScale.x), col.size.y * Mathf.Abs(transform.localScale.y)),
                col.transform.eulerAngles.z * (transform.localScale.x < 0 ? -1 : 1),
                Vector2.zero, 1,
                1 << LayerMask.NameToLayer("Terrain")
            );

        for (int i = 0; i < monsters.Length; i++)
            if (monsters[i].transform.tag.Equals("Object"))
                return true;

        return false;
    }
    #endregion
}
