using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetFountainMat : MonoBehaviour
{
    public SpriteRenderer sprite;

    public Material BackFlower;
    [ColorUsage(true, true)]
    public Color BackFlowerTint;
    public Material ChickenFountain;
    [ColorUsage(true, true)]
    public Color ChickenFountainTint;

    public Material ChickenFountainBelow;
    [ColorUsage(true, true)]
    public Color ChickenFountainBelowTint;

    public Material FountainStone;
    [ColorUsage(true,true)]
    public Color FountainStoneTint;

    [ColorUsage(true, true)]
    public Color EndColor;

    [Space(30)]
    public Material backMat;
    [Range(0,1)]
    public float backGrayPow;
    [ColorUsage(true, true)]
    public Color FountainBackTint;

    void Update()
    {
        float v = 1 - sprite.color.a;
        BackFlower.SetFloat("_Power", v);
        BackFlower.SetColor("_Color", Color.Lerp(BackFlowerTint, EndColor, v));
        ChickenFountain.SetFloat("_Power", v);
        ChickenFountain.SetColor("_Color", Color.Lerp(ChickenFountainTint, EndColor, v));
        ChickenFountainBelow.SetFloat("_Power", v);
        ChickenFountainBelow.SetColor("_Color", Color.Lerp(ChickenFountainBelowTint, EndColor, v));
        FountainStone.SetFloat("_Power", v);
        FountainStone.SetColor("_Color", Color.Lerp(FountainStoneTint, EndColor, v));
        if(Player.instance)
            Player.instance.inShadow = sprite.color.a/2f;

        backMat.SetFloat("_Power", backGrayPow);
        backMat.SetColor("_Color", Color.Lerp(FountainBackTint, EndColor, backGrayPow));
    }

    private void OnDisable()
    {
        if (Player.instance)
            Player.instance.inShadow = 0;
    }
}
