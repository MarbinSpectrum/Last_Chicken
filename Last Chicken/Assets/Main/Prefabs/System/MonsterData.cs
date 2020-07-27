using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterData : MonoBehaviour
{
    public string monster_name;

    public Image monsterImage;

    public Text monsterText;
    public Text noneData;

    void Update()
    {
        int monsterNum = MonsterManager.FindData(monster_name);
        if(monsterNum != -1)
        {
            monsterImage.sprite = MonsterManager.instance.recordSprite[monsterNum];
            if (GameManager.instance.playData.monsterRecords[monsterNum])
                monsterImage.color = Color.white;
            else
                monsterImage.color = Color.black;

            if (GameManager.instance.playData.monsterRecords[monsterNum])
            {
                noneData.gameObject.SetActive(false);
                monsterText.gameObject.SetActive(true);
                monsterText.text = GameManager.instance.playData.language == PlayData.Language.한국어 ? MonsterManager.monsterName_KR[monsterNum] : MonsterManager.monsterName[monsterNum];
            }
            else
            {
                noneData.gameObject.SetActive(true);
                monsterText.gameObject.SetActive(false);
                noneData.text = "???";
            }

        }
    }
}
