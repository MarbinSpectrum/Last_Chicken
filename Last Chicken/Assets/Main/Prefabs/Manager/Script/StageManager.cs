using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Sprite tutorial_BackGround;
    public string tutorial_Name;
    public string tutorial_Name_Eng;

    public Sprite stage0101_BackGround;
    public string stage0101_Name;
    public string stage0101_Name_Eng;
    public MonsterManager.SpawnMonster stage0101_Monsters = new MonsterManager.SpawnMonster();
    public int stage0101_ObjectValue;
    public int stage0101_WoodBoxValue;
    public int stage0101_TrapValue;

    public Sprite stage0102_BackGround;
    public string stage0102_Name;
    public string stage0102_Name_Eng;
    public MonsterManager.SpawnMonster stage0102_Monsters = new MonsterManager.SpawnMonster();
    public int stage0102_ObjectValue;
    public int stage0102_WoodBoxValue;
    public int stage0102_TrapValue;

    public Sprite stage0103_BackGround;
    public string stage0103_Name;
    public string stage0103_Name_Eng;
    public MonsterManager.SpawnMonster stage0103_Monsters = new MonsterManager.SpawnMonster();
    public int stage0103_ObjectValue;
    public int stage0103_WoodBoxValue;
    public int stage0103_TrapValue;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public Sprite stage0201_BackGround;
    public string stage0201_Name;
    public string stage0201_Name_Eng;
    public MonsterManager.SpawnMonster stage0201_Monsters = new MonsterManager.SpawnMonster();
    public int stage0201_ObjectValue;
    public int stage0201_WoodBoxValue;
    public int stage0201_TrapValue;


    public Sprite Igloo_BackGround;
    public string Igloo_Name;
    public string Igloo_Name_Eng;
    public MonsterManager.SpawnMonster Igloo_Monsters = new MonsterManager.SpawnMonster();
    public int Igloo_ObjectValue;
    public int Igloo_WoodBoxValue;
    public int Igloo_TrapValue;


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    public void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion
}