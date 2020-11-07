using UnityEngine;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.black;

    [Range(0, 16)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;
    MaterialPropertyBlock mpb;

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
        if (mpb == null)
            mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}