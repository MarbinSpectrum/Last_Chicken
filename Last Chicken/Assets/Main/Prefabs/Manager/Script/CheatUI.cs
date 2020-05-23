using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatUI : MonoBehaviour
{
    public void GoToMap(string s)
    {
        SoundManager.instance.SelectMenu();
        UIManager.instance.ActPauseMenu(false);
        SceneController.instance.MoveScene(s);
    }
}
