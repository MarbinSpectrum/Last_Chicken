using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    public string item_name;

    public Image itemImage;

    public Text itemText;
    public Text noneData;

    void Update()
    {
        int itemNum = ItemManager.FindData(item_name);
        if(itemNum != -1)
        {
            itemImage.sprite = ItemManager.instance.itemData[itemNum].itemImg;
            if (GameManager.instance.playData.itemRecords[itemNum])
                itemImage.color = Color.white;
            else
                itemImage.color = Color.black;

            if (GameManager.instance.playData.itemRecords[itemNum])
            {
                noneData.gameObject.SetActive(false);
                itemText.gameObject.SetActive(true);
                itemText.text = GameManager.instance.playData.language == PlayData.Language.한국어 ? ItemManager.instance.itemData[itemNum].itemName : ItemManager.instance.itemData[itemNum].itemName_Eng;
            }
            else
            {
                noneData.gameObject.SetActive(true);
                itemText.gameObject.SetActive(false);
                noneData.text = "???";
            }

        }
    }
}
