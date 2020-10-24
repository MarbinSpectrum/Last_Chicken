using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum Language { English, 한국어 };

[System.Serializable]
public class PlayData
{
    public float SE_Volume;         //SE 크기
    public float BGM_Volume;        //BGM 크기
    public int ScreenWidth;         //화면 너비
    public int ScreenHeight;        //화면 높이
    public bool fullScreen;         //전체화면 여부
    public bool firstGame;          //처음게임을 실행했는가?
    public Language language;
    public List<string> gamePadListXBOX = new List<string>();
    public List<string> gamePadListPS = new List<string>();
    public List<KeyCode> keyBoardList = new List<KeyCode>();

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

    public bool[] monsterRecords = new bool[MonsterManager.monsterName.Length];
    public bool[] itemRecords = new bool[ItemManager.itemName.Length];

    public int seed = 0;

    public int ItemBagLevel()
    {
        int level = 0;
        for (int i = 3; i < 6; i++)
            if (slotAct[i])
                level++;
        return level;
    }

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

        gamePadListXBOX.Add("StickLeft");
        gamePadListXBOX.Add("StickRight");
        gamePadListXBOX.Add("StickUp");
        gamePadListXBOX.Add("StickDown");
        //gamePadListXBOX.Add("◀");
        //gamePadListXBOX.Add("▶");
        //gamePadListXBOX.Add("▲");
        //gamePadListXBOX.Add("▼");
        gamePadListXBOX.Add("A");
        gamePadListXBOX.Add("X");
        gamePadListXBOX.Add("Y");
        gamePadListXBOX.Add("LT");
        gamePadListXBOX.Add("RT");
        gamePadListXBOX.Add("B");
        gamePadListXBOX.Add("RB");

        gamePadListPS.Add("StickLeft");
        gamePadListPS.Add("StickRight");
        gamePadListPS.Add("StickUp");
        gamePadListPS.Add("StickDown");
        gamePadListPS.Add("Ⅹ");
        gamePadListPS.Add("□");
        gamePadListPS.Add("△");
        gamePadListPS.Add("L2");
        gamePadListPS.Add("R2");
        gamePadListPS.Add("○");
        gamePadListPS.Add("R1");

        keyBoardList.Add(KeyCode.A);
        keyBoardList.Add(KeyCode.D);
        keyBoardList.Add(KeyCode.W);
        keyBoardList.Add(KeyCode.S);
        keyBoardList.Add(KeyCode.Space);
        keyBoardList.Add(KeyCode.Mouse0);
        keyBoardList.Add(KeyCode.E);
        keyBoardList.Add(KeyCode.F);
        keyBoardList.Add(KeyCode.Q);
        keyBoardList.Add(KeyCode.Mouse1);
        keyBoardList.Add(KeyCode.M);

        for (int i = 0; i < MonsterManager.monsterName.Length; i++)
            monsterRecords[i] = false;

        for (int i = 0; i < ItemManager.itemName.Length; i++)
            itemRecords[i] = false;

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