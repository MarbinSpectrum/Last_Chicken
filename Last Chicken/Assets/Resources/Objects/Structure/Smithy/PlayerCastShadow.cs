using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastShadow : MonoBehaviour
{
    public CopySprite playerShadowCopySprite;
    public CopySprite chickenHeadShadowCopySprite;
    public GameObject lightPos;

    public float lightRange;
    [Range(0, 1)]
    public float maxlightAlpha;

    public float shadowRange;
    public float maxShadowRange;
    public void Update()
    {
        if(Player.instance)
        {
            playerShadowCopySprite.copySpriteRenderer = Player.instance.spriteRenderer;
            playerShadowCopySprite.enabled = true;
            playerShadowCopySprite.transform.position = Player.instance.transform.position;

            chickenHeadShadowCopySprite.copySpriteRenderer = Player.instance.chickenHeadSpriteRenderer;
            chickenHeadShadowCopySprite.enabled = true;
            chickenHeadShadowCopySprite.transform.position = Player.instance.chickenHeadSpriteRenderer.transform.position;
            chickenHeadShadowCopySprite.gameObject.SetActive(Player.instance.getChicken);

            float a = Mathf.Max(0,lightRange - Mathf.Abs(lightPos.transform.position.x - Player.instance.transform.position.x)) / lightRange;
            a = Mathf.Min(maxlightAlpha, a);
            playerShadowCopySprite.spriteRenderer.color = new Color(playerShadowCopySprite.spriteRenderer.color.r, playerShadowCopySprite.spriteRenderer.color.g, playerShadowCopySprite.spriteRenderer.color.b,a);
            chickenHeadShadowCopySprite.spriteRenderer.color = new Color(chickenHeadShadowCopySprite.spriteRenderer.color.r, chickenHeadShadowCopySprite.spriteRenderer.color.g, chickenHeadShadowCopySprite.spriteRenderer.color.b, a);

            float offsetX = Mathf.Min(maxShadowRange, shadowRange * Mathf.Abs(lightPos.transform.position.x - Player.instance.transform.position.x)) * (Player.instance.transform.position.x < lightPos.transform.position.x ? -1 : 1);
            playerShadowCopySprite.transform.position += new Vector3(offsetX, 0, playerShadowCopySprite.transform.position.z - 1);
            playerShadowCopySprite.transform.localScale = new Vector3(Player.instance.transform.localScale.x, 1, 1);
            chickenHeadShadowCopySprite.transform.position += new Vector3(offsetX, 0, chickenHeadShadowCopySprite.transform.position.z - 1);
            chickenHeadShadowCopySprite.transform.localScale = new Vector3(Player.instance.transform.localScale.x, 1, 1);

        }
    }

}
