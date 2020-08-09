using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGameKey : MonoBehaviour
{

    public Text text_UI;
    public GameController gameController;
    public GameKeyType gameKeyType;
    public static bool runSetting = false;
    public bool isRun = false;
    public void OnGUI()
    {
        if (isRun)
        {
            Event keyEvent = Event.current;
            if (keyEvent.isKey)
            {
                KeyManager.instance.keyBoard[gameKeyType] = keyEvent.keyCode;

                runSetting = false;
                isRun = false;
            }
            else if(KeyManager.CheckJoyStick())
            {
                KeyManager.instance.gamePad[gameKeyType] = KeyManager.GetJoyStickKey();

                runSetting = false;
                isRun = false;
            }
        }
        else
        {
            if (gameController == GameController.KeyBoard)
                text_UI.text = KeyManager.instance.keyBoard[gameKeyType].ToString();
            else if (gameController == GameController.GamePad)
                text_UI.text = KeyManager.instance.gamePad[gameKeyType];
        }

        if(!runSetting)
            isRun = false;
    }

    public void InputGameKey()
    {
        if (runSetting)
            return;
        isRun = true;
        runSetting = true;
        text_UI.text = "_";
    }
}
