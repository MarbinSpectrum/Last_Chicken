using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PlayData
{
    public enum Language { English, 한국어 };

    public float SE_Volume;         //SE 크기
    public float BGM_Volume;        //BGM 크기
    public int ScreenWidth;         //화면 너비
    public int ScreenHeight;        //화면 높이
    public bool fullScreen;         //전체화면 여부
    public bool firstGame;          //처음게임을 실행했는가?
    public Language language;

    ////////////////////////////////////////////////////////////////////////////

    public string stageName;        //진행스테이지

    ///////////////////////////////////////////////////////////////////

    public float playerNowHp;      //플레이어 현재 체력
    public float playerMaxHp;   //플레이어 최대 체력

    ///////////////////////////////////////////////////////////////////

    public string[] itemSlot = new string[6];    
    public int[] itemNum = new int[6];
    public bool[] slotAct = new bool[6];

    ///////////////////////////////////////////////////////////////////

    public int playerMoney;   //플레이어 금화

    ///////////////////////////////////////////////////////////////////

    public bool shopVIP;
    public int randomDice;

    ///////////////////////////////////////////////////////////////////

    public int pickLevel = 0;

    ///////////////////////////////////////////////////////////////////

    public bool[] playerBuffItemHas = new bool[BuffManager.buffName.Length];
    public int[] playerBuffItemNum = new int[BuffManager.buffName.Length];
    public float[] playerBuffItemTime = new float[BuffManager.buffName.Length];

    ///////////////////////////////////////////////////////////////////

    public int seed = 0;

    //초기화
    public PlayData()
    {
        SE_Volume = 0.5f;
        BGM_Volume = 0.5f;
        ScreenWidth = 1600;
        ScreenHeight = 900;
        fullScreen = false;
        firstGame = true;
        language = Language.한국어;

        ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////

        stageName = "Stage0101";

        ///////////////////////////////////////////////////////////////////

        playerMaxHp = 3;
        playerNowHp = 3;

        ///////////////////////////////////////////////////////////////////

        for(int i = 0; i < 6; i++)
        {
            slotAct[i] = false;
            itemSlot[i] = "";
            itemNum[i] = 0;
        }

        ///////////////////////////////////////////////////////////////////

        slotAct[0] = true;
        slotAct[1] = true;
        slotAct[2] = true;
        itemSlot[0] = ItemManager.itemName[0];
        itemSlot[1] = ItemManager.itemName[5];

        ///////////////////////////////////////////////////////////////////

        playerMoney = 0;

        ///////////////////////////////////////////////////////////////////

        shopVIP = false;
        randomDice = 100;

        ///////////////////////////////////////////////////////////////////

        pickLevel = 0;

        ///////////////////////////////////////////////////////////////////

        for (int i = 0; i < BuffManager.buffName.Length; i++)
        {
            playerBuffItemHas[i] = false;
            playerBuffItemNum[i] = 0;
            playerBuffItemTime[i] = 0;
        }

        seed = Random.Range(0, 10000);
    }

}