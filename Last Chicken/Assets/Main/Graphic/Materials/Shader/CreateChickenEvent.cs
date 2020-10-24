using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CreateChickenEvent : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [SerializeField] private float glowBrightness = 2f;
    [SerializeField] private Color glowColor = Color.white;
    [SerializeField] [Range(0, 1)] private float size = 0;
    [SerializeField] [Range(0, 1)] private float alphaCutOff = 0;
    [SerializeField] private bool imgAlpha = false;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_InLightColor", glowColor * glowBrightness);
        mpb.SetFloat("_InLightSize", size);
        mpb.SetFloat("_AlphaCutOff", alphaCutOff);
        mpb.SetFloat("_ImgAlpha", imgAlpha ? 1 : 0);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}