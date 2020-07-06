using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLanguageManager : MonoBehaviour
{
    bool flag = false;
    bool run = false;
    float time = 0;
    public void Update()
    {
        time += Time.deltaTime;
       // if (Input.GetKeyDown(KeyCode.Space))
          //  time = 4;
        if (!run && time > 4 && GameManager.instance.playData.firstGame == false)
        {
            run = true;
            SceneManager.LoadScene("Prologue");
        }
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
