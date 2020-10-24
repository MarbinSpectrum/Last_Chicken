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
            itemNameText.text += MonsterManager.instance.GetRecordData((MonsterManager.Monster)MonsterNum, GameManager.instance.playData.language, MonsterSetting.Name);
            string temp = string.Empty;
            string[] addString = MonsterManager.instance.GetRecordData((MonsterManager.Monster)MonsterNum, GameManager.instance.playData.language, MonsterSetting.Explain).Replace("\\n","\n").Split('\n');
            for (int i = 0; i < addString.Length; i++)
            {
                temp += addString[i];
                if (i != addString.Length - 1)
                    temp += '\n';
            }

            explainText.text = temp;
        }

    }

    public void PointExit()
    {
        flag = false;
        RecordManager.instance.explainRect.gameObject.SetActive(false);
    }
}
