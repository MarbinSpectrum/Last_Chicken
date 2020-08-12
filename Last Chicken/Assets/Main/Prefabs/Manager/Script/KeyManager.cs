using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameController
{
    KeyBoard,XBOX, Wireless
}
public enum GameKeyType
{
    Left,Right,Up,Down,Jump,Attack,
    ItemGet,ItemTrash,ItemChange,ItemUse,
    Map
}

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    public static GameController nowController;
    public static float HorizonScale;
    public static float VerticalScale;

    public static float _lastRightTrigger;
    public static bool rightTriggerDown;
    public static bool rightTriggerUp;

    public static float _lastLeftTrigger;
    public static bool leftTriggerDown;
    public static bool leftTriggerUp;

    //////////////////////////////////////////////////////////////////////////////

    public static float _lastLeftArrow;
    public static bool leftArrowDown;
    public static bool leftArrowUp;

    public static float _lastRightArrow;
    public static bool rightArrowDown;
    public static bool rightArrowUp;

    public static float _lastUpArrow;
    public static bool upArrowDown;
    public static bool upArrowUp;

    public static float _lastDownArrow;
    public static bool downArrowDown;
    public static bool downArrowUp;

    //////////////////////////////////////////////////////////////////////////////

    public static float _lastLeftStick;
    public static bool leftStickDown;
    public static bool leftStickUp;

    public static float _lastRightStick;
    public static bool rightStickDown;
    public static bool rightStickUp;

    public static float _lastUpStick;
    public static bool upStickDown;
    public static bool upStickUp;

    public static float _lastDownStick;
    public static bool downStickDown;
    public static bool downStickUp;


    public Dictionary<GameKeyType, KeyCode> keyBoard = new Dictionary<GameKeyType, KeyCode>();
    public Dictionary<GameKeyType, string> gamePad = new Dictionary<GameKeyType, string>();
    public float checkSize = 0.5f;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;

            keyBoard[GameKeyType.Left] = GameManager.instance.playData.keyBoardList[0];
            keyBoard[GameKeyType.Right] = GameManager.instance.playData.keyBoardList[1];
            keyBoard[GameKeyType.Up] = GameManager.instance.playData.keyBoardList[2];
            keyBoard[GameKeyType.Down] = GameManager.instance.playData.keyBoardList[3];
            keyBoard[GameKeyType.Jump] = GameManager.instance.playData.keyBoardList[4];
            keyBoard[GameKeyType.Attack] = GameManager.instance.playData.keyBoardList[5];
            keyBoard[GameKeyType.ItemGet] = GameManager.instance.playData.keyBoardList[6];
            keyBoard[GameKeyType.ItemTrash] = GameManager.instance.playData.keyBoardList[7];
            keyBoard[GameKeyType.ItemChange] = GameManager.instance.playData.keyBoardList[8];
            keyBoard[GameKeyType.ItemUse] = GameManager.instance.playData.keyBoardList[9];
            keyBoard[GameKeyType.Map] = GameManager.instance.playData.keyBoardList[10];

            gamePad[GameKeyType.Left] = GameManager.instance.playData.gamePadListXBOX[0];
            gamePad[GameKeyType.Right] = GameManager.instance.playData.gamePadListXBOX[1];
            gamePad[GameKeyType.Up] = GameManager.instance.playData.gamePadListXBOX[2];
            gamePad[GameKeyType.Down] = GameManager.instance.playData.gamePadListXBOX[3];
            gamePad[GameKeyType.Jump] = GameManager.instance.playData.gamePadListXBOX[4];
            gamePad[GameKeyType.Attack] = GameManager.instance.playData.gamePadListXBOX[5];
            gamePad[GameKeyType.ItemGet] = GameManager.instance.playData.gamePadListXBOX[6];
            gamePad[GameKeyType.ItemTrash] = GameManager.instance.playData.gamePadListXBOX[7];
            gamePad[GameKeyType.ItemChange] = GameManager.instance.playData.gamePadListXBOX[8];
            gamePad[GameKeyType.ItemUse] = GameManager.instance.playData.gamePadListXBOX[9];
            gamePad[GameKeyType.Map] = GameManager.instance.playData.gamePadListXBOX[10];
        }
    }

    public void Update()
    {
        if (nowController == GameController.XBOX)
        {
            gamePad[GameKeyType.Left] = GameManager.instance.playData.gamePadListXBOX[0];
            gamePad[GameKeyType.Right] = GameManager.instance.playData.gamePadListXBOX[1];
            gamePad[GameKeyType.Up] = GameManager.instance.playData.gamePadListXBOX[2];
            gamePad[GameKeyType.Down] = GameManager.instance.playData.gamePadListXBOX[3];
            gamePad[GameKeyType.Jump] = GameManager.instance.playData.gamePadListXBOX[4];
            gamePad[GameKeyType.Attack] = GameManager.instance.playData.gamePadListXBOX[5];
            gamePad[GameKeyType.ItemGet] = GameManager.instance.playData.gamePadListXBOX[6];
            gamePad[GameKeyType.ItemTrash] = GameManager.instance.playData.gamePadListXBOX[7];
            gamePad[GameKeyType.ItemChange] = GameManager.instance.playData.gamePadListXBOX[8];
            gamePad[GameKeyType.ItemUse] = GameManager.instance.playData.gamePadListXBOX[9];
            gamePad[GameKeyType.Map] = GameManager.instance.playData.gamePadListXBOX[10];
        }
        else if (nowController == GameController.Wireless)
        {
            gamePad[GameKeyType.Left] = GameManager.instance.playData.gamePadListPS[0];
            gamePad[GameKeyType.Right] = GameManager.instance.playData.gamePadListPS[1];
            gamePad[GameKeyType.Up] = GameManager.instance.playData.gamePadListPS[2];
            gamePad[GameKeyType.Down] = GameManager.instance.playData.gamePadListPS[3];
            gamePad[GameKeyType.Jump] = GameManager.instance.playData.gamePadListPS[4];
            gamePad[GameKeyType.Attack] = GameManager.instance.playData.gamePadListPS[5];
            gamePad[GameKeyType.ItemGet] = GameManager.instance.playData.gamePadListPS[6];
            gamePad[GameKeyType.ItemTrash] = GameManager.instance.playData.gamePadListPS[7];
            gamePad[GameKeyType.ItemChange] = GameManager.instance.playData.gamePadListPS[8];
            gamePad[GameKeyType.ItemUse] = GameManager.instance.playData.gamePadListPS[9];
            gamePad[GameKeyType.Map] = GameManager.instance.playData.gamePadListPS[10];
        }

        if (nowController == GameController.KeyBoard)
        {
            if (Input.GetKey(keyBoard[GameKeyType.Left]) && Input.GetKey(keyBoard[GameKeyType.Right]))
                HorizonScale = 0;
            else if (Input.GetKey(keyBoard[GameKeyType.Left]) && !Input.GetKey(keyBoard[GameKeyType.Right]))
                HorizonScale = -1;
            else if (!Input.GetKey(keyBoard[GameKeyType.Left]) && Input.GetKey(keyBoard[GameKeyType.Right]))
                HorizonScale = +1;
            else
                HorizonScale = 0;

            if (Input.GetKey(keyBoard[GameKeyType.Up]) && Input.GetKey(keyBoard[GameKeyType.Down]))
                VerticalScale = 0;
            else if (Input.GetKey(keyBoard[GameKeyType.Up]) && !Input.GetKey(keyBoard[GameKeyType.Down]))
                VerticalScale = +1;
            else if (!Input.GetKey(keyBoard[GameKeyType.Up]) && Input.GetKey(keyBoard[GameKeyType.Down]))
                VerticalScale = -1;
            else
                VerticalScale = 0;
        }
        else
        {
            HorizonScale = Input.GetAxisRaw("Horizontal");

            VerticalScale = Input.GetAxisRaw("Vertical");

            #region[Left & Right Trigger]
            string rightTrigger = "Right Trigger " + nowController.ToString();
            if (rightTriggerDown)
                rightTriggerDown = false;
            if (rightTriggerUp)
                rightTriggerUp = false;
            if (_lastRightTrigger != Input.GetAxisRaw(rightTrigger))
            {
                if (_lastRightTrigger < 1f && Input.GetAxisRaw(rightTrigger) == 1)
                    rightTriggerDown = true;
                if (_lastRightTrigger > 0f && Input.GetAxisRaw(rightTrigger) == 0)
                    rightTriggerUp = true;
                _lastRightTrigger = Input.GetAxisRaw(rightTrigger);
            }
            string leftTrigger = "Left Trigger " + nowController.ToString();
            //Debug.Log(Input.GetAxisRaw(leftTrigger) + "," + Input.GetAxisRaw(rightTrigger));
            if (leftTriggerDown)
                leftTriggerDown = false;
            if (leftTriggerUp)
                leftTriggerUp = false;
            if (_lastLeftTrigger != Input.GetAxisRaw(leftTrigger))
            {
                if (_lastLeftTrigger < 1f && Input.GetAxisRaw(leftTrigger) == 1)
                    leftTriggerDown = true;
                if (_lastLeftTrigger > 0f && Input.GetAxisRaw(leftTrigger) == 0)
                    leftTriggerUp = true;
                _lastLeftTrigger = Input.GetAxisRaw(leftTrigger);
            }
            #endregion

            #region[Left & Right & Up & Down Arrow]
            string horizontalArrow = "Horizontal Arrow " + nowController.ToString();
            if (leftArrowDown)
                leftArrowDown = false;
            if (leftArrowUp)
                leftArrowUp = false;
            if (_lastLeftArrow != Input.GetAxisRaw(horizontalArrow))
            {
                if (_lastLeftArrow > -1f && Input.GetAxisRaw(horizontalArrow) == -1)
                    leftArrowDown = true;
                if (_lastLeftArrow < 0f && Input.GetAxisRaw(horizontalArrow) == 0)
                    leftArrowUp = true;
                _lastLeftArrow = Input.GetAxisRaw(horizontalArrow);
            }
            if (rightArrowDown)
                rightArrowDown = false;
            if (rightArrowUp)
                rightArrowUp = false;
            if (_lastRightArrow != Input.GetAxisRaw(horizontalArrow))
            {
                if (_lastRightArrow < 1f && Input.GetAxisRaw(horizontalArrow) == 1)
                    rightArrowDown = true;
                if (_lastRightArrow > 0f && Input.GetAxisRaw(horizontalArrow) == 0)
                    rightArrowUp = true;
                _lastRightArrow = Input.GetAxisRaw(horizontalArrow);
            }
            string verticalArrow = "Vertical Arrow " + nowController.ToString();
            if (downArrowDown)
                downArrowDown = false;
            if (downArrowUp)
                downArrowUp = false;
            if (_lastDownArrow != Input.GetAxisRaw(verticalArrow))
            {
                if (_lastDownArrow > -1f && Input.GetAxisRaw(verticalArrow) == -1)
                    downArrowDown = true;
                if (_lastDownArrow < 0f && Input.GetAxisRaw(verticalArrow) == 0)
                    downArrowUp = true;
                _lastDownArrow = Input.GetAxisRaw(verticalArrow);
            }
            if (upArrowDown)
                upArrowDown = false;
            if (upArrowUp)
                upArrowUp = false;
            if (_lastUpArrow != Input.GetAxisRaw(verticalArrow))
            {
                if (_lastUpArrow < 1f && Input.GetAxisRaw(verticalArrow) == 1)
                    upArrowDown = true;
                if (_lastUpArrow > 0f && Input.GetAxisRaw(verticalArrow) == 0)
                    upArrowUp = true;
                _lastUpArrow = Input.GetAxisRaw(verticalArrow);
            }
            #endregion

            #region[Left & Right & Up & Down Stick]
            if (leftStickDown)
                leftStickDown = false;
            if (leftStickUp)
                leftStickUp = false;
            if (_lastLeftStick != Input.GetAxisRaw("Horizontal"))
            {
                if (_lastLeftStick > -checkSize && Input.GetAxisRaw("Horizontal") >= -checkSize)
                    leftStickDown = true;
                if (_lastLeftStick < 0f && Input.GetAxisRaw("Horizontal") >= 0)
                    leftStickUp = true;
                _lastLeftStick = Input.GetAxisRaw("Horizontal");
            }
            if (rightStickDown)
                rightStickDown = false;
            if (rightStickUp)
                rightStickUp = false;
            if (_lastRightStick != Input.GetAxisRaw("Horizontal"))
            {
                if (_lastRightStick < checkSize && Input.GetAxisRaw("Horizontal") >= checkSize)
                    rightStickDown = true;
                if (_lastRightStick > 0f && Input.GetAxisRaw("Horizontal") <= 0)
                    rightStickUp = true;
                _lastRightStick = Input.GetAxisRaw("Horizontal");
            }
            if (downStickDown)
                downStickDown = false;
            if (downStickUp)
                downStickUp = false;
            if (_lastDownStick != Input.GetAxisRaw("Vertical"))
            {
                if (_lastDownStick > -checkSize && Input.GetAxisRaw("Vertical") >= -checkSize)
                    downStickDown = true;
                if (_lastDownStick < 0f && Input.GetAxisRaw("Vertical") >= 0)
                    downStickUp = true;
                _lastDownStick = Input.GetAxisRaw("Vertical");
            }
            if (upStickDown)
                upStickDown = false;
            if (upStickUp)
                upStickUp = false;
            if (_lastUpStick != Input.GetAxisRaw("Vertical"))
            {
                if (_lastUpStick < checkSize && Input.GetAxisRaw("Vertical") >= checkSize)
                    upStickDown = true;
                if (_lastUpStick > 0f && Input.GetAxisRaw("Vertical") <= 0)
                    upStickUp = true;
                _lastUpStick = Input.GetAxisRaw("Vertical");
            }
            #endregion
        }
    }

    public void ChangeKeyBoardInput(GameKeyType keyType, KeyCode keyCode)
    {
        if (keyBoard.ContainsValue(keyCode))
        {
            for (int i = 0; i < keyBoard.Count; i++)
            {
                GameKeyType temp = (GameKeyType)(i);
                if(keyBoard[temp] == keyCode)
                    keyBoard[temp] = KeyCode.None;
            }
        }
        keyBoard[keyType] = keyCode;

        SaveKeyData();
    }

    public void ChangeGamePadInput(GameKeyType keyType, string keyValue)
    {
        if (nowController == GameController.XBOX)
        {
            for (int i = 0; i < GameManager.instance.playData.gamePadListXBOX.Count; i++)
                if (GameManager.instance.playData.gamePadListXBOX[i] == keyValue)
                {
                    gamePad[(GameKeyType)(i)] = "?";
                    GameManager.instance.playData.gamePadListXBOX[i] = "?";
                    break;
                }

            gamePad[keyType] = keyValue;
            GameManager.instance.playData.gamePadListXBOX[(int)keyType] = keyValue;
        }
        else if (nowController == GameController.Wireless)
        {
            for (int i = 0; i < GameManager.instance.playData.gamePadListPS.Count; i++)
                if (GameManager.instance.playData.gamePadListPS[i] == keyValue)
                {
                    gamePad[(GameKeyType)(i)] = "?";
                    GameManager.instance.playData.gamePadListPS[i] = "?";
                    break;
                }

            gamePad[keyType] = keyValue;
            GameManager.instance.playData.gamePadListPS[(int)keyType] = keyValue;
        }
        SaveKeyData();
    }

    public static KeyCode GetJoyStickKeyCode()
    {
        for (KeyCode key = KeyCode.JoystickButton0; key != KeyCode.JoystickButton19; key++)
            if (Input.GetKey(key))
                return key;
        return KeyCode.None;
    }

    public void SaveKeyData()
    {
        if(nowController == GameController.XBOX)
        {
            GameManager.instance.playData.gamePadListXBOX[0] = gamePad[GameKeyType.Left];
            GameManager.instance.playData.gamePadListXBOX[1] = gamePad[GameKeyType.Right];
            GameManager.instance.playData.gamePadListXBOX[2] = gamePad[GameKeyType.Up];
            GameManager.instance.playData.gamePadListXBOX[3] = gamePad[GameKeyType.Down];
            GameManager.instance.playData.gamePadListXBOX[4] = gamePad[GameKeyType.Jump];
            GameManager.instance.playData.gamePadListXBOX[5] = gamePad[GameKeyType.Attack];
            GameManager.instance.playData.gamePadListXBOX[6] = gamePad[GameKeyType.ItemGet];
            GameManager.instance.playData.gamePadListXBOX[7] = gamePad[GameKeyType.ItemTrash];
            GameManager.instance.playData.gamePadListXBOX[8] = gamePad[GameKeyType.ItemChange];
            GameManager.instance.playData.gamePadListXBOX[9] = gamePad[GameKeyType.ItemUse];
            GameManager.instance.playData.gamePadListXBOX[10] = gamePad[GameKeyType.Map];
        }
        else if (nowController == GameController.Wireless)
        {
            GameManager.instance.playData.gamePadListPS[0] = gamePad[GameKeyType.Left];
            GameManager.instance.playData.gamePadListPS[1] = gamePad[GameKeyType.Right];
            GameManager.instance.playData.gamePadListPS[2] = gamePad[GameKeyType.Up];
            GameManager.instance.playData.gamePadListPS[3] = gamePad[GameKeyType.Down];
            GameManager.instance.playData.gamePadListPS[4] = gamePad[GameKeyType.Jump];
            GameManager.instance.playData.gamePadListPS[5] = gamePad[GameKeyType.Attack];
            GameManager.instance.playData.gamePadListPS[6] = gamePad[GameKeyType.ItemGet];
            GameManager.instance.playData.gamePadListPS[7] = gamePad[GameKeyType.ItemTrash];
            GameManager.instance.playData.gamePadListPS[8] = gamePad[GameKeyType.ItemChange];
            GameManager.instance.playData.gamePadListPS[9] = gamePad[GameKeyType.ItemUse];
            GameManager.instance.playData.gamePadListPS[10] = gamePad[GameKeyType.Map];
        }
    

        GameManager.instance.playData.keyBoardList[0] = keyBoard[GameKeyType.Left];
        GameManager.instance.playData.keyBoardList[1] = keyBoard[GameKeyType.Right];
        GameManager.instance.playData.keyBoardList[2] = keyBoard[GameKeyType.Up];
        GameManager.instance.playData.keyBoardList[3] = keyBoard[GameKeyType.Down];
        GameManager.instance.playData.keyBoardList[4] = keyBoard[GameKeyType.Jump];
        GameManager.instance.playData.keyBoardList[5] = keyBoard[GameKeyType.Attack];
        GameManager.instance.playData.keyBoardList[6] = keyBoard[GameKeyType.ItemGet];
        GameManager.instance.playData.keyBoardList[7] = keyBoard[GameKeyType.ItemTrash];
        GameManager.instance.playData.keyBoardList[8] = keyBoard[GameKeyType.ItemChange];
        GameManager.instance.playData.keyBoardList[9] = keyBoard[GameKeyType.ItemUse];
        GameManager.instance.playData.keyBoardList[10] = keyBoard[GameKeyType.Map];
    }

    public static string GetJoyStickKey()
    {
        if(nowController == GameController.XBOX)
        {
            for (KeyCode key = KeyCode.JoystickButton0; key != KeyCode.JoystickButton19; key++)
                if (Input.GetKey(key))
                {
                    switch (key)
                    {
                        case KeyCode.JoystickButton0:
                            return "A";
                        case KeyCode.JoystickButton1:
                            return "B";
                        case KeyCode.JoystickButton2:
                            return "X";
                        case KeyCode.JoystickButton3:
                            return "Y";
                        case KeyCode.JoystickButton4:
                            return "LB";
                        case KeyCode.JoystickButton5:
                            return "RB";
                        case KeyCode.JoystickButton6:
                            return "BACK";
                        case KeyCode.JoystickButton7:
                            return "START";
                    }
                }
            if (Input.GetAxisRaw("Right Trigger " + nowController.ToString()) != 0)
                return "RT";
            if (Input.GetAxisRaw("Left Trigger " + nowController.ToString()) != 0)
                return "LT";
            if (Input.GetAxisRaw("Horizontal Arrow " + nowController.ToString()) < 0)
                return "◀";
            if (Input.GetAxisRaw("Horizontal Arrow " + nowController.ToString()) > 0)
                return "▶";
            if (Input.GetAxisRaw("Vertical Arrow " + nowController.ToString()) > 0)
                return "▲";
            if (Input.GetAxisRaw("Vertical Arrow " + nowController.ToString()) < 0)
                return "▼";
            if (Input.GetAxisRaw("Horizontal") < 0)
                return "StickLeft";
            if (Input.GetAxisRaw("Horizontal") > 0)
                return "StickRight";
            if (Input.GetAxisRaw("Vertical") > 0)
                return "StickUp";
            if (Input.GetAxisRaw("Vertical") < 0)
                return "StickDown";
            return "?";
        }
        else if (nowController == GameController.Wireless)
        {
            for (KeyCode key = KeyCode.JoystickButton0; key != KeyCode.JoystickButton19; key++)
                if (Input.GetKey(key))
                {
                    switch (key)
                    {
                        case KeyCode.JoystickButton0:
                            return "□";
                        case KeyCode.JoystickButton1:
                            return "Ⅹ";
                        case KeyCode.JoystickButton2:
                            return "○";
                        case KeyCode.JoystickButton3:
                            return "△";
                        case KeyCode.JoystickButton4:
                            return "L1";
                        case KeyCode.JoystickButton5:
                            return "R1";
                        case KeyCode.JoystickButton6:
                            return "L2";
                        case KeyCode.JoystickButton7:
                            return "R2";
                        case KeyCode.JoystickButton8:
                            return "SHARE";
                        case KeyCode.JoystickButton9:
                            return "OPTIONS";
                    }
                }
            if (Input.GetAxisRaw("Horizontal Arrow " + nowController.ToString()) < 0)
                return "◀";
            if (Input.GetAxisRaw("Horizontal Arrow " + nowController.ToString()) > 0)
                return "▶";
            if (Input.GetAxisRaw("Vertical Arrow " + nowController.ToString()) > 0)
                return "▲";
            if (Input.GetAxisRaw("Vertical Arrow " + nowController.ToString()) < 0)
                return "▼";
            if (Input.GetAxisRaw("Horizontal") < 0)
                return "StickLeft";
            if (Input.GetAxisRaw("Horizontal") > 0)
                return "StickRight";
            if (Input.GetAxisRaw("Vertical") > 0)
                return "StickUp";
            if (Input.GetAxisRaw("Vertical") < 0)
                return "StickDown";
            return "?";
        }
        return "?";
    }

    public static bool CheckJoyStick()
    {
        for (KeyCode key = KeyCode.JoystickButton0; key != KeyCode.JoystickButton19; key++)
            if (Input.GetKey(key))
                return true;
        if (Input.GetAxisRaw("Horizontal") != 0)
            return true;
        if (Input.GetAxisRaw("Vertical") != 0)
            return true;

        string[] pad = Input.GetJoystickNames();
        bool xbox = false;
        bool ps = false;
        for (int i = 0; i < pad.Length; i++)
        {
            if (pad[i].Contains("XBOX"))
            {
                xbox = true;
                break;
            }
            else if (pad[i].Contains("Wireless"))
            {
                ps = true;
                break;
            }
        }

        if (xbox)
        {
            if (Input.GetAxisRaw("Right Trigger XBOX") != 0)
                return true;
            if (Input.GetAxisRaw("Left Trigger XBOX") != 0)
                return true;
            if (Input.GetAxisRaw("Horizontal Arrow XBOX") != 0)
                return true;
            if (Input.GetAxisRaw("Vertical Arrow XBOX") != 0)
                return true;
        }
        else if (ps)
        {
            if (Input.GetAxisRaw("Right Trigger Wireless") != 0)
                return true;
            if (Input.GetAxisRaw("Left Trigger Wireless") != 0)
                return true;
            if (Input.GetAxisRaw("Horizontal Arrow Wireless") != 0)
                return true;
            if (Input.GetAxisRaw("Vertical Arrow Wireless") != 0)
                return true;
        }
        return false;

    }

    public static bool GetKey(string key)
    {
        if(nowController == GameController.XBOX)
        {
            if (key == "A")
                if (Input.GetKey(KeyCode.JoystickButton0))
                    return true;
            if (key == "B")
                if (Input.GetKey(KeyCode.JoystickButton1))
                    return true;
            if (key == "X")
                if (Input.GetKey(KeyCode.JoystickButton2))
                    return true;
            if (key == "Y")
                if (Input.GetKey(KeyCode.JoystickButton3))
                    return true;
            if (key == "LB")
                if (Input.GetKey(KeyCode.JoystickButton4))
                    return true;
            if (key == "RB")
                if (Input.GetKey(KeyCode.JoystickButton5))
                    return true;
            if (key == "START")
                if (Input.GetKey(KeyCode.JoystickButton7))
                    return true;
            if (key == "RT")
                if (Input.GetAxisRaw("Right Trigger " + nowController.ToString()) != 0)
                    return true;
            if (key == "LT")
                if (Input.GetAxisRaw("Left Trigger " + nowController.ToString()) != 0)
                    return true;
        }
        else if (nowController == GameController.Wireless)
        {
            if (key == "□")
                if (Input.GetKey(KeyCode.JoystickButton0))
                    return true;
            if (key == "Ⅹ")
                if (Input.GetKey(KeyCode.JoystickButton1))
                    return true;
            if (key == "○")
                if (Input.GetKey(KeyCode.JoystickButton2))
                    return true;
            if (key == "△")
                if (Input.GetKey(KeyCode.JoystickButton3))
                    return true;
            if (key == "L1")
                if (Input.GetKey(KeyCode.JoystickButton4))
                    return true;
            if (key == "R1")
                if (Input.GetKey(KeyCode.JoystickButton5))
                    return true;
            if (key == "L2")
                if (Input.GetKey(KeyCode.JoystickButton6))
                    return true;
            if (key == "R2")
                if (Input.GetKey(KeyCode.JoystickButton7))
                    return true;
            if (key == "SHARE")
                if (Input.GetKey(KeyCode.JoystickButton8))
                    return true;
            if (key == "OPTIONS")
                if (Input.GetKey(KeyCode.JoystickButton9))
                    return true;
        }

        if (key == "◀")
            if (Input.GetAxisRaw("Horizontal Arrow " + nowController.ToString()) < 0)
                return true;
        if (key == "▶")
            if (Input.GetAxisRaw("Horizontal Arrow " + nowController.ToString()) > 0)
                return true;
        if (key == "▲")
            if (Input.GetAxisRaw("Vertical Arrow " + nowController.ToString()) < 0)
                return true;
        if (key == "▼")
            if (Input.GetAxisRaw("Vertical Arrow " + nowController.ToString()) > 0)
                return true;
        if (key == "StickLeft")
            if (Input.GetAxisRaw("Horizontal") < 0)
                return true;
        if (key == "StickRight")
            if (Input.GetAxisRaw("Horizontal") > 0)
                return true;
        if (key == "StickUp")
            if (Input.GetAxisRaw("Vertical") > 0)
                return true;
        if (key == "StickDown")
            if (Input.GetAxisRaw("Vertical") < 0)
                return true;
        return false;
    }

    public static bool GetKeyDown(string key)
    {
        if (nowController == GameController.XBOX)
        {
            if (key == "A")
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                    return true;
            if (key == "B")
                if (Input.GetKeyDown(KeyCode.JoystickButton1))
                    return true;
            if (key == "X")
                if (Input.GetKeyDown(KeyCode.JoystickButton2))
                    return true;
            if (key == "Y")
                if (Input.GetKeyDown(KeyCode.JoystickButton3))
                    return true;
            if (key == "LB")
                if (Input.GetKeyDown(KeyCode.JoystickButton4))
                    return true;
            if (key == "RB")
                if (Input.GetKeyDown(KeyCode.JoystickButton5))
                    return true;
            if (key == "START")
                if (Input.GetKeyDown(KeyCode.JoystickButton7))
                    return true;
            if (key == "RT")
                return rightTriggerDown;
            if (key == "LT")
                return leftTriggerDown;
        }
        else if(nowController == GameController.Wireless)
        {
            if (key == "□")
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                    return true;
            if (key == "Ⅹ")
                if (Input.GetKeyDown(KeyCode.JoystickButton1))
                    return true;
            if (key == "○")
                if (Input.GetKeyDown(KeyCode.JoystickButton2))
                    return true;
            if (key == "△")
                if (Input.GetKeyDown(KeyCode.JoystickButton3))
                    return true;
            if (key == "L1")
                if (Input.GetKeyDown(KeyCode.JoystickButton4))
                    return true;
            if (key == "R1")
                if (Input.GetKeyDown(KeyCode.JoystickButton5))
                    return true;
            if (key == "L2")
                if (Input.GetKeyDown(KeyCode.JoystickButton6))
                    return true;
            if (key == "R2")
                if (Input.GetKeyDown(KeyCode.JoystickButton7))
                    return true;
            if (key == "SHARE")
                if (Input.GetKeyDown(KeyCode.JoystickButton8))
                    return true;
            if (key == "OPTIONS")
                if (Input.GetKeyDown(KeyCode.JoystickButton9))
                    return true;
        }

        if (key == "◀")
            return leftArrowDown;
        if (key == "▶")
            return rightArrowDown;
        if (key == "▲")
            return upArrowDown;
        if (key == "▼")
            return downArrowDown;
        if (key == "StickLeft")
            return leftStickDown;
        if (key == "StickRight")
            return rightStickDown;
        if (key == "StickUp")
            return upStickDown;
        if (key == "StickDown")
            return downStickDown;
        return false;
    }

    public static bool GetKeyUp(string key)
    {
        if (nowController == GameController.XBOX)
        {
            if (key == "A")
                if (Input.GetKeyUp(KeyCode.JoystickButton0))
                    return true;
            if (key == "B")
                if (Input.GetKeyUp(KeyCode.JoystickButton1))
                    return true;
            if (key == "X")
                if (Input.GetKeyUp(KeyCode.JoystickButton2))
                    return true;
            if (key == "Y")
                if (Input.GetKeyUp(KeyCode.JoystickButton3))
                    return true;
            if (key == "LB")
                if (Input.GetKeyUp(KeyCode.JoystickButton4))
                    return true;
            if (key == "RB")
                if (Input.GetKeyUp(KeyCode.JoystickButton5))
                    return true;
            if (key == "START")
                if (Input.GetKeyUp(KeyCode.JoystickButton7))
                    return true;
            if (key == "RT")
                return rightTriggerUp;
            if (key == "LT")
                return leftTriggerUp;
        }
        else if(nowController == GameController.Wireless)
        {
            if (key == "□")
                if (Input.GetKeyUp(KeyCode.JoystickButton0))
                    return true;
            if (key == "Ⅹ")
                if (Input.GetKeyUp(KeyCode.JoystickButton1))
                    return true;
            if (key == "○")
                if (Input.GetKeyUp(KeyCode.JoystickButton2))
                    return true;
            if (key == "△")
                if (Input.GetKeyUp(KeyCode.JoystickButton3))
                    return true;
            if (key == "L1")
                if (Input.GetKeyUp(KeyCode.JoystickButton4))
                    return true;
            if (key == "R1")
                if (Input.GetKeyUp(KeyCode.JoystickButton5))
                    return true;
            if (key == "L2")
                if (Input.GetKeyUp(KeyCode.JoystickButton6))
                    return true;
            if (key == "R2")
                if (Input.GetKeyUp(KeyCode.JoystickButton7))
                    return true;
            if (key == "SHARE")
                if (Input.GetKeyUp(KeyCode.JoystickButton8))
                    return true;
            if (key == "OPTIONS")
                if (Input.GetKeyUp(KeyCode.JoystickButton9))
                    return true;
        }

        if (key == "◀")
            return leftArrowUp;
        if (key == "▶")
            return rightArrowUp;
        if (key == "▲")
            return upArrowUp;
        if (key == "▼")
            return downArrowUp;
        if (key == "StickLeft")
            return leftStickUp;
        if (key == "StickRight")
            return rightStickUp;
        if (key == "StickUp")
            return upStickUp;
        if (key == "StickDown")
            return downStickUp;
        return false;
    }

    public void OnGUI()
    {
        Event keyEvent = Event.current;
        if (nowController != GameController.KeyBoard && (keyEvent.isKey || keyEvent.isMouse))
        {
            nowController = GameController.KeyBoard;
            Debug.Log("키보드");
        }
        else if(nowController == GameController.KeyBoard/* && CheckJoyStick()*/)
        {
            string[] pad = Input.GetJoystickNames();
            for (int i = 0; i < pad.Length; i++)
            {
                if (pad[i].Contains("XBOX"))
                {
                    nowController = GameController.XBOX;
                    Debug.Log("XBOX");
                    break;
                }
                else if (pad[i].Contains("Wireless"))
                {
                    nowController = GameController.Wireless;
                    Debug.Log("PS");
                    break;
                }
            }
        }
    }

}
