using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PlayData
{
    public float SE_Volume;         //SE 크기
    public float BGM_Volume;        //BGM 크기
    public int ScreenWidth;         //화면 너비
    public int ScreenHeight;        //화면 높이
    public bool fullScreen;         //전체화면 여부
    public bool firstGame;          //처음게임을 실행했는가?

    ////////////////////////////////////////////////////////////////////////////

    public string stageName;        //진행스테이지

    ///////////////////////////////////////////////////////////////////

    public float playerNowHp;      //플레이어 현재 체력
    public float playerMaxHp;   //플레이어 최대 체력

    ///////////////////////////////////////////////////////////////////

    public string playerActiveItem;   //플레이어 액티브
    public int playerActiveItemNum;

    ///////////////////////////////////////////////////////////////////

    public string[] playerPassiveItem = new string[5];    //플레이어 서브 아이템
    public int[] playerPassiveItemNum = new int[5];
    public bool[] playerPassiveSlotAct = new bool[5];

    public int playerPassivePointer;

    ///////////////////////////////////////////////////////////////////

    public int playerMoney;   //플레이어 금화

    ///////////////////////////////////////////////////////////////////

    public bool shopVIP;
    public int randomDice;

    ///////////////////////////////////////////////////////////////////

    public bool[] playerBuffItemHas = new bool[BuffManager.buffName.Length];
    public int[] playerBuffItemNum = new int[BuffManager.buffName.Length];
    public float[] playerBuffItemTime = new float[BuffManager.buffName.Length];

    //초기화
    public PlayData()
    {
        SE_Volume = 0.5f;
        BGM_Volume = 0.5f;
        ScreenWidth = 1920;
        ScreenHeight = 1080;
        fullScreen = true;
        firstGame = true;

        ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////
        
        stageName = "Stage0101";

        ///////////////////////////////////////////////////////////////////

        playerMaxHp = 3;
        playerNowHp = 3;

        ///////////////////////////////////////////////////////////////////

        playerActiveItem = ItemManager.itemName[0];
        playerActiveItemNum = 0;

        for(int i = 0; i < 5; i++)
        {
            playerPassiveSlotAct[i] = false;
            playerPassiveItem[i] = "";
            playerPassiveItemNum[i] = 0;
        }

        ///////////////////////////////////////////////////////////////////

        playerPassiveSlotAct[0] = true;
        playerPassiveSlotAct[1] = true;
        playerPassiveItem[0] = ItemManager.itemName[4];
        playerPassiveItem[1] = ItemManager.itemName[5];

        playerPassivePointer = 0;

        ///////////////////////////////////////////////////////////////////

        playerMoney = 0;

        ///////////////////////////////////////////////////////////////////

        shopVIP = false;
        randomDice = 100;

        ///////////////////////////////////////////////////////////////////

        for (int i = 0; i < BuffManager.buffName.Length; i++)
        {
            playerBuffItemHas[i] = false;
            playerBuffItemNum[i] = 0;
            playerBuffItemTime[i] = 0;
        }
    }

}