using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Custom;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    public static string[] buffName = new string[] { "Power", "Shield", "Speed", "AttackSpeed", "Luminous"};

    [System.Serializable]
    public class BuffStats
    {
        public Sprite BuffImg;
        public Color buffColor;
        public float buffGlow;
        public string buffName;
        public string buffName_Eng;
        public string buffExplain;
        public string buffExplain_Eng;
        public int value = 0;
        public bool Overlap;
        public float time = 0;
        public bool limitTime = false;
    }

    public static int FindData(string s)
    {
        for (int i = 0; i < buffName.Length; i++)
            if (buffName[i].Equals(s))
                return i;
        return 0;
    }

    public BuffStats[] buffData = new BuffStats[buffName.Length];

    public class BuffInfo
    {
        public bool hasBuff;
        public float time = 0;
        public int hasNum = 0;
    }
    public Dictionary<string, BuffInfo> nowBuffList = new Dictionary<string, BuffInfo>();

    public static bool loadEnd = false;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[Awake]
    public void Awake()
    {
        if(instance == null)
        {
            for(int i = 0; i < buffName.Length; i++)
                nowBuffList.Add(buffName[i], new BuffInfo());
            instance = this;
        }
    }
    #endregion

    #region[Update]
    void Update()
    {
        BuffUpdate();
    }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    #region[버프추가]
    public void AddBuff(int buffIndex)
    {
        if(buffData[buffIndex].Overlap)
            nowBuffList[buffName[buffIndex]].hasNum++;
        else
            nowBuffList[buffName[buffIndex]].hasNum = 1;

        if (buffData[buffIndex].Overlap)
            nowBuffList[buffName[buffIndex]].time += buffData[buffIndex].time;
        else
            nowBuffList[buffName[buffIndex]].time = buffData[buffIndex].time;

        if (buffName[buffIndex].Equals("Shield"))
            Player.instance.shieldFlag = false;
    }
    #endregion

    #region[버프 업데이트]
    void BuffUpdate()
    {
        BuffLoad();
        for (int i = 0; i < buffName.Length; i++)
        {
            if(buffData[i].limitTime)
            {
                if(nowBuffList[buffName[i]].time > 0)
                    nowBuffList[buffName[i]].time -= Time.deltaTime;
                else
                {
                    nowBuffList[buffName[i]].time = 0;
                    nowBuffList[buffName[i]].hasNum = 0;
                }
            }

            if (nowBuffList[buffName[i]].hasNum <= 0)
            {
                nowBuffList[buffName[i]].hasNum = 0;
                nowBuffList[buffName[i]].hasBuff = false;
            }
            else
                nowBuffList[buffName[i]].hasBuff = true;
        }
    }
    #endregion

    #region[버프 로드]
    public void BuffLoad()
    {
        if (!loadEnd)
            loadEnd = !loadEnd;
        else
            return;

        for (int i = 0; i < buffName.Length; i++)
        {
            instance.nowBuffList[buffName[i]].hasBuff = GameManager.instance.playData.playerBuffItemHas[i];
            instance.nowBuffList[buffName[i]].hasNum = GameManager.instance.playData.playerBuffItemNum[i];
            instance.nowBuffList[buffName[i]].time = GameManager.instance.playData.playerBuffItemTime[i];
        }
    }
    #endregion

    #region[버프 다지움]
    public void BuffRemove()
    {
        for (int i = 0; i < buffName.Length; i++)
        {
            GameManager.instance.playData.playerBuffItemHas[i] = false;
            GameManager.instance.playData.playerBuffItemNum[i] = 0;
            GameManager.instance.playData.playerBuffItemNum[i] = 0;
        }
    }
    #endregion

}
