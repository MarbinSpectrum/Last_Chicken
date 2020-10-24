using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    public string item_name;

    public Image itemImage;
    public Text NumText;
    bool flag = false;

    void Update()
    {
        int itemNum = ItemManager.FindData(item_name);
        if (itemNum != -1)
        {
            NumText.text = (itemNum + 1).ToString("D3");
            itemImage.sprite = ItemManager.instance.itemData[itemNum].itemImg;
            if (GameManager.instance.playData.itemRecords[itemNum])
                itemImage.color = Color.white;
            else
                itemImage.color = Color.black;

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
        int itemNum = ItemManager.FindData(item_name);
        if (!GameManager.instance.playData.itemRecords[itemNum])
            return;

        flag = true;
        RecordManager.instance.explainRect.gameObject.SetActive(true);

        Text itemNameText = RecordManager.instance.explainRect.transform.Find("ItemNameText").GetComponent<Text>();
        Text explainText = RecordManager.instance.explainRect.transform.Find("ExplainText").GetComponent<Text>();


        if (GameManager.instance.playData.itemRecords[itemNum])
        {
            itemNameText.text = (itemNum + 1).ToString("D3") + ". ";
            itemNameText.text += ItemManager.instance.GetRecordData(itemNum, GameManager.instance.playData.language, ItemSetting.Name);
            string temp = "";
            string[] addString = ItemManager.instance.GetRecordData(itemNum, GameManager.instance.playData.language, ItemSetting.Explain).Replace("\\n", "\n").Replace("\"", "").Split('\n');
            for (int i = 0; i < addString.Length; i++)
            {
                temp += addString[i];
                if(i != addString.Length - 1)
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
