using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProlgueManager : MonoBehaviour
{

    TextMeshProUGUI prolgueText;
    TextMeshProUGUI prolgueText_Eng;
    Image prolgueImg;
    Animator prolgueAnimator;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.Serializable]
    public class ProlgueData
    {
        public Sprite sprite;
        [TextArea]
        public string context;
        [TextArea]
        public string context_Eng;
    }

    [Header("프롤로그에 사용할 대사 및 이미지")]
    public List<ProlgueData> prolgueDatas;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    bool prolgueOut = false;
    bool prolgueOutFlag = false;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    void Awake()
    {
        Transform canvas = transform.Find("Canvas");

        prolgueImg = canvas.Find("PrologueImg").GetComponent<Image>();
        prolgueAnimator = canvas.Find("PrologueImg").GetComponent<Animator>();
        prolgueText = canvas.Find("PrologueText").GetComponent<TextMeshProUGUI>();
        prolgueText_Eng = canvas.Find("PrologueText_English").GetComponent<TextMeshProUGUI>();
    }
    #endregion

    #region[Start]
    void Start()
    {
        StartCoroutine(Prolgue(0));
    }
    #endregion

    #region[Update]
    void Update()
    {
        if (!prolgueOut && (Input.GetKeyDown(KeyManager.instance.keyBoard[GameKeyType.Select]) || KeyManager.GetKeyDown(KeyManager.instance.gamePad[GameKeyType.Attack])))
            prolgueOut = true;

        if (prolgueOut)
            if (!prolgueOutFlag)
            {
                prolgueOutFlag = true;
                if(!GameManager.instance.playData.firstGame)
                    SceneController.instance.MoveScene("Title");
                else
                    SceneController.instance.MoveScene("Tutorial");
            }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[프롤로그 코루틴]
    IEnumerator Prolgue(int n)
    {
        if (n >= prolgueDatas.Count)
        {
            prolgueOut = true;
            yield break;
        }
        string prolgueString = "";

        if (prolgueDatas[n].sprite)
            prolgueImg.sprite = prolgueDatas[n].sprite;
        prolgueAnimator.Rebind();
        if (GameManager.instance.playData.language == PlayData.Language.한국어)
        {
            if (prolgueDatas[n].context != null)
            {
                for (int i = 0; i < prolgueDatas[n].context.Length; i++)
                {
                    prolgueString += prolgueDatas[n].context[i];
                    string tempString = prolgueString;
                    tempString += "<color=#00000000>";
                    for (int j = i + 1; j < prolgueDatas[n].context.Length; j++)
                        tempString += prolgueDatas[n].context[j];
                    tempString += "</color>";
                    prolgueText.text = tempString;
                    if (!prolgueOut)
                        SoundManager.instance.BtnClick();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        else
        {
            if (prolgueDatas[n].context_Eng != null)
            {
                for (int i = 0; i < prolgueDatas[n].context_Eng.Length; i++)
                {
                    prolgueString += prolgueDatas[n].context_Eng[i];
                    string tempString = prolgueString;
                    tempString += "<color=#00000000>";
                    for (int j = i + 1; j < prolgueDatas[n].context_Eng.Length; j++)
                        tempString += prolgueDatas[n].context_Eng[j];
                    tempString += "</color>";
                    prolgueText_Eng.text = tempString;
                    if (!prolgueOut)
                        SoundManager.instance.BtnClick();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        yield return new WaitForSeconds(2f);
        if (prolgueOut)
            yield break;
        StartCoroutine(Prolgue(n + 1));
    }
    #endregion
}