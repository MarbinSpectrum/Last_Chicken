using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameController
{
    KeyBoard,GamePad
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
        instance = this;
        keyBoard[GameKeyType.Left] = KeyCode.A;
        keyBoard[GameKeyType.Right] = KeyCode.D;
        keyBoard[GameKeyType.Up] = KeyCode.W;
        keyBoard[GameKeyType.Down] = KeyCode.S;
        keyBoard[GameKeyType.Jump] = KeyCode.Space;
        keyBoard[GameKeyType.Attack] = KeyCode.Mouse0;
        keyBoard[GameKeyType.ItemGet] = KeyCode.E;
        keyBoard[GameKeyType.ItemTrash] = KeyCode.F;
        keyBoard[GameKeyType.ItemChange] = KeyCode.Q;
        keyBoard[GameKeyType.ItemUse] = KeyCode.Mouse1;
        keyBoard[GameKeyType.Map] = KeyCode.M;

        gamePad[GameKeyType.Left] = "StickLeft";
        gamePad[GameKeyType.Right] = "StickRight";
        gamePad[GameKeyType.Up] = "StickUp";
        gamePad[GameKeyType.Down] = "StickDown";
        gamePad[GameKeyType.Jump] = "A";
        gamePad[GameKeyType.Attack] = "X";
        gamePad[GameKeyType.ItemGet] = "Y";
        gamePad[GameKeyType.ItemTrash] = "LT";
        gamePad[GameKeyType.ItemChange] = "RT";
        gamePad[GameKeyType.ItemUse] = "B";
        gamePad[GameKeyType.Map] = "START";
    }

    public void Update()
    {
      // Debug.Log(Input.GetAxisRaw("Horizontal") + "," + Input.GetAxisRaw("Vertical"));
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
        }

        #region[Left & Right Trigger]
        if (rightTriggerDown)
            rightTriggerDown = false;
        if (rightTriggerUp)
            rightTriggerUp = false;
        if (_lastRightTrigger != Input.GetAxisRaw("Right Trigger"))
        {
            if (_lastRightTrigger < 1f && Input.GetAxisRaw("Right Trigger") == 1)
                rightTriggerDown = true;
            if (_lastRightTrigger > 0f && Input.GetAxisRaw("Right Trigger") == 0)
                rightTriggerUp = true;
            _lastRightTrigger = Input.GetAxisRaw("Right Trigger");
        }
        if (leftTriggerDown)
            leftTriggerDown = false;
        if (leftTriggerUp)
            leftTriggerUp = false;
        if (_lastLeftTrigger != Input.GetAxisRaw("Left Trigger"))
        {
            if (_lastLeftTrigger < 1f && Input.GetAxisRaw("Left Trigger") == 1)
                leftTriggerDown = true;
            if (_lastLeftTrigger > 0f && Input.GetAxisRaw("Left Trigger") == 0)
                leftTriggerUp = true;
            _lastLeftTrigger = Input.GetAxisRaw("Left Trigger");
        }
        #endregion

        #region[Left & Right & Up & Down Arrow]
        if (leftArrowDown)
            leftArrowDown = false;
        if (leftArrowUp)
            leftArrowUp = false;
        if (_lastLeftArrow != Input.GetAxisRaw("Horizontal Arrow"))
        {
            if (_lastLeftArrow > -1f && Input.GetAxisRaw("Horizontal Arrow") == -1)
                leftArrowDown = true;
            if (_lastLeftArrow < 0f && Input.GetAxisRaw("Horizontal Arrow") == 0)
                leftArrowUp = true;
            _lastLeftArrow = Input.GetAxisRaw("Horizontal Arrow");
        }
        if (rightArrowDown)
            rightArrowDown = false;
        if (rightArrowUp)
            rightArrowUp = false;
        if (_lastRightArrow != Input.GetAxisRaw("Horizontal Arrow"))
        {
            if (_lastRightArrow < 1f && Input.GetAxisRaw("Horizontal Arrow") == 1)
                rightArrowDown = true;
            if (_lastRightArrow > 0f && Input.GetAxisRaw("Horizontal Arrow") == 0)
                rightArrowUp = true;
            _lastRightArrow = Input.GetAxisRaw("Horizontal Arrow");
        }
        if (downArrowDown)
            downArrowDown = false;
        if (downArrowUp)
            downArrowUp = false;
        if (_lastDownArrow != Input.GetAxisRaw("Vertical Arrow"))
        {
            if (_lastDownArrow > -1f && Input.GetAxisRaw("Vertical Arrow") == -1)
               downArrowDown = true;
            if (_lastDownArrow < 0f && Input.GetAxisRaw("Vertical Arrow") == 0)
                downArrowUp = true;
            _lastDownArrow = Input.GetAxisRaw("Vertical Arrow");
        }
        if (upArrowDown)
            upArrowDown = false;
        if (upArrowUp)
            upArrowUp = false;
        if (_lastUpArrow != Input.GetAxisRaw("Vertical Arrow"))
        {
            if (_lastUpArrow < 1f && Input.GetAxisRaw("Vertical Arrow") == 1)
                upArrowDown = true;
            if (_lastUpArrow > 0f && Input.GetAxisRaw("Vertical Arrow") == 0)
                upArrowUp = true;
            _lastUpArrow = Input.GetAxisRaw("Vertical Arrow");
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

    public static KeyCode GetJoyStickKeyCode()
    {
        for (KeyCode key = KeyCode.JoystickButton0; key != KeyCode.JoystickButton19; key++)
            if (Input.GetKey(key))
                return key;
        return KeyCode.None;
    }

    public static string GetJoyStickKey()
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
                    case KeyCode.JoystickButton7:
                        return "START";
                }
            }
        if (Input.GetAxisRaw("Right Trigger") != 0)
            return "RT";
        if (Input.GetAxisRaw("Left Trigger") > 0)
            return "LT";
        if (Input.GetAxisRaw("Horizontal Arrow") < 0)
            return "◀";
        if (Input.GetAxisRaw("Horizontal Arrow") > 0)
            return "▶";
        if (Input.GetAxisRaw("Vertical Arrow") > 0)
            return "▲";
        if (Input.GetAxisRaw("Vertical Arrow") < 0)
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

    public static bool CheckJoyStick()
    {
        for (KeyCode key = KeyCode.JoystickButton0; key != KeyCode.JoystickButton19; key++)
            if (Input.GetKey(key))
                return true;
        if (Input.GetAxisRaw("Horizontal") != 0)
            return true;
        if (Input.GetAxisRaw("Vertical") != 0)
            return true;
        if (Input.GetAxisRaw("Right Trigger") != 0)
            return true;
        if (Input.GetAxisRaw("Horizontal Arrow") != 0)
            return true;
        if (Input.GetAxisRaw("Vertical Arrow") != 0)
            return true;
        return false;

    }

    public static bool GetKey(string key)
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
            if (Input.GetAxisRaw("Right Trigger") != 0)
                return true;
        if (key == "LT")
            if (Input.GetAxisRaw("Left Trigger") != 0)
                return true;
        if (key == "◀")
            if (Input.GetAxisRaw("Horizontal Arrow") < 0)
                return true;
        if (key == "▶")
            if (Input.GetAxisRaw("Horizontal Arrow") > 0)
                return true;
        if (key == "▲")
            if (Input.GetAxisRaw("Vertical Arrow") < 0)
                return true;
        if (key == "▼")
            if (Input.GetAxisRaw("Vertical Arrow") > 0)
                return true;
        if (key == "◀")
            if (Input.GetAxisRaw("Horizontal Arrow") < 0)
                return true;
        if (key == "▶")
            if (Input.GetAxisRaw("Horizontal Arrow") > 0)
                return true;
        if (key == "▲")
            if (Input.GetAxisRaw("Vertical Arrow") < 0)
                return true;
        if (key == "▼")
            if (Input.GetAxisRaw("Vertical Arrow") > 0)
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
        if (nowController == GameController.GamePad && (keyEvent.isKey || keyEvent.isMouse))
        {
            nowController = GameController.KeyBoard;
            Debug.Log("키보드");
        }
        else if(nowController == GameController.KeyBoard && CheckJoyStick())
        {
            nowController = GameController.GamePad;
            Debug.Log("게임패드");

        }
    }

}
