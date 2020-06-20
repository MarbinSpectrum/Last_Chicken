using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IglooObject : CustomCollider
{
    public BoxCollider2D check;
    void Update()
    {
        if(IsAtPlayer(check) && Input.GetKeyDown(KeyCode.W) && Player.instance.nowHp > 0)
        {
            SceneController.instance.MoveScene("IglooMap");
            Player.instance.canControl = false;
            Player.instance.notDamage = true;
        }
    }
}
