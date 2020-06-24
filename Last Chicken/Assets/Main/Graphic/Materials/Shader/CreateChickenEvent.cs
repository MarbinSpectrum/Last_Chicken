using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), DisallowMultipleComponent, ExecuteInEditMode]
public class CreateChickenEvent : MonoBehaviour
{
    private MaterialPropertyBlock materialProperties;
    public SpriteRenderer Renderer { get; private set; }

    [SerializeField] private float glowBrightness = 2f;
    [SerializeField] private Color glowColor = Color.white;
    [SerializeField] [Range(0, 1)] private float size = 0;
    [SerializeField] [Range(0, 1)] private float alphaCutOff = 0;
    [SerializeField] private bool imgAlpha = false;

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        SetMaterialProperties();
    }

    private void OnDisable()
    {
        SetMaterialProperties();
    }

    private void OnValidate()
    {
        if (!isActiveAndEnabled) return;
        SetMaterialProperties();
    }

    private void OnDidApplyAnimationProperties()
    {
        SetMaterialProperties();
    }

    public void SetMaterialProperties()
    {
        if (!Renderer) return;

        Renderer.sharedMaterial = ChickenEventMat.GetSharedFor(this);

        if (materialProperties == null) // Initializing it at `Awake` or `OnEnable` causes nullref in editor in some cases.
            materialProperties = new MaterialPropertyBlock();

        materialProperties.SetColor(Shader.PropertyToID("_InLightColor"), glowColor * glowBrightness);
        materialProperties.SetFloat(Shader.PropertyToID("_InLightSize"), size);
        materialProperties.SetFloat(Shader.PropertyToID("_AlphaCutOff"), alphaCutOff);
        materialProperties.SetFloat(Shader.PropertyToID("_ImgAlpha"), imgAlpha ? 1 : 0);

        Renderer.SetPropertyBlock(materialProperties);
    }

}