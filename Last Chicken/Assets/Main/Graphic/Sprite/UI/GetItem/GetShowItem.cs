using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetShowItem : MonoBehaviour
{
    public static GetShowItem instance;

    public Text itemNameText;
    public Text itemExplainText;
    public GameObject getAni;
    Animator getAniAnimator;

    List<int> getlist = new List<int>();
    bool startAni = false;
    bool delay = false;
    void Awake()
    {
        instance = this;
        getAniAnimator = getAni.GetComponent<Animator>();
    }

    private void Update()
    {
        if(getlist.Count > 0)
        {
            if(!startAni)
            {
                getAniAnimator.Rebind();
                getAni.SetActive(true);
                startAni = true;
                delay = false;
                if(GameManager.instance.playData.language == PlayData.Language.한국어)
                {
                    itemNameText.text = ItemManager.instance.itemData[getlist[0]].itemName;
                    itemExplainText.text = ItemManager.instance.itemData[getlist[0]].itemExplain;
                }
                else if (GameManager.instance.playData.language == PlayData.Language.English)
                {
                    itemNameText.text = ItemManager.instance.itemData[getlist[0]].itemName_Eng;
                    itemExplainText.text = ItemManager.instance.itemData[getlist[0]].itemExplain_Eng;
                }
            }
            else if(getAniAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.9f)
            {
                getAni.SetActive(false);
                if(!delay)
                {
                    delay = true;
                    StartCoroutine(Delay());
                }
            }
        }
        else
            getAni.SetActive(false);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        startAni = false;
        getlist.RemoveAt(0);
    }

    public void AddShowList(int i)
    {
        getlist.Add(i);
    }

    public void AddShowList(string s)
    {
        int itemValue = ItemManager.FindData(s);
        
        AddShowList(itemValue);
    }
}
