using UnityEngine;
using Custom;
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public AudioSource SE;
    [System.NonSerialized] public AudioSource StopSE;
    [System.NonSerialized] public AudioSource BGM;
    [System.NonSerialized] public AudioSource SubBGM;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip attackIce;
    AudioClip[] attackDirt = new AudioClip[5];
    AudioClip attackStone;
    AudioClip attackIron;
    AudioClip attackGold;
    AudioClip attackMithril;
    AudioClip attackDiamond;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip playerAttack;
    AudioClip playerDamage;
    AudioClip playerRunDirt;
    AudioClip playerRunStone;
    AudioClip playerGlup;
    AudioClip playerBell;
    AudioClip playerGun;
    AudioClip playerJump;
    AudioClip playerSplash;
    AudioClip playerSplashSmall;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip[] chickenBark = new AudioClip[11];
    AudioClip chickenCoco;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip monsterDamage;
    AudioClip monsterDead;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip woodObjectAttack;
    AudioClip explosion;
    AudioClip ignite;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip ticking;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip btnClick;
    AudioClip selectMenu;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip itemGet;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    AudioClip title;
    AudioClip altar;
    AudioClip stage1;
    AudioClip tutorial;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[Awake]
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            SE = transform.Find("SE").GetComponent<AudioSource>();
            BGM = transform.Find("BGM").GetComponent<AudioSource>();

            SE.volume = GameManager.instance.playData.SE_Volume;
            BGM.volume = GameManager.instance.playData.BGM_Volume;

            StopSE = transform.Find("StopSE").GetComponent<AudioSource>();
            SubBGM = transform.Find("SubBGM").GetComponent<AudioSource>();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            attackIce = Resources.Load("Sounds/SE/Mineral/얼음") as AudioClip;
            for (int i = 0; i < 5; i++)
                attackDirt[i] = Resources.Load("Sounds/SE/Mineral/Dirt/흙" + (i + 1)) as AudioClip;
            attackStone = Resources.Load("Sounds/SE/Mineral/화강암,돌") as AudioClip;
            attackIron = Resources.Load("Sounds/SE/Mineral/구리,철") as AudioClip;
            attackGold = Resources.Load("Sounds/SE/Mineral/은,금,코발트") as AudioClip;
            attackMithril = Resources.Load("Sounds/SE/Mineral/미스릴,티타늄") as AudioClip;
            attackDiamond = Resources.Load("Sounds/SE/Mineral/자철석,다이아몬드") as AudioClip;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            playerAttack = Resources.Load("Sounds/SE/Player/플레이어공격") as AudioClip;
            playerDamage = Resources.Load("Sounds/SE/Player/플레이어피격") as AudioClip;
            playerRunDirt = Resources.Load("Sounds/SE/Player/흙에서뛰기") as AudioClip;
            playerRunStone = Resources.Load("Sounds/SE/Player/돌에서뛰기") as AudioClip;
            playerGlup = Resources.Load("Sounds/SE/Player/꿀꺽") as AudioClip;
            playerBell = Resources.Load("Sounds/SE/Player/방울소리") as AudioClip;
            playerGun = Resources.Load("Sounds/SE/Player/총소리") as AudioClip;
            playerJump = Resources.Load("Sounds/SE/Player/점프소리") as AudioClip;
            playerSplash = Resources.Load("Sounds/SE/Player/첨벙") as AudioClip;
            playerSplashSmall = Resources.Load("Sounds/SE/Player/첨벙작은") as AudioClip;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            for (int i = 0; i < 11; i++)
                chickenBark[i] = Resources.Load("Sounds/SE/Chicken/닭" + (i + 1)) as AudioClip;
            chickenCoco = Resources.Load("Sounds/SE/Chicken/꼬끼오") as AudioClip;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            monsterDamage = Resources.Load("Sounds/SE/Monster/몬스터피격") as AudioClip;
            monsterDead = Resources.Load("Sounds/SE/Monster/몬스터소멸") as AudioClip;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ticking = Resources.Load("Sounds/SE/UI/시계소리") as AudioClip;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            woodObjectAttack = Resources.Load("Sounds/SE/Object/나무오브젝트타격") as AudioClip;
            explosion = Resources.Load("Sounds/SE/Object/폭탄폭발") as AudioClip;
            ignite = Resources.Load("Sounds/SE/Object/불꽃점화") as AudioClip;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            btnClick = Resources.Load("Sounds/SE/UI/버튼클릭") as AudioClip;
            selectMenu = Resources.Load("Sounds/SE/UI/메뉴선택") as AudioClip;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            itemGet = Resources.Load("Sounds/SE/Object/아이템획득") as AudioClip;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            title = Resources.Load("Sounds/BGM/타이틀") as AudioClip;
            altar = Resources.Load("Sounds/BGM/제단") as AudioClip;
            stage1 = Resources.Load("Sounds/BGM/스테이지1") as AudioClip;
            tutorial = Resources.Load("Sounds/BGM/튜토리얼") as AudioClip;
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[효과음 멈추기]
    public void StopSE_Sound()
    {
        if (StopSE && StopSE.clip != null)
            StopSE.Stop();
    }
    #endregion

    #region[배경음 멈추기]
    public void StopBGM_Sound(bool sub = false)
    {
        if(!sub)
        {
            if (BGM)
                BGM.Stop();
        }
        else
        {
            if (SubBGM)
                SubBGM.Stop();
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[얼음 채광]
    public void AttackIce(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(attackIce);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(attackIce);
        }
    }
    #endregion

    #region[흙 채광]
    public void AttackDirt(int n,bool canStop = false)
    {
        if (!Exception.IndexOutRange(n, attackDirt))
            return;

        if (!canStop)
            SE.PlayOneShot(attackDirt[n]);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(attackDirt[n]);
        }
    }

    public void AttackDirt(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(attackDirt[Random.Range(0, attackDirt.Length)]);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(attackDirt[Random.Range(0, attackDirt.Length)]);
        }
    }
    #endregion

    #region[돌 채광]
    public void AttackStone(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(attackStone);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(attackStone);
        }
    }
    #endregion

    #region[철 채광]
    public void AttackIron(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(attackIron);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(attackIron);
        }
    }
    #endregion

    #region[금 채광]
    public void AttackGold(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(attackGold);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(attackGold);
        }
    }
    #endregion

    #region[미스릴 채광]
    public void AttackMithril(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(attackMithril);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(attackMithril);
        }
    }
    #endregion

    #region[다이아 채광]
    public void AttackDiamond(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(attackDiamond);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(attackDiamond);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[플레이어 공격]
    public void PlayerAttack(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerAttack);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerAttack);
        }
    }
    #endregion

    #region[플레이어 피격]
    public void PlayerDamage(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerDamage);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerDamage);
        }
    }
    #endregion

    #region[플레이어 흙에서 걷기]
    public void PlayerRunDirt(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerRunDirt);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerRunDirt);
        }
    }
    #endregion

    #region[플레이어 돌에서 걷기]
    public void PlayerRunStone(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerRunStone);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerRunStone);
        }
    }
    #endregion

    #region[플레이어 꿀꺽]
    public void PlayerGlup(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerGlup);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerGlup);
        }
    }
    #endregion

    #region[플레이어 방울]
    public void PlayerBell(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerBell);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerBell);
        }
    }
    #endregion

    #region[플레이어 방울]
    public void PlayerGun(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerGun);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerGun);
        }
    }
    #endregion

    #region[플레이어 점프]
    public void PlayerJump(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerJump);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerJump);
        }
    }
    #endregion

    #region[플레이어 첨벙]
    public void PlayerSplash(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerSplash);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerSplash);
        }
    }
    #endregion

    #region[플레이어 첨벙 작은]
    public void PlayerSplashSmall(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(playerSplashSmall);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(playerSplashSmall);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[닭 울음소리]
    public void ChickenBark(int n = 0,bool canStop = false)
    {
        if (!Exception.IndexOutRange(n, chickenBark))
            return;

        if (!canStop)
            SE.PlayOneShot(chickenBark[n]);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(chickenBark[n]);
        }
    }

    public void ChickenBark(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(chickenBark[Random.Range(0, chickenBark.Length)]);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(chickenBark[Random.Range(0, chickenBark.Length)]);
        }
    }
    #endregion

    #region[닭 꼬끼오]
    public void ChickenCoco(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(chickenCoco);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(chickenCoco);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[몬스터 피격]
    public void MonsterDamage(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(monsterDamage);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(monsterDamage);
        }
    }
    #endregion

    #region[몬스터 소멸]
    public void MonsterDead(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(monsterDead);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(monsterDead);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[나무오브젝트 타격]
    public void WoodObjectAttack(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(woodObjectAttack);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(woodObjectAttack);
        }
    }
    #endregion

    #region[폭탄폭발]
    public void Explosion(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(explosion);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(explosion);
        }
    }
    #endregion

    #region[불꽃점화]
    public void Ignite(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(ignite);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(ignite);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[시계소리]
    public void Ticking(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(ticking);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(ticking);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[버튼 클릭]
    public void BtnClick(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(btnClick);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(btnClick);
        }
    }
    #endregion

    #region[메뉴 선택]
    public void SelectMenu(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(selectMenu);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(selectMenu);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    #region[아이템획득]
    public void ItemGet(bool canStop = false)
    {
        if (!canStop)
            SE.PlayOneShot(itemGet);
        else
        {
            StopSE.volume = SE.volume;
            StopSE.PlayOneShot(itemGet);
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region[타이틀]
    public void Title(bool sub = false)
    {
        if(!sub)
        {
            SubBGM.Pause();
            BGM.clip = title;
            BGM.Play();
        }
        else
        {
            BGM.Pause();
            SubBGM.volume = BGM.volume;
            SubBGM.clip = title;
            SubBGM.Play();
        }
    }
    #endregion

    #region[제단]
    public void Altar(bool sub = false)
    {
        if (!sub)
        {
            SubBGM.Pause();
            BGM.clip = altar;
            BGM.Play();
        }
        else
        {
            BGM.Pause();
            SubBGM.volume = BGM.volume;
            SubBGM.clip = altar;
            SubBGM.Play();
        }
    }
    #endregion

    #region[튜토리얼]
    public void Tutorial(bool sub = false)
    {
        if (!sub)
        {
            SubBGM.Pause();
            BGM.clip = tutorial;
            BGM.Play();
        }
        else
        {
            BGM.Pause();
            SubBGM.volume = BGM.volume;
            SubBGM.clip = tutorial;
            SubBGM.Play();
        }
    }
    #endregion

    #region[스테이지1]
    public void Stage1(bool sub = false)
    {
        if (!sub)
        {
            SubBGM.Pause();
            BGM.clip = stage1;
            BGM.Play();
        }
        else
        {
            BGM.Pause();
            SubBGM.volume = BGM.volume;
            SubBGM.clip = stage1;
            SubBGM.Play();
        }
    }
    #endregion

}