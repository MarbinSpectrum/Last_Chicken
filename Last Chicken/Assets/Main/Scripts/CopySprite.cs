using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopySprite : MonoBehaviour
{
    [Header("복사할 대상")]
    public Image copyImage;
    public SpriteRenderer copySpriteRenderer;
    bool filp = false;

    [Header("복사될 대상")]
    public Image image;
    public SpriteRenderer spriteRenderer;


    #region[Update]
    void Update()
    {
        Sprite sprite = (copyImage != null) ? copyImage.sprite : (copySpriteRenderer != null) ? copySpriteRenderer.sprite : null;

        if (copySpriteRenderer)
            filp = copySpriteRenderer.flipX;

        if (!sprite)
            return;

        if (image)
            image.sprite = sprite;
        if (spriteRenderer)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.flipX = filp;
        }
    }
    #endregion
}
