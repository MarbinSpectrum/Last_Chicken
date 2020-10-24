using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarBackGroundImg : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite darkImg;
    public Material darkMat;
    public Sprite lightImg;
    public Material lightMat;
    public bool changeLayer = true;
    public bool changeSprite = true;
    public bool changeMaterial = true;

    public static List<AltarBackGroundImg> altarImgList = new List<AltarBackGroundImg>();

    public void ChangeImg(bool dark)
    {
        if(dark)
        {
            if (changeSprite)
                spriteRenderer.sprite = darkImg;
            if (changeMaterial)
                spriteRenderer.material = darkMat;
            if(changeLayer)
                gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            if (changeSprite)
                spriteRenderer.sprite = lightImg;
            if (changeMaterial)
                spriteRenderer.material = lightMat;
            if (changeLayer)
                gameObject.layer = LayerMask.NameToLayer("PostProcess");
        }
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
