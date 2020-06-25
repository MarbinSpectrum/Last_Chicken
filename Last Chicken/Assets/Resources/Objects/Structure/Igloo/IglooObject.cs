using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IglooObject : CustomCollider
{
    public BoxCollider2D check;
    public GameObject enterUi;
    public List<GameObject> languageData = new List<GameObject>();
    bool used = false;

    #region[Update]
    void Update()
    {
        Language();
        EnterIgloo();
    }
    #endregion

    #region[언어]
    void Language()
    {
        for (int i = 0; i < languageData.Count; i++)
            if (languageData[i])
                languageData[i].SetActive(languageData[i].transform.name.Contains(GameManager.instance.playData.language.ToString()));
    }
    #endregion

    #region[이글루들어가기]
    void EnterIgloo()
    {
        enterUi.SetActive(!used && IsAtPlayer(check));
        if (IsAtPlayer(check) && Input.GetKeyDown(KeyCode.W) && Player.instance.canControl && !used)
        {
            used = true;
            SceneController.instance.MoveScene("IglooMap");
            Player.instance.canControl = false;
            Player.instance.notDamage = true;
        }
    }
    #endregion

}
