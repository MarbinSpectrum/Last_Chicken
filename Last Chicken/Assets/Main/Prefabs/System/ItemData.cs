using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    public string item_name;

    public Image itemImage;
    public Text NumText;

     void Update()
    {
        int itemNum = ItemManager.FindData(item_name);
        if(itemNum != -1)
        {
            NumText.text = (itemNum + 1).ToString("D3");
            itemImage.sprite = ItemManager.instance.itemData[itemNum].itemImg;
            if (GameManager.instance.playData.itemRecords[itemNum])
                itemImage.color = Color.white;
            else
                itemImage.color = Color.black;

        }
    }

    public void PointEnter()
    {
        int itemNum = ItemManager.FindData(item_name);
        if (!GameManager.instance.playData.itemRecords[itemNum])
            return;
        RecordManager.instance.explainRect.gameObject.SetActive(true);

        int pivot_x = Input.mousePosition.x < Screen.width / 2 ? 0 : 1;
        int pivot_y = Input.mousePosition.y < Screen.height / 2 ? 0 : 1;
        RecordManager.instance.explainRect.pivot = new Vector2(pivot_x, pivot_y);
        RecordManager.instance.explainRect.transform.position = transform.position;

        Text itemNameText = RecordManager.instance.explainRect.transform.Find("ItemNameText").GetComponent<Text>();
        Text explainText = RecordManager.instance.explainRect.transform.Find("ExplainText").GetComponent<Text>();


        if (GameManager.instance.playData.itemRecords[itemNum])
        {
            itemNameText.text = (itemNum + 1).ToString("D3") + ". ";
            itemNameText.text += GameManager.instance.playData.language == PlayData.Language.한국어 ? ItemManager.instance.itemData[itemNum].itemName : ItemManager.instance.itemData[itemNum].itemName_Eng;
            string temp = string.Empty;
            if (ItemManager.instance.itemData[itemNum].spawnShop)
            {
                for (int i = 0; i < 3; i++)
                {
                    string addString = GameManager.instance.playData.language == PlayData.Language.한국어 ? ItemManager.instance.itemData[itemNum].shopItemExplain[i] : ItemManager.instance.itemData[itemNum].shopItemExplain_Eng[i];
                    if (string.IsNullOrEmpty(addString))
                        break;
                    if (!string.IsNullOrEmpty(temp))
                        temp += "\n";
                    temp += addString;
                }
            }
            else
            {
                string addString = GameManager.instance.playData.language == PlayData.Language.한국어 ? ItemManager.instance.itemData[itemNum].itemExplain : ItemManager.instance.itemData[itemNum].itemExplain_Eng;
                temp += addString;
            }
            explainText.text = temp;
        }

    }

    public void PointExit()
    {
        RecordManager.instance.explainRect.gameObject.SetActive(false);
    }
}
