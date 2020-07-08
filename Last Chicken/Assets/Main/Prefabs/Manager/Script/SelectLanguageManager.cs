using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLanguageManager : MonoBehaviour
{
    bool flag = false;
    bool run = false;
    bool chicken = false;
    float time = 0;
    public List<GameObject> obj = new List<GameObject>();

    public void Update()
    {
        time += Time.deltaTime;
        // if (Input.GetKeyDown(KeyCode.Space))
        //  time = 4;

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
