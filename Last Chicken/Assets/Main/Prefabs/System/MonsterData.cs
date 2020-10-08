using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterData : MonoBehaviour
{
    public string monster_name;

    public Image monsterImage;
    public Text NumText;
    bool flag = false;

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

            NumText.text = (monsterNum + 1).ToString("D3");
        }

        if (flag && Vector2.Distance(transform.position, Input.mousePosition) > 70)
            PointExit();
        else if (!flag && Vector2.Distance(transform.position, Input.mousePosition) <= 70)
            PointEnter();

        int pivot_x = Input.mousePosition.x < Screen.width / 2 ? 0 : 1;
        int pivot_y = Input.mousePosition.y < Screen.height / 2 ? 0 : 1;
        RecordManager.instance.explainRect.pivot = new Vector2(pivot_x, pivot_y);
        RecordManager.instance.explainRect.transform.position = Input.mousePosition;

    }

    public void PointEnter()
    {
        int MonsterNum = MonsterManager.FindData(monster_name);
        if (!GameManager.instance.playData.monsterRecords[MonsterNum])
            return;

        flag = true;
        RecordManager.instance.explainRect.gameObject.SetActive(true);

        Text itemNameText = RecordManager.instance.explainRect.transform.Find("ItemNameText").GetComponent<Text>();
        Text explainText = RecordManager.instance.explainRect.transform.Find("ExplainText").GetComponent<Text>();


        if (GameManager.instance.playData.monsterRecords[MonsterNum])
        {
            itemNameText.text = (MonsterNum + 1).ToString("D3") + ". ";
            itemNameText.text += GameManager.instance.playData.language == Language.한국어 ? MonsterManager.monsterName_KR[MonsterNum] : MonsterManager.monsterName[MonsterNum];
            string temp = string.Empty;
            string addString = GameManager.instance.playData.language == Language.한국어 ? 
                MonsterManager.monsterExplain[new KeyValuePair<MonsterManager.Monster, Language>((MonsterManager.Monster)MonsterNum,Language.한국어)] :
                MonsterManager.monsterExplain[new KeyValuePair<MonsterManager.Monster, Language>((MonsterManager.Monster)MonsterNum, Language.English)];
            temp += addString;
            explainText.text = temp;
        }

    }

    public void PointExit()
    {
        flag = false;
        RecordManager.instance.explainRect.gameObject.SetActive(false);
    }
}
