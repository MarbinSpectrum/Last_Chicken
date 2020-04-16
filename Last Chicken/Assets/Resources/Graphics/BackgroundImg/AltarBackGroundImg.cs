using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarBackGroundImg : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite darkImg;
    public Sprite lightImg;

    public static List<AltarBackGroundImg> altarImgList = new List<AltarBackGroundImg>();

    public void ChangeImg(bool dark)
    {
        if(dark)
            spriteRenderer.sprite = darkImg;
        else
            spriteRenderer.sprite = lightImg;
    }

    public static void ChangeImgs(bool dark)
    {
        for (int i = 0; i < altarImgList.Count; i++)
            altarImgList[i].ChangeImg(dark);
    }

    public void Awake()
    {
        altarImgList.Add(this);
    }
}
