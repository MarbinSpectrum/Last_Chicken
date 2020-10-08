using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstSetting : MonoBehaviour
{
    enum 진행도 { 로고,언어,배경음,효과음,종료};

    진행도 run = 진행도.로고;
    float time = 0;
    float time2 = 0;
    bool flag = false;
    bool flag2 = false;
    bool flag3 = false;
    bool chicken = false;
    bool bgm = false;

    public GameObject LogoObj;

    [Space(30)]

    public GameObject LanguageObj;
    public EventTrigger[] BtnTrigger;
    public Image[] btnImg;
    public Sprite[] selectImg;
    public Sprite[] notSelectImg;

    [Space(30)]

    public GameObject BgmObj;
    public GameObject[] bgmBanner;
    public EventTrigger BgmBtnTrigger;
    public Text BgmBtnText;
    public Slider BgmSlider;
    [Space(30)]

    public GameObject SeObj;
    public GameObject[] SeBanner;
    public EventTrigger SeBtnTrigger;
    public Text SeBtnText;
    public Slider SeSlider;

    [Space(30)]

    public Image fade;

    int selectBtn = 1;

    #region[Awake]
    public void Awake()
    {
        //언어설정 버튼
        for (int i = 0; i < BtnTrigger.Length; i++)
        {
            EventTrigger.Entry pEnter = new EventTrigger.Entry();
            pEnter.eventID = EventTriggerType.PointerEnter;
            int n = i;
            pEnter.callback.AddListener((data) =>
            {
                //if (KeyManager.nowController != GameController.KeyBoard)
                //    return;
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

        //배경음설정 버튼
        {

            EventTrigger.Entry pDown = new EventTrigger.Entry();
            pDown.eventID = EventTriggerType.PointerDown;
            pDown.callback.AddListener((data) =>
            {
                Select_BGM();
            });
            BgmBtnTrigger.triggers.Add(pDown);
        }

        //효과음설정 버튼
        {

            EventTrigger.Entry pDown = new EventTrigger.Entry();
            pDown.eventID = EventTriggerType.PointerDown;
            pDown.callback.AddListener((data) =>
            {
                Select_SE();
            });
            SeBtnTrigger.triggers.Add(pDown);
        }
    }
    #endregion

    public void Update()
    {
        time += Time.deltaTime;

        //팀 로고 표시
        ShowTeamLogo();

        //세팅을 스킵하는지 검사
        SkipSetting();

        //언어 설정;
        SettingLanguage();

        //배경음 설정
        SettingBGM();

        //효과음 설정
        SettingSE();
    }

    private void ShowTeamLogo()
    {
        if (run == 진행도.로고)
        {
            LogoObj.SetActive(true);
            if (!chicken && time > 2f)
            {
                chicken = true;
                SoundManager.instance.ChickenBark(0);
            }
        }
    }

    private void SkipSetting()
    {
        if (run == 진행도.로고 && time > 6f)
        {
            run = 진행도.언어;
            if (GameManager.instance.playData.firstGame == false)
                SceneController.instance.MoveScene("Prologue");
        }
    }

    private void SettingLanguage()
    {
        if (run == 진행도.언어)
        {
            LanguageObj.SetActive(true);

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

    private void Select_Language()
    {
        if (flag)
            return;
        flag = true;
        GameManager.instance.playData.language = (Language)selectBtn;
        if(GameManager.instance.playData.language == Language.English)
        {
            BgmBtnText.text = "OK";
            SeBtnText.text = "OK";
        }
        time = 0;
        run = 진행도.배경음;
        SoundManager.instance.BtnClick();
    }

    private void SettingBGM()
    {
        if (run == 진행도.배경음)
        {
            if (time < 1)
            {
                fade.raycastTarget = true;
                fade.color = new Color(0, 0, 0, time);
            }
            else if (time < 2)
            {
                fade.color = new Color(0, 0, 0, 1);
                LanguageObj.SetActive(false);
            }
            else if (time < 3)
            {
                if(!bgm)
                {
                    SoundManager.instance.Altar();
                    bgm = true;
                }
                fade.color = new Color(0, 0, 0, 3 - time);
                BgmObj.SetActive(true);

                for (int i = 0; i < bgmBanner.Length; i++)
                    bgmBanner[i].SetActive(false);
                bgmBanner[(int)GameManager.instance.playData.language].SetActive(true);
            }
            else
            {
                fade.raycastTarget = false;
                fade.color = new Color(0, 0, 0, 0);

                if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.SystemLeft]))
                {
                    BgmSlider.value -= 0.003f;
                    BgmSlider.value = Mathf.Max(BgmSlider.value, 0);
                }
                else if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.SystemRight]))
                {
                    BgmSlider.value += 0.003f;
                    BgmSlider.value = Mathf.Min(BgmSlider.value, 1);
                }

                SoundManager.instance.BGM.volume = BgmSlider.value;

                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    Select_BGM();
            }
        }
    }

    private void Select_BGM()
    {
        if (flag2)
            return;
        flag2 = true;
        SoundManager.instance.StopBGM_Sound();
        GameManager.instance.playData.BGM_Volume = BgmSlider.value;
        UIManager.instance.bgmSlider.value = BgmSlider.value;
        time = 0;
        run = 진행도.효과음;
        SoundManager.instance.BtnClick();
    }

    private void SettingSE()
    {
        if (run == 진행도.효과음)
        {
            if (time < 1)
            {
                fade.raycastTarget = true;
                fade.color = new Color(0, 0, 0, time);
            }
            else if (time < 2)
            {
                fade.color = new Color(0, 0, 0, 1);
                BgmObj.SetActive(false);
            }
            else if (time < 3)
            {
                fade.color = new Color(0, 0, 0, 3 - time);
                SeObj.SetActive(true);

                for (int i = 0; i < bgmBanner.Length; i++)
                    SeBanner[i].SetActive(false);
                SeBanner[(int)GameManager.instance.playData.language].SetActive(true);
            }
            else
            {
                time2 += Time.deltaTime;
                if (time2 > 2)
                {
                    SoundManager.instance.ChickenBark(false);
                    time2 = 0;
                }
                fade.raycastTarget = false;
                fade.color = new Color(0, 0, 0, 0);

                if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.SystemLeft]))
                {
                    SeSlider.value -= 0.003f;
                    SeSlider.value = Mathf.Max(SeSlider.value, 0);
                }
                else if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.SystemRight]))
                {
                    SeSlider.value += 0.003f;
                    SeSlider.value = Mathf.Min(SeSlider.value, 1);
                }

                SoundManager.instance.SE.volume = SeSlider.value;

                if (KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Select]))
                    Select_SE();
            }
        }
    }

    private void Select_SE()
    {
        if (flag3)
            return;
        flag3 = true;
        run = 진행도.종료;
        GameManager.instance.playData.SE_Volume = SeSlider.value;
        UIManager.instance.seSlider.value = SeSlider.value;
        SoundManager.instance.BtnClick();
        SceneController.instance.MoveScene("Prologue");
    }
}
