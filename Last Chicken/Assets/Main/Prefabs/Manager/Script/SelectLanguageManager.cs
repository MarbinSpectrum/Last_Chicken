using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLanguageManager : MonoBehaviour
{
    bool flag = false;
    public void Start()
    {
        if(GameManager.instance.playData.firstGame == false)
            SceneManager.LoadScene("Prologue");
    }

    public void Select_Language(int language)
    {
        if (flag)
            return;
        flag = true;
        GameManager.instance.playData.language = (PlayData.Language)language;
        SceneController.instance.MoveScene("Prologue");
        SoundManager.instance.BtnClick();
    }
}
