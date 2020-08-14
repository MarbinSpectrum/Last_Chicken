using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLanguageManager : MonoBehaviour
{
    bool flag = false;
    bool run = false;
    bool chicken = false;
    float time = 0;
    public List<GameObject> obj = new List<GameObject>();

    public EventTrigger[] BtnTrigger;
    public Image[] btnImg;
    public Sprite[] selectImg;
    public Sprite[] notSelectImg;
    int selectBtn = 1;

    public void Awake()
    {
        for(int i = 0; i < BtnTrigger.Length; i++)
        {
            EventTrigger.Entry pEnter = new EventTrigger.Entry();
            pEnter.eventID = EventTriggerType.PointerEnter;
            int n = i;
            pEnter.callback.AddListener((data) =>
            {
                if (KeyManager.nowController != GameController.KeyBoard)
                    return;
                selectBtn = n;
            });
            BtnTrigger[i].triggers.Add(pEnter);

            EventTrigger.Entry pDown = new EventTrigger.Entry();
            pDown.eventID = EventTriggerType.PointerDown;
            pDown.callback.AddListener((data) =>
            {
                Select_Language();
            });
            BtnTrigger[i].triggers.Add(pDown);
        }

    }

    public void Update()
    {
        time += Time.deltaTime;

        if (!chicken && time > 2f)
        {
            chicken = true;
            SoundManager.instance.ChickenBark(0);
        }
        if (!run && time > 6f)
        {
            run = true;
            if (GameManager.instance.playData.firstGame == false)
                SceneController.instance.MoveScene("Prologue");
            else
            {
                for (int i = 0; i < obj.Count; i++)
                    obj[i].SetActive(true);
            }
        }
        if(run)
        {


            if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.SystemUp]) || KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.SystemDown]))
            {
                selectBtn++;
                selectBtn %= 2;
            }

            for (int i = 0; i < btnImg.Length; i++)
            {
                if (selectBtn == i)
                    btnImg[i].sprite = selectImg[i];
                else
                    btnImg[i].sprite = notSelectImg[i];
            }

            if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                Select_Language();
        }
    }

    public void Select_Language()
    {
        if (flag)
            return;
        flag = true;
        GameManager.instance.playData.language = (PlayData.Language)selectBtn;
        SceneController.instance.MoveScene("Prologue");
        SoundManager.instance.BtnClick();
    }
}
