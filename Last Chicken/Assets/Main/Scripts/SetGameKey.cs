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
    bool realCheck = false;
    public void OnGUI()
    {
        if (isRun)
        {
            Event keyEvent = Event.current;
            if (keyEvent.isKey)
            {
                KeyManager.instance.ChangeKeyBoardInput(gameKeyType, keyEvent.keyCode);
                runSetting = false;
                isRun = false;
            }
        }
    }

    public void Update()
    {
        if (!isRun)
        {
            if (gameController == GameController.KeyBoard)
            {
                if (KeyManager.instance.keyBoard[gameKeyType] == KeyCode.None)
                    text_UI.text = "?";
                else
                    text_UI.text = KeyManager.instance.keyBoard[gameKeyType].ToString();

            }
            else if (gameController == GameController.XBOX)
            {
                if (KeyManager.instance.gamePad[gameKeyType] == "A")
                    text_UI.text = "<color=#1DFF00>" + "[A]" + "</color>";
                else if (KeyManager.instance.gamePad[gameKeyType] == "B")
                    text_UI.text = "<color=#FF0000>" + "[B]" + "</color>";
                else if (KeyManager.instance.gamePad[gameKeyType] == "X")
                    text_UI.text = "<color=#009CFF>" + "[X]" + "</color>";
                else if (KeyManager.instance.gamePad[gameKeyType] == "Y")
                    text_UI.text = "<color=#FFC500>" + "[Y]" + "</color>";
                else
                    text_UI.text = "<color=#FFFFFF>[" + KeyManager.instance.gamePad[gameKeyType].ToString() + "]</color>";
            }
            else if (gameController == GameController.Wireless)
            {
                if (KeyManager.instance.gamePad[gameKeyType] == "□")
                    text_UI.text = "<color=#FF00A2>" + "[□]" + "</color>";
                else if (KeyManager.instance.gamePad[gameKeyType] == "Ⅹ")
                    text_UI.text = "<color=#009CFF>" + "[Ⅹ]" + "</color>";
                else if (KeyManager.instance.gamePad[gameKeyType] == "○")
                    text_UI.text = "<color=#FF0000>" + "[○]" + "</color>";
                else if (KeyManager.instance.gamePad[gameKeyType] == "△")
                    text_UI.text = "<color=#1DFF00>" + "[△]" + "</color>";
                else
                    text_UI.text = "<color=#FF00FF>[" + KeyManager.instance.gamePad[gameKeyType].ToString() + "]</color>";
            }
        }
        else
        {
            if (KeyManager.GetKeyUp(KeyManager.instance.gamePad[GameKeyType.Select]))
            {
                realCheck = true;
            }
            else if (realCheck && KeyManager.CheckJoyStick())
            {
                KeyManager.instance.ChangeGamePadInput(gameKeyType, KeyManager.GetJoyStickKey());
                runSetting = false;
                isRun = false;
            }

        }
    }

    public void InputGameKey()
    {
        if (runSetting)
            return;
        isRun = true;
        runSetting = true;
        realCheck = false;
        text_UI.text = "<color=#FFFFFF>_</color>";
    }
}
